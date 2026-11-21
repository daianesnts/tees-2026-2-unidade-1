import hashlib
import hmac
import json
import os
from datetime import datetime
from http.server import BaseHTTPRequestHandler, HTTPServer

PORTA = int(os.environ.get("RECEPTOR_PORT", "8001"))
WEBHOOK_SECRET = os.environ.get("WEBHOOK_SECRET", "")

# Armazenamento em memória dos eventos registrados e dos IDs já processados (idempotência)
eventos_registrados = []
ids_processados = set()


def calcular_assinatura(corpo_bytes: bytes, segredo: str) -> str:
    digest = hmac.new(
        segredo.encode("utf-8"),
        corpo_bytes,
        hashlib.sha256,
    ).hexdigest()
    return f"sha256={digest}"


class ReceptorHandler(BaseHTTPRequestHandler):
    def _responder_json(self, status_code: int, payload: dict | list):
        corpo = json.dumps(payload, ensure_ascii=False, indent=2).encode("utf-8")
        self.send_response(status_code)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(corpo)))
        self.end_headers()
        self.wfile.write(corpo)

    def do_GET(self):
        if self.path == "/eventos":
            self._responder_json(200, eventos_registrados)
        else:
            self._responder_json(404, {"erro": "Endpoint não encontrado"})

    def do_POST(self):
        if self.path != "/webhook":
            self._responder_json(404, {"erro": "Endpoint não encontrado"})
            return

        tamanho = int(self.headers.get("Content-Length", 0))
        corpo_bytes = self.rfile.read(tamanho)

        # 1. Validação da assinatura HMAC-SHA256 sobre os bytes brutos (antes do parse JSON)
        assinatura_recebida = self.headers.get("X-Assinatura", "")
        assinatura_esperada = calcular_assinatura(corpo_bytes, WEBHOOK_SECRET)

        if not assinatura_recebida or not hmac.compare_digest(
            assinatura_recebida, assinatura_esperada
        ):
            print(
                f"[{datetime.now().strftime('%H:%M:%S')}] [RECEPTOR] 401 Unauthorized - "
                f"Assinatura ausente ou inválida (X-Assinatura: {assinatura_recebida!r})"
            )
            self._responder_json(
                401, {"erro": "Assinatura ausente ou inválida. Evento recusado."}
            )
            return

        # 2. Conversão do JSON somente após a assinatura ser validada
        try:
            evento = json.loads(corpo_bytes.decode("utf-8"))
        except json.JSONDecodeError:
            self._responder_json(400, {"erro": "JSON inválido"})
            return

        evento_id = evento.get("id")
        pedido_id = evento.get("pedido_id")
        status = evento.get("status")

        if not evento_id or not pedido_id or not status:
            self._responder_json(
                400, {"erro": "Campos obrigatórios: id, pedido_id, status"}
            )
            return

        # 3. Verificação de Idempotência pelo ID do evento
        if evento_id in ids_processados:
            print(
                f"[{datetime.now().strftime('%H:%M:%S')}] [RECEPTOR] 200 OK (Duplicado ignorado) - "
                f"Evento {evento_id} (pedido={pedido_id}, status={status}) já foi registrado anteriormente."
            )
            self._responder_json(
                200,
                {
                    "mensagem": "Evento já processado anteriormente (idempotente)",
                    "id": evento_id,
                    "duplicado": True,
                },
            )
            return

        # 4. Registro do novo evento
        ids_processados.add(evento_id)
        registro = {
            "id": evento_id,
            "pedido_id": pedido_id,
            "status": status,
        }
        eventos_registrados.append(registro)

        print(
            f"[{datetime.now().strftime('%H:%M:%S')}] [RECEPTOR] 200 OK - "
            f"Evento registrado: {json.dumps(registro, ensure_ascii=False)}"
        )
        self._responder_json(
            200,
            {
                "mensagem": "Evento registrado com sucesso",
                "evento": registro,
                "duplicado": False,
            },
        )

    def log_message(self, format, *args):
        # Suprime o log padrão do BaseHTTPRequestHandler para manter o terminal limpo
        pass


if __name__ == "__main__":
    if not WEBHOOK_SECRET:
        print(
            "ERRO: A variável de ambiente WEBHOOK_SECRET não foi definida.\n"
            "Defina antes de iniciar. Exemplo no PowerShell:\n"
            '  $env:WEBHOOK_SECRET="meu_segredo_ficticio_123"\n'
            "  python receptor.py"
        )
        raise SystemExit(1)

    servidor = HTTPServer(("127.0.0.1", PORTA), ReceptorHandler)
    print(f"[RECEPTOR] Servidor ouvindo em http://localhost:{PORTA}")
    print(f"[RECEPTOR] Endpoints disponíveis:")
    print(f"  - POST http://localhost:{PORTA}/webhook")
    print(f"  - GET  http://localhost:{PORTA}/eventos")
    try:
        servidor.serve_forever()
    except KeyboardInterrupt:
        print("\n[RECEPTOR] Encerrando servidor...")
        servidor.server_close()

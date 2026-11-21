import hashlib
import hmac
import json
import os
import threading
import time
import urllib.error
import urllib.request
from datetime import datetime
from http.server import BaseHTTPRequestHandler, HTTPServer

PORTA = int(os.environ.get("EMISSOR_PORT", "8000"))
WEBHOOK_URL = os.environ.get("WEBHOOK_URL", "http://localhost:8001/webhook")
WEBHOOK_SECRET = os.environ.get("WEBHOOK_SECRET", "")
INTERVALO_SEGUNDOS = float(os.environ.get("INTERVALO_SEGUNDOS", "3"))

SEQUENCIA_STATUS = ["recebido", "aprovado", "enviado", "entregue"]

lock = threading.Lock()
pedidos = {}
contador_eventos = 0
historico_eventos = {}


def proximo_id_evento() -> str:
    global contador_eventos
    contador_eventos += 1
    return f"evt_{contador_eventos}"


def calcular_assinatura(corpo_bytes: bytes, segredo: str) -> str:
    digest = hmac.new(
        segredo.encode("utf-8"),
        corpo_bytes,
        hashlib.sha256,
    ).hexdigest()
    return f"sha256={digest}"


def enviar_webhook(evento: dict, segredo_override: str | None = None) -> int:
    """Envia o evento para o POST /webhook do receptor com cabeçalho X-Assinatura."""
    corpo_bytes = json.dumps(evento, ensure_ascii=False).encode("utf-8")
    segredo = WEBHOOK_SECRET if segredo_override is None else segredo_override
    assinatura = calcular_assinatura(corpo_bytes, segredo)

    req = urllib.request.Request(
        WEBHOOK_URL,
        data=corpo_bytes,
        headers={
            "Content-Type": "application/json; charset=utf-8",
            "X-Assinatura": assinatura,
        },
        method="POST",
    )

    try:
        with urllib.request.urlopen(req, timeout=5) as resp:
            status_http = resp.getcode()
            print(
                f"[{datetime.now().strftime('%H:%M:%S')}] [EMISSOR] Webhook enviado -> "
                f"evento={evento['id']} pedido={evento['pedido_id']} status={evento['status']} "
                f"| Resposta do receptor: HTTP {status_http}"
            )
            return status_http
    except urllib.error.HTTPError as e:
        print(
            f"[{datetime.now().strftime('%H:%M:%S')}] [EMISSOR] Webhook recusado -> "
            f"evento={evento['id']} | Resposta do receptor: HTTP {e.code}"
        )
        return e.code
    except Exception as e:
        print(
            f"[{datetime.now().strftime('%H:%M:%S')}] [EMISSOR] Erro ao enviar webhook "
            f"do evento {evento['id']}: {e}"
        )
        return 0


def processamento_segundo_plano():
    """Avança o status de cada pedido em intervalos regulares e notifica o receptor."""
    while True:
        time.sleep(INTERVALO_SEGUNDOS)
        eventos_para_disparar = []

        with lock:
            for pedido_id, dados in pedidos.items():
                indice_atual = dados["indice_status"]
                if indice_atual + 1 < len(SEQUENCIA_STATUS):
                    novo_indice = indice_atual + 1
                    novo_status = SEQUENCIA_STATUS[novo_indice]
                    dados["indice_status"] = novo_indice
                    dados["status"] = novo_status

                    evento_id = proximo_id_evento()
                    evento = {
                        "id": evento_id,
                        "pedido_id": pedido_id,
                        "status": novo_status,
                    }
                    historico_eventos[evento_id] = evento
                    eventos_para_disparar.append(evento)
                    print(
                        f"[{datetime.now().strftime('%H:%M:%S')}] [EMISSOR] Pedido '{pedido_id}' "
                        f"avançou para status '{novo_status}' (evento {evento_id})"
                    )

        for evento in eventos_para_disparar:
            enviar_webhook(evento)


class EmissorHandler(BaseHTTPRequestHandler):
    def _responder_json(self, status_code: int, payload: dict | list):
        corpo = json.dumps(payload, ensure_ascii=False, indent=2).encode("utf-8")
        self.send_response(status_code)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.send_header("Content-Length", str(len(corpo)))
        self.end_headers()
        self.wfile.write(corpo)

    def do_GET(self):
        if self.path == "/pedidos":
            with lock:
                lista = [
                    {"pedido_id": pid, "status": d["status"]}
                    for pid, d in pedidos.items()
                ]
            self._responder_json(200, lista)
        else:
            self._responder_json(404, {"erro": "Endpoint não encontrado"})

    def do_POST(self):
        # Endpoint principal: POST /pedidos/{pedido_id}
        if self.path.startswith("/pedidos/"):
            pedido_id = self.path[len("/pedidos/") :].strip("/")
            if not pedido_id:
                self._responder_json(400, {"erro": "Informe o pedido_id na URL"})
                return

            with lock:
                pedidos[pedido_id] = {
                    "pedido_id": pedido_id,
                    "status": "recebido",
                    "indice_status": 0,
                }

            print(
                f"[{datetime.now().strftime('%H:%M:%S')}] [EMISSOR] Pedido '{pedido_id}' "
                f"cadastrado com status 'recebido'"
            )
            self._responder_json(
                201,
                {
                    "pedido_id": pedido_id,
                    "status": "recebido",
                    "mensagem": "Pedido cadastrado com sucesso. O processamento em segundo plano avançará o status.",
                },
            )
            return

        # Endpoint auxiliar para reenviar um mesmo evento (teste de idempotência)
        if self.path.startswith("/reenviar/"):
            evento_id = self.path[len("/reenviar/") :].strip("/")
            with lock:
                evento = historico_eventos.get(evento_id)
            if not evento:
                self._responder_json(404, {"erro": f"Evento {evento_id} não encontrado"})
                return

            status_http = enviar_webhook(evento)
            self._responder_json(
                200,
                {
                    "mensagem": f"Reentrega do evento {evento_id} enviada ao receptor",
                    "evento": evento,
                    "status_resposta_receptor": status_http,
                },
            )
            return

        self._responder_json(404, {"erro": "Endpoint não encontrado"})

    def log_message(self, format, *args):
        pass


if __name__ == "__main__":
    if not WEBHOOK_SECRET:
        print(
            "ERRO: A variável de ambiente WEBHOOK_SECRET não foi definida.\n"
            "Defina antes de iniciar. Exemplo no PowerShell:\n"
            '  $env:WEBHOOK_SECRET="meu_segredo_ficticio_123"\n'
            "  python emissor.py"
        )
        raise SystemExit(1)

    worker = threading.Thread(target=processamento_segundo_plano, daemon=True)
    worker.start()

    servidor = HTTPServer(("127.0.0.1", PORTA), EmissorHandler)
    print(f"[EMISSOR] Servidor ouvindo em http://localhost:{PORTA}")
    print(f"[EMISSOR] Webhook de destino: {WEBHOOK_URL}")
    print(f"[EMISSOR] Intervalo de avanço de status: {INTERVALO_SEGUNDOS}s")
    print(f"[EMISSOR] Endpoints disponíveis:")
    print(f"  - POST http://localhost:{PORTA}/pedidos/{{pedido_id}}")
    print(f"  - POST http://localhost:{PORTA}/reenviar/{{evento_id}} (teste de idempotência)")
    try:
        servidor.serve_forever()
    except KeyboardInterrupt:
        print("\n[EMISSOR] Encerrando servidor...")
        servidor.server_close()

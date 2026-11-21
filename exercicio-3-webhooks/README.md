# Atividade 03 - Webhooks

Implementação das duas pontas de uma arquitetura orientada a eventos utilizando **Webhooks**, composta por um **Emissor** e um **Receptor** executados em processos separados, incluindo **Assinatura HMAC-SHA256** e **Idempotência**.

---

## 1. Visão Geral: Emissor e Receptor

- **Emissor (`emissor/emissor.py`)**: Serviço de pedidos que roda na porta `8000`. Ele expõe o endpoint `POST /pedidos/{pedido_id}` para cadastrar novos pedidos com o status inicial `"recebido"`. Uma rotina em segundo plano (*background thread*) avança automaticamente o status de cada pedido em intervalos regulares de 3 segundos na sequência:
  `recebido` ➔ `aprovado` ➔ `enviado` ➔ `entregue`.
- **Receptor (`receptor/receptor.py`)**: Serviço consumidor de notificações que roda na porta `8001`. Ele recebe as notificações via `POST /webhook`, valida a autenticidade e integridade da mensagem via assinatura HMAC-SHA256 (`X-Assinatura`), garante a idempotência verificando o `id` único de cada evento e disponibiliza a listagem de todos os eventos registrados em `GET /eventos`.

---

## 2. Evento que Dispara a Notificação

A notificação de webhook é disparada automaticamente pelo **processamento em segundo plano do Emissor a cada mudança de status de um pedido**:
1. Quando o pedido passa de `recebido` para **`aprovado`**;
2. Quando o pedido passa de `aprovado` para **`enviado`**;
3. Quando o pedido passa de `enviado` para **`entregue`**.

Cada mudança de status gera um **novo evento** com seu próprio identificador único (`evt_1`, `evt_2`, `evt_3`, etc.). Caso ocorra uma reentrega do mesmo evento, o `id` do evento permanece o mesmo.

---

## 3. Endpoint Receptor, Cabeçalhos e Payload Enviado

- **Endpoint que recebe o webhook**: `POST http://localhost:8001/webhook`
- **Cabeçalhos enviados pelo Emissor**:
  - `Content-Type: application/json; charset=utf-8`
  - `X-Assinatura: sha256=<64 caracteres hexadecimais>` (HMAC-SHA256 calculado sobre os bytes brutos do corpo JSON utilizando o segredo compartilhado `WEBHOOK_SECRET`).

### Exemplo de Payload (JSON):

```json
{
  "id": "evt_1",
  "pedido_id": "ped_1",
  "status": "aprovado"
}
```

---

## 4. Como Subir os Dois Programas (Comandos na Ordem)

Os programas utilizam apenas a biblioteca padrão do **Python 3** (não é necessário instalar dependências externas). O segredo compartilhado é lido da variável de ambiente `WEBHOOK_SECRET` (abaixo usamos o valor fictício `segredo_ficticio_webhook_123`).

### Passo 1 — Terminal 1: Subir o Receptor (Porta 8001)

No **PowerShell**:
```powershell
cd exercicio-3-webhooks/receptor
$env:WEBHOOK_SECRET="segredo_ficticio_webhook_123"
python receptor.py
```

*(Ou no Linux/macOS/Git Bash)*:
```bash
cd exercicio-3-webhooks/receptor
export WEBHOOK_SECRET="segredo_ficticio_webhook_123"
python3 receptor.py
```

### Passo 2 — Terminal 2: Subir o Emissor (Porta 8000)

Em outro terminal, no **PowerShell**:
```powershell
cd exercicio-3-webhooks/emissor
$env:WEBHOOK_SECRET="segredo_ficticio_webhook_123"
python emissor.py
```

*(Ou no Linux/macOS/Git Bash)*:
```bash
cd exercicio-3-webhooks/emissor
export WEBHOOK_SECRET="segredo_ficticio_webhook_123"
python3 emissor.py
```

### Passo 3 — Terminal 3: Cadastrar um Pedido e Consultar os Eventos

1. **Criar o pedido `ped_1` no Emissor**:
   ```bash
   curl -X POST http://localhost:8000/pedidos/ped_1
   ```
2. **Aguardar cerca de 9 segundos** para o processamento em segundo plano avançar pelos status (`aprovado` ➔ `enviado` ➔ `entregue`) e consultar os eventos registrados no Receptor:
   ```bash
   curl http://localhost:8001/eventos
   ```

---

## 5. Evidência de Funcionamento (Ciclo Completo de um Pedido)

### Saída do Terminal do Emissor (`emissor.py`):
```text
[EMISSOR] Servidor ouvindo em http://localhost:8000
[EMISSOR] Webhook de destino: http://localhost:8001/webhook
[EMISSOR] Intervalo de avanço de status: 3.0s
[EMISSOR] Endpoints disponíveis:
  - POST http://localhost:8000/pedidos/{pedido_id}
  - POST http://localhost:8000/reenviar/{evento_id} (teste de idempotência)
[12:15:01] [EMISSOR] Pedido 'ped_1' cadastrado com status 'recebido'
[12:15:04] [EMISSOR] Pedido 'ped_1' avançou para status 'aprovado' (evento evt_1)
[12:15:04] [EMISSOR] Webhook enviado -> evento=evt_1 pedido=ped_1 status=aprovado | Resposta do receptor: HTTP 200
[12:15:07] [EMISSOR] Pedido 'ped_1' avançou para status 'enviado' (evento evt_2)
[12:15:07] [EMISSOR] Webhook enviado -> evento=evt_2 pedido=ped_1 status=enviado | Resposta do receptor: HTTP 200
[12:15:10] [EMISSOR] Pedido 'ped_1' avançou para status 'entregue' (evento evt_3)
[12:15:10] [EMISSOR] Webhook enviado -> evento=evt_3 pedido=ped_1 status=entregue | Resposta do receptor: HTTP 200
```

### Saída do Terminal do Receptor (`receptor.py`):
```text
[RECEPTOR] Servidor ouvindo em http://localhost:8001
[RECEPTOR] Endpoints disponíveis:
  - POST http://localhost:8001/webhook
  - GET  http://localhost:8001/eventos
[12:15:04] [RECEPTOR] 200 OK - Evento registrado: {"id": "evt_1", "pedido_id": "ped_1", "status": "aprovado"}
[12:15:07] [RECEPTOR] 200 OK - Evento registrado: {"id": "evt_2", "pedido_id": "ped_1", "status": "enviado"}
[12:15:10] [RECEPTOR] 200 OK - Evento registrado: {"id": "evt_3", "pedido_id": "ped_1", "status": "entregue"}
```

### Resposta do `GET http://localhost:8001/eventos` após o ciclo completo:
```json
[
  {
    "id": "evt_1",
    "pedido_id": "ped_1",
    "status": "aprovado"
  },
  {
    "id": "evt_2",
    "pedido_id": "ped_1",
    "status": "enviado"
  },
  {
    "id": "evt_3",
    "pedido_id": "ped_1",
    "status": "entregue"
  }
]
```

---

## 6. Pontuação Extra: Assinatura HMAC-SHA256 e Idempotência

### 6.1. Como funciona na implementação
1. **Assinatura HMAC-SHA256**:
   - O Emissor serializa o corpo em bytes UTF-8, calcula o HMAC-SHA256 com o segredo da variável de ambiente `WEBHOOK_SECRET` e envia no cabeçalho `X-Assinatura: sha256=<hex>`.
   - O Receptor lê os bytes brutos da requisição **antes** de converter o JSON, recalcula o HMAC-SHA256 esperado e utiliza comparação em tempo constante (`hmac.compare_digest`). Se o cabeçalho estiver ausente ou inválido, responde `401 Unauthorized` e não registra o evento.
2. **Idempotência**:
   - Cada mudança de status recebe um identificador de evento único (`evt_1`, `evt_2`, `evt_3`).
   - O Receptor mantém o conjunto `ids_processados`. Quando um evento com `id` já conhecido é recebido novamente, ele responde `200 OK` (2xx) sem duplicar o registro na lista de eventos.

---

### 6.2. Evidências das 3 Situações Extras

#### Situação 1: Assinatura válida aceita (`200 OK`)

Quando o Emissor envia o evento `evt_1` assinado com o mesmo `WEBHOOK_SECRET` configurado no Receptor:

- **Cabeçalho enviado**:
  `X-Assinatura: sha256=8f4a21b93bc46c50e5f2e90f74a3c3582c8971a13a73b4b5c6278042e91f1a0c`
- **Resposta do Receptor (`HTTP 200 OK`)**:
  ```json
  {
    "mensagem": "Evento registrado com sucesso",
    "evento": {
      "id": "evt_1",
      "pedido_id": "ped_1",
      "status": "aprovado"
    },
    "duplicado": false
  }
  ```

---

#### Situação 2: Assinatura errada ou ausente recusada com `401 Unauthorized`

Enviando uma requisição diretamente ao Receptor com uma assinatura incorreta:

- **Comando de teste**:
  ```bash
  curl -i -X POST http://localhost:8001/webhook \
    -H "Content-Type: application/json" \
    -H "X-Assinatura: sha256=0000000000000000000000000000000000000000000000000000000000000000" \
    -d '{"id": "evt_99", "pedido_id": "ped_1", "status": "aprovado"}'
  ```
- **Resposta HTTP do Receptor (`401 Unauthorized`)**:
  ```http
  HTTP/1.0 401 Unauthorized
  Content-Type: application/json; charset=utf-8

  {
    "erro": "Assinatura ausente ou inválida. Evento recusado."
  }
  ```
- **Saída no terminal do Receptor**:
  ```text
  [12:16:02] [RECEPTOR] 401 Unauthorized - Assinatura ausente ou inválida (X-Assinatura: 'sha256=0000000000000000000000000000000000000000000000000000000000000000')
  ```
*(O evento `evt_99` não é registrado em `GET /eventos`.)*

---

#### Situação 3: Mesmo evento enviado duas vezes e registrado uma única vez (Idempotência)

Solicitando ao Emissor a reentrega do evento `evt_1` (que já havia sido entregue):

- **Comando de teste**:
  ```bash
  curl -i -X POST http://localhost:8000/reenviar/evt_1
  ```
- **Saída no terminal do Emissor**:
  ```text
  [12:16:20] [EMISSOR] Webhook enviado -> evento=evt_1 pedido=ped_1 status=aprovado | Resposta do receptor: HTTP 200
  ```
- **Saída no terminal do Receptor**:
  ```text
  [12:16:20] [RECEPTOR] 200 OK (Duplicado ignorado) - Evento evt_1 (pedido=ped_1, status=aprovado) já foi registrado anteriormente.
  ```
- **Verificação em `GET http://localhost:8001/eventos`** (o evento `evt_1` aparece **apenas uma vez**):
  ```json
  [
    {
      "id": "evt_1",
      "pedido_id": "ped_1",
      "status": "aprovado"
    },
    {
      "id": "evt_2",
      "pedido_id": "ped_1",
      "status": "enviado"
    },
    {
      "id": "evt_3",
      "pedido_id": "ped_1",
      "status": "entregue"
    }
  ]
  ```

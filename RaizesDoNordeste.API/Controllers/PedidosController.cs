using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;
using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class PedidosController : ApiControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        /// <summary>
        /// Lista pedidos com filtros opcionais — Admin, Gerente, Cozinha ou Atendente.
        /// RF: filtro por canalPedido obrigatório no roteiro (item 5.5).
        /// Exemplo: GET /api/pedidos?canalPedido=Totem&amp;status=Confirmado&amp;pagina=1&amp;limite=10
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,Gerente,Cozinha,Atendente")]
        [ProducesResponseType(typeof(IEnumerable<PedidoResponse>), 200)]
        public async Task<IActionResult> Listar(
            [FromQuery] CanalPedido? canalPedido,
            [FromQuery] StatusPedido? status,
            [FromQuery] int? unidadeId,
            [FromQuery] int pagina = 1,
            [FromQuery] int limite = 10)
        {
            var resultado = await _pedidoService.ListarAsync(canalPedido, status, unidadeId, pagina, limite);
            return Ok(resultado.Data);
        }

        /// <summary>Lista pedidos do próprio cliente autenticado</summary>
        [HttpGet("meus-pedidos")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(typeof(IEnumerable<PedidoResponse>), 200)]
        public async Task<IActionResult> MeusPedidos()
        {
            var resultado = await _pedidoService.ListarPorUsuarioAsync(UsuarioLogadoId);
            return Ok(resultado.Data);
        }

        /// <summary>Detalha um pedido — dono do pedido ou equipe interna</summary>
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Gerente,Cozinha,Atendente,Cliente")]
        [ProducesResponseType(typeof(PedidoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _pedidoService.ObterPorIdAsync(id);
            if (!resultado.IsSuccess)
            {
                return StatusCode(404, new ErroResponse(
                    "PEDIDO_NAO_ENCONTRADO",
                    resultado.ErrorMessage ?? "O pedido solicitado não foi encontrado.",
                    HttpContext.Request.Path,
                    DateTime.UtcNow.AddHours(-3)));
            }

            return Ok(resultado.Data);
        }

        /// <summary>
        /// Cria um novo pedido — somente Cliente.
        /// RF: CanalPedido obrigatório, valida estoque e calcula total.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(typeof(PedidoResponse), 201)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        [ProducesResponseType(typeof(ErroResponse), 409)]
        [ProducesResponseType(typeof(ErroResponse), 422)]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoRequest request)
        {
            var resultado = await _pedidoService.CriarAsync(UsuarioLogadoId, request);

            if (!resultado.IsSuccess)
            {
                var msgErro = resultado.ErrorMessage ?? string.Empty;

                if (msgErro.Contains("não encontrado", StringComparison.OrdinalIgnoreCase) ||
                    msgErro.Contains("inexistente", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode(404, new ErroResponse(
                        "NAO_ENCONTRADO",
                        msgErro,
                        HttpContext.Request.Path,
                        DateTime.UtcNow.AddHours(-3)));
                }

                if (msgErro.Contains("estoque", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode(409, new ErroResponse(
                        "ESTOQUE_INSUFICIENTE",
                        msgErro,
                        HttpContext.Request.Path,
                        DateTime.UtcNow.AddHours(-3)));
                }

                return ErroBadRequest(msgErro);
            }

            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data!.Id }, resultado.Data);
        }

        /// <summary>
        /// Atualiza status do pedido — Cozinha, Atendente ou Gerente.
        /// RF: fluxo cozinha → pronto → entregue / cancelado.
        /// </summary>
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin,Gerente,Cozinha,Atendente")]
        [ProducesResponseType(typeof(PedidoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] AtualizarStatusRequest request)
        {
            var resultado = await _pedidoService.AtualizarStatusAsync(id, request.NovoStatus);
            return Ok(resultado.Data);
        }

        /// <summary>
        /// Cancela um pedido — Cliente cancela o próprio, equipe cancela qualquer um.
        /// RF: cancelar pedido devolve estoque.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
        [ProducesResponseType(typeof(PedidoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _pedidoService.CancelarAsync(id);
            return Ok(resultado.Data);
        }
    }
}

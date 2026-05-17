using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class PagamentosController : ApiControllerBase
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentosController(IPagamentoService pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }

        /// <summary>
        /// Processa pagamento via mock — somente Cliente dono do pedido.
        /// RF: solicitação de pagamento via serviço externo (mock) + registro.
        /// Simula envio ao gateway e retorna status (aprovado/recusado).
        /// </summary>
        [HttpPost("pedido/{pedidoId:int}")]
        [Authorize(Roles = "Cliente,Atendente")]
        [ProducesResponseType(typeof(PagamentoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        [ProducesResponseType(typeof(ErroResponse), 409)]
        public async Task<IActionResult> Processar(int pedidoId, [FromBody] ProcessarPagamentoRequest request)
        {
            var resultado = await _pagamentoService.ProcessarAsync(pedidoId, request);

            if (!resultado.IsSuccess)
            {
                var msgErro = resultado.ErrorMessage ?? string.Empty;

                if (msgErro.Contains("duplicado", StringComparison.OrdinalIgnoreCase) ||
                    msgErro.Contains("já processado", StringComparison.OrdinalIgnoreCase) ||
                    msgErro.Contains("já pago", StringComparison.OrdinalIgnoreCase))
                {
                    return StatusCode(409, new ErroResponse(
                        "PAGAMENTO_DUPLICADO",
                        msgErro,
                        HttpContext.Request.Path,
                        DateTime.UtcNow.AddHours(-3)));
                }

                return ErroBadRequest(msgErro);
            }

            return Ok(resultado.Data);
        }

        /// <summary>
        /// Consulta o pagamento de um pedido — equipe interna e o próprio cliente.
        /// </summary>
        [HttpGet("pedido/{pedidoId:int}")]
        [Authorize(Roles = "Admin,Gerente,Atendente,Cliente")]
        [ProducesResponseType(typeof(PagamentoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterPorPedido(int pedidoId)
        {
            var resultado = await _pagamentoService.ObterPorPedidoAsync(pedidoId);
            return Ok(resultado.Data);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class FidelidadeController : ApiControllerBase
    {
        private readonly IFidelidadeService _fidelidadeService;

        public FidelidadeController(IFidelidadeService fidelidadeService)
        {
            _fidelidadeService = fidelidadeService;
        }

        /// <summary>
        /// Consulta saldo de pontos do próprio cliente.
        /// RF: programa de fidelização — pontos e resgate simples.
        /// </summary>
        [HttpGet("saldo")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(typeof(FidelidadeResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterSaldo()
        {
            var resultado = await _fidelidadeService.ObterSaldoAsync(UsuarioLogadoId);
            return Ok(resultado.Data);
        }

        /// <summary>Consulta saldo de qualquer cliente — Admin ou Gerente</summary>
        [HttpGet("saldo/usuario/{usuarioId:Guid}")]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(FidelidadeResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterSaldoUsuario(Guid usuarioId)
        {
            var resultado = await _fidelidadeService.ObterSaldoAsync(usuarioId);
            return Ok(resultado.Data);
        }

        /// <summary>Lista histórico de transações do próprio cliente</summary>
        [HttpGet("transacoes")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(typeof(IEnumerable<TransacaoFidelidadeResponse>), 200)]
        public async Task<IActionResult> ListarTransacoes()
        {
            var resultado = await _fidelidadeService.ListarTransacoesAsync(UsuarioLogadoId);
            return Ok(resultado.Data);
        }

        /// <summary>
        /// Resgata pontos do próprio cliente.
        /// RF: resgate simples com consentimento.
        /// </summary>
        [HttpPost("resgatar")]
        [Authorize(Roles = "Cliente")]
        [ProducesResponseType(typeof(FidelidadeResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 409)]
        public async Task<IActionResult> Resgatar([FromBody] ResgatarPontosRequest request)
        {
            var resultado = await _fidelidadeService.ResgatarPontosAsync(UsuarioLogadoId, request.PontosResgatar);
            return Ok(resultado.Data);
        }
    }
}

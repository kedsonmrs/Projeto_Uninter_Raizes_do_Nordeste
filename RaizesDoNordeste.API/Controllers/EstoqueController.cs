using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class EstoqueController : ApiControllerBase
    {
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IEstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        /// <summary>
        /// Lista estoque completo de uma unidade — Admin, Gerente ou Atendente.
        /// RF: controle de estoque por unidade.
        /// </summary>
        [HttpGet("unidade/{unidadeId:int}")]
        [Authorize(Roles = "Admin,Gerente,Atendente")]
        [ProducesResponseType(typeof(IEnumerable<EstoqueResponse>), 200)]
        public async Task<IActionResult> ListarPorUnidade(int unidadeId)
        {
            var resultado = await _estoqueService.ListarPorUnidadeAsync(unidadeId);
            return Ok(resultado.Data);
        }

        /// <summary>Consulta saldo de um produto em uma unidade específica</summary>
        [HttpGet("produto/{produtoId:int}/unidade/{unidadeId:int}")]
        [Authorize(Roles = "Admin,Gerente,Atendente")]
        [ProducesResponseType(typeof(EstoqueResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterSaldo(int produtoId, int unidadeId)
        {
            var resultado = await _estoqueService.ObterSaldoAsync(produtoId, unidadeId);
            return Ok(resultado.Data);
        }

        /// <summary>
        /// Lista itens abaixo do mínimo de alerta — Admin ou Gerente.
        /// </summary>
        [HttpGet("unidade/{unidadeId:int}/alertas")]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(IEnumerable<EstoqueResponse>), 200)]
        public async Task<IActionResult> ListarAbaixoMinimo(int unidadeId)
        {
            var resultado = await _estoqueService.ListarAbaixoMinimoAsync(unidadeId);
            return Ok(resultado.Data);
        }

        /// <summary>
        /// Registra entrada de estoque — Admin ou Gerente.
        /// RF: entrada de estoque.
        /// </summary>
        [HttpPost("entrada")]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(EstoqueResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> Entrada([FromBody] MovimentarEstoqueRequest request)
        {
            var resultado = await _estoqueService.EntradaAsync(request);
            return Ok(resultado.Data);
        }

        /// <summary>
        /// Registra saída manual de estoque — Admin ou Gerente.
        /// RF: saída de estoque.
        /// </summary>
        [HttpPost("saida")]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(EstoqueResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 409)]
        public async Task<IActionResult> Saida([FromBody] MovimentarEstoqueRequest request)
        {
            var resultado = await _estoqueService.SaidaAsync(request);
            return Ok(resultado.Data);
        }
    }
}

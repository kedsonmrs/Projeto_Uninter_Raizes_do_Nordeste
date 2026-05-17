using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Implementations;
using RaizesDoNordeste.Application.Services.Interfaces;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class ProdutosController : ApiControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        /// <summary>
        /// Lista o cardápio de uma unidade — qualquer autenticado.
        /// RF: Visualização/consulta de cardápio por unidade.
        /// </summary>
        [HttpGet("unidade/{unidadeId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), 200)]
        public async Task<IActionResult> ListarPorUnidade(
            int unidadeId,
            [FromQuery] bool somenteDisponiveis = true)
        {
            var resultado = await _produtoService.ListarPorUnidadeAsync(unidadeId, somenteDisponiveis);
            return Ok(resultado.Data);
        }

        /// <summary>Detalha um produto</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProdutoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _produtoService.ObterPorIdAsync(id);
            return Ok(resultado.Data);
        }

        /// <summary>Cria produto no cardápio — somente Admin ou Gerente</summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(ProdutoResponse), 201)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> Criar([FromBody] CriarProdutoRequest request)
        {
            var resultado = await _produtoService.CriarAsync(request);
            if (!resultado.IsSuccess)
            {
                return ErroBadRequest(resultado.ErrorMessage!);
            }
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data!.Id }, resultado.Data);
        }

        /// <summary>Atualiza produto — somente Admin ou Gerente</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(ProdutoResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CriarProdutoRequest request)
        {
            var resultado = await _produtoService.AtualizarAsync(id, request);
            return Ok(resultado.Data);
        }

        /// <summary>
        /// Altera disponibilidade do produto no cardápio — Admin, Gerente ou Atendente.
        /// RF: restrição de venda por indisponibilidade.
        /// </summary>
        [HttpPatch("{id:int}/disponibilidade")]
        [Authorize(Roles = "Admin,Gerente,Atendente")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> AlterarDisponibilidade(int id, [FromQuery] bool disponivel)
        {
            await _produtoService.AlterarDisponibilidadeAsync(id, disponivel);
            return NoContent();
        }
    }
}

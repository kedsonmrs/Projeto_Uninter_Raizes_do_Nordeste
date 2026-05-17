using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class UnidadesController : ApiControllerBase
    {
        private readonly IUnidadeService _unidadeService;

        public UnidadesController(IUnidadeService unidadeService)
        {
            _unidadeService = unidadeService;
        }
        /// <summary>Lista todas as unidades ativas — qualquer usuário autenticado</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UnidadeResponse>), 200)]
        public async Task<IActionResult> Listar()
        {
            var resultado = await _unidadeService.ListarAtivasAsync();
            return Ok(resultado.Data);
        }

        /// <summary>Detalha uma unidade</summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UnidadeResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _unidadeService.ObterPorIdAsync(id);
            return Ok(resultado.Data);
        }

        /// <summary>Cria uma nova unidade — somente Admin ou Gerente</summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(UnidadeResponse), 201)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 403)]
        public async Task<IActionResult> Criar([FromBody] CriarUnidadeRequest request)
        {
            var resultado = await _unidadeService.CriarAsync(request);
            if (!resultado.IsSuccess)
            {
                return ErroBadRequest(resultado.ErrorMessage!);
            }
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Data!.Id }, resultado.Data);
        }

        /// <summary>Atualiza dados de uma unidade — somente Admin ou Gerente</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Gerente")]
        [ProducesResponseType(typeof(UnidadeResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CriarUnidadeRequest request)
        {
            var resultado = await _unidadeService.AtualizarAsync(id, request);
            return Ok(resultado.Data);
        }

        /// <summary>Desativa uma unidade — somente Admin</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErroResponse), 404)]
        public async Task<IActionResult> Desativar(int id)
        {
            await _unidadeService.DesativarAsync(id);
            return NoContent();
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.ResponseViewModel;
using System.Security.Claims;

namespace RaizesDoNordeste.API.Controllers
{
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public abstract class ApiControllerBase : ControllerBase
    {
        
        protected Guid UsuarioLogadoId =>
            Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
      
        protected string UsuarioLogadoRole =>
            User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
       
        protected IActionResult ErroNotFound(string mensagem) =>
            NotFound(new ErroResponse(
                "NAO_ENCONTRADO", mensagem,
                HttpContext.Request.Path, DateTime.UtcNow.AddHours(-3)));

        protected IActionResult ErroBadRequest(string mensagem, List<DetalheErro>? detalhes = null) =>
            BadRequest(new ErroResponse(
                "REQUISICAO_INVALIDA", mensagem,
                HttpContext.Request.Path, DateTime.UtcNow.AddHours(-3), detalhes));

        protected IActionResult ErroConflito(string mensagem, List<DetalheErro>? detalhes = null) =>
            Conflict(new ErroResponse(
                "CONFLITO", mensagem,
                HttpContext.Request.Path, DateTime.UtcNow.AddHours(-3), detalhes));

        protected IActionResult ErroForbidden() =>
            StatusCode(403, new ErroResponse(
                "SEM_PERMISSAO", "Você não tem permissão para esta ação.",
                HttpContext.Request.Path, DateTime.UtcNow.AddHours(-3)));
    }
}

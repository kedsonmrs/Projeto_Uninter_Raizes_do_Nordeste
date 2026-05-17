using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaizesDoNordeste.Application.RequestViewModel;
using RaizesDoNordeste.Application.ResponseViewModel;
using RaizesDoNordeste.Application.Services.Interfaces;

namespace RaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Cadastra um novo cliente</summary>
        [HttpPost("cadastro")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UsuarioResumo), 201)]
        [ProducesResponseType(typeof(ErroResponse), 400)]
        [ProducesResponseType(typeof(ErroResponse), 409)]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarUsuarioRequest request)
        {
            var resultado = await _authService.CadastrasAsync(request);

            if (!resultado.IsSuccess)
            {
                return ErroBadRequest(resultado.ErrorMessage!);
            }

            return CreatedAtAction(nameof(Cadastrar), resultado.Data);
        }

        /// <summary>Autentica e retorna JWT</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), 200)]
        [ProducesResponseType(typeof(ErroResponse), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var resultado = await _authService.LoginAsync(request);

            if (!resultado.IsSuccess)
            {
                return StatusCode(401, new ErroResponse(
                    "CREDENCIAIS_INVALIDAS",
                    resultado.ErrorMessage!,
                    HttpContext.Request.Path,
                    DateTime.UtcNow.AddHours(-3)));
            }

            return Ok(resultado.Data);
        }
    }
}

namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record LoginResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    UsuarioResumo Usuario);
}

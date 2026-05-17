namespace RaizesDoNordeste.Application.RequestViewModel
{
    public record CadastrarUsuarioRequest(
        string Nome,
        string Email,
        string Senha,
        bool ConsentimentoLGPD,
        string? Telefone = null
        );

}

namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record UnidadeResponse(
    int Id,
    string Nome,
    string Endereco,
    string? Telefone,
    bool Ativa);
}

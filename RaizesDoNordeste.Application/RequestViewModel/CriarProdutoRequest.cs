namespace RaizesDoNordeste.Application.RequestViewModel
{
    public record CriarProdutoRequest(
    int UnidadeId,
    string Nome,
    string? Descricao,
    decimal Preco,
    string Categoria);
}

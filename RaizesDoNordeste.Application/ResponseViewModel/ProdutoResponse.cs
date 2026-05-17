namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record ProdutoResponse(
    int Id,
    int UnidadeId,
    string Nome,
    string? Descricao,
    decimal Preco,
    string Categoria,
    bool Disponivel);
}

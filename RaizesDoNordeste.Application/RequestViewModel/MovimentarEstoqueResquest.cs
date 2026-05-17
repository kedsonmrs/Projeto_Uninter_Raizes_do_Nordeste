namespace RaizesDoNordeste.Application.RequestViewModel
{
    public record MovimentarEstoqueRequest(
    int ProdutoId,
    int UnidadeId,
    int Quantidade, 
    string? Motivo = null);
}

namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record EstoqueResponse(
    int ProdutoId,
    string NomeProduto,
    int UnidadeId,
    int Quantidade,
    int MinimoAlerta,
    bool AbaixoMinimo);
}

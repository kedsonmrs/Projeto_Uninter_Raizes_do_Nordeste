namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record ItemPedidoResponse(
    int ProdutoId,
    string NomeProduto,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal);
}

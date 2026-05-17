namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record PedidoResponse(
    int Id,
    string Status,
    string CanalPedido,
    decimal Total,
    DateTime CriadoEm,
    List<ItemPedidoResponse> Itens);
}

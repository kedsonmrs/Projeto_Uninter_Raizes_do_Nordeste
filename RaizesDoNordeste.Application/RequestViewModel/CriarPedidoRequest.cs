using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.Application.RequestViewModel
{
    public record CriarPedidoRequest(
    int UnidadeId,
    CanalPedido? CanalPedido,     
    List<ItemPedidoRequest> Itens,
    string? Observacao = null);
}

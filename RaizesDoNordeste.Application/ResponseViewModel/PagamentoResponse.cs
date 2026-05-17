namespace RaizesDoNordeste.Application.ResponseViewModel
{
    public record PagamentoResponse(
    Guid Id,
    int PedidoId,
    string Metodo,
    string Status,
    decimal Valor,
    string? ReferenciaExterna,
    string? MensagemRetorno,
    DateTime CriadoEm);
}

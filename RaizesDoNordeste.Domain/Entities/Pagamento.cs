using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.Domain.Entities
{
    public class Pagamento : BaseEntity<Guid>    
    {
        public int PedidoId { get; set; }
        public MetodoPagamento Metodo { get; set; }
        public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;
        public decimal Valor { get; set; }
        public string? ReferenciaExterna { get; set; }
        public string? MensagemRetorno { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow.AddHours(-3);
        public DateTime? ProcessadoEm { get; set; }

        public Pedido Pedido { get; set; }

    }
}

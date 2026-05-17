using RaizesDoNordeste.Domain.Enum;


namespace RaizesDoNordeste.Domain.Entities
{
    public class Pedido : BaseEntity<int>
    {
        public Guid UsuarioId { get; set; }
        public int UnidadeId { get; set; }
        public CanalPedido CanalPedido { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.AguardandoPagamento;
        public decimal Total { get; set; }
        public string? Observacao { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow.AddHours(-3);
        public DateTime? AtualizadoEm { get; set; }

        public Usuario Usuario { get; set; }
        public Unidade Unidade { get; set; }
        public ICollection<ItemPedido> Itens { get; set; } = [];
        public Pagamento? Pagamento { get; set; }
    }
}

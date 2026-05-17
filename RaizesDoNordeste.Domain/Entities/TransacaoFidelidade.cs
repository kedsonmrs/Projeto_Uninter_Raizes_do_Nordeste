using RaizesDoNordeste.Domain.Enum;

namespace RaizesDoNordeste.Domain.Entities
{
    public class TransacaoFidelidade : BaseEntity<int>
    {
        public Guid UsuarioId { get; set; }
        public int? PedidoId { get; set; }
        public TipoTransacaoFidelidade Tipo { get; set; }
        public int Pontos { get; set; }
        public string? Descricao { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow.AddHours(-3);

        public Usuario Usuario { get; set; } = null!;
        public Pedido? Pedido { get; set; }
    }
}

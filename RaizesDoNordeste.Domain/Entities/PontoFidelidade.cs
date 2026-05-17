namespace RaizesDoNordeste.Domain.Entities
{
    public class PontoFidelidade : BaseEntity<int>
    {
        public Guid UsuarioId { get; set; }
        public int PontosTotal { get; set; } = 0;
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;

        public Usuario Usuario { get; set; } = null!;
        public ICollection<TransacaoFidelidade> Transacoes { get; set; } = [];
    }
}

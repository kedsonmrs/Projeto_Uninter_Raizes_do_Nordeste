namespace RaizesDoNordeste.Domain.Entities
{
    public class Produto : BaseEntity<int>
    {
        public int UnidadeId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public bool Disponivel { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow.AddHours(-3);
        public DateTime? AtualizadoEm { get; set; }

        public Unidade Unidade { get; set; }
        public ICollection<ItemPedido> ItensPedidos { get; set; } = [];
        public Estoque? Estoque { get; set; }
    }
}

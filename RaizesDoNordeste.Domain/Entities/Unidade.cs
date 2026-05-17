namespace RaizesDoNordeste.Domain.Entities
{
    public class Unidade : BaseEntity<int>
    {
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public bool Ativa { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow.AddHours(-3);

        public ICollection<Produto> Produtos { get; set; } = [];
        public ICollection<Pedido> Pedidos { get; set; } = [];
        public ICollection<Estoque> Estoques { get; set; } = [];

    }
}

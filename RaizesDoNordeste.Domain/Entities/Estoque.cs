namespace RaizesDoNordeste.Domain.Entities
{
    public class Estoque : BaseEntity<int>
    {
        public int ProdutoId { get; set; }
        public int UnidadeId { get; set; }
        public int Quantidade { get; set; }
        public int MinimoAlerta { get; set; } = 5;
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow.AddHours(-3);

        public Produto Produtos { get; set; } = null!;
        public Unidade Unidades { get; set; } = null!;

        public bool PossuiEstoque(int qtd) => Quantidade >= qtd;

    }
}

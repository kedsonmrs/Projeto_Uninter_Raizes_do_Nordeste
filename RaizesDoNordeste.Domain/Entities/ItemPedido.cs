namespace RaizesDoNordeste.Domain.Entities
{
    public class ItemPedido : BaseEntity<int>
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal SubTotal => Quantidade * PrecoUnitario;

        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }
    }
}

using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class ItemPedidoValidator : AbstractValidator<ItemPedido>
    {
        public ItemPedidoValidator()
        {
            RuleFor(i => i.PedidoId)
                .GreaterThan(0).WithMessage("Pedido inválido.");

            RuleFor(i => i.ProdutoId)
                .GreaterThan(0).WithMessage("Produto inválido.");

            RuleFor(i => i.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");

            RuleFor(i => i.PrecoUnitario)
                .GreaterThanOrEqualTo(0).WithMessage("O preço unitário não pode ser negativo.");
        }
    }
}

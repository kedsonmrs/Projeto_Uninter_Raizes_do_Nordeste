using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class PedidoValidator : AbstractValidator<Pedido>
    {
        public PedidoValidator()
        {
            RuleFor(p => p.UsuarioId)
                .NotEmpty().WithMessage("Usuário inválido.");

            RuleFor(p => p.UnidadeId)
                .GreaterThan(0).WithMessage("Unidade inválida.");

            RuleFor(p => p.CanalPedido)
                .IsInEnum().WithMessage("Canal de pedido inválido.");

            RuleFor(p => p.Status)
                .IsInEnum().WithMessage("Status do pedido inválido.");

            RuleFor(p => p.Total)
                .GreaterThanOrEqualTo(0).WithMessage("O valor total não pode ser negativo.");

            RuleFor(p => p.Observacao)
                .MaximumLength(300).WithMessage("A observação deve ter no máximo 300 caracteres.");
        }
    }
}

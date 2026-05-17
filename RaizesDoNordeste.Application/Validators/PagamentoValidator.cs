using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class PagamentoValidator : AbstractValidator<Pagamento>
    {
        public PagamentoValidator()
        {
            RuleFor(p => p.PedidoId)
                .GreaterThan(0).WithMessage("Pedido inválido.");

            RuleFor(p => p.Metodo)
                .IsInEnum().WithMessage("Método de pagamento inválido.");

            RuleFor(p => p.Status)
                .IsInEnum().WithMessage("Status de pagamento inválido.");

            RuleFor(p => p.Valor)
                .GreaterThan(0).WithMessage("O valor do pagamento deve ser maior que zero.");

            RuleFor(p => p.ReferenciaExterna)
                .MaximumLength(255).WithMessage("A referência externa deve ter no máximo 255 caracteres.");
        }
    }
}

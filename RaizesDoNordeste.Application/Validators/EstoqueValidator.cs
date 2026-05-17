using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class EstoqueValidator : AbstractValidator<Estoque>
    {
        public EstoqueValidator()
        {
            RuleFor(e => e.ProdutoId)
                .GreaterThan(0).WithMessage("Produto inválido.");

            RuleFor(e => e.UnidadeId)
                .GreaterThan(0).WithMessage("Unidade inválida.");

            RuleFor(e => e.Quantidade)
                .GreaterThanOrEqualTo(0).WithMessage("A quantidade em estoque não pode ser negativa.");

            RuleFor(e => e.MinimoAlerta)
                .GreaterThanOrEqualTo(0).WithMessage("O alerta mínimo não pode ser negativo.");
        }
    }
}

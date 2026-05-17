using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class UnidadeValidator : AbstractValidator<Unidade>
    {
        public UnidadeValidator()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome da unidade é obrigatório.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

            RuleFor(u => u.Endereco)
                .NotEmpty().WithMessage("O endereço é obrigatório.")
                .MaximumLength(200).WithMessage("O endereço deve ter no máximo 200 caracteres.");

            RuleFor(u => u.Telefone)
                .MaximumLength(11).WithMessage("O telefone deve ter no máximo 11 caracteres.");
        }
    }
}

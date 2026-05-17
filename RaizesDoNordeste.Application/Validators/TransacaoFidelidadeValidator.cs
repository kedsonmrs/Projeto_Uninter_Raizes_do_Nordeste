using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class TransacaoFidelidadeValidator : AbstractValidator<TransacaoFidelidade>
    {
        public TransacaoFidelidadeValidator()
        {
            RuleFor(t => t.UsuarioId)
                .NotEmpty().WithMessage("Usuário inválido.");

            RuleFor(t => t.Tipo)
                .IsInEnum().WithMessage("Tipo de transação inválida.");

            RuleFor(t => t.Pontos)
                .NotEqual(0).WithMessage("A quantidade de pontos da transação não pode ser zero.");

            RuleFor(t => t.Descricao)
                .MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");
        }
    }
}

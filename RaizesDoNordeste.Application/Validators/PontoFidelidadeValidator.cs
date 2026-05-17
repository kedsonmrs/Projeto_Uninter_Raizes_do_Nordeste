using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class PontoFidelidadeValidator : AbstractValidator<PontoFidelidade>
    {
        public PontoFidelidadeValidator()
        {
            RuleFor(p => p.UsuarioId)
                .NotEmpty().WithMessage("Usuário inválido.");

            RuleFor(p => p.PontosTotal)
                .GreaterThanOrEqualTo(0).WithMessage("O total de pontos não pode ser negativo.");
        }
    }
}

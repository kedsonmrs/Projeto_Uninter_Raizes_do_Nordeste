using FluentValidation;
using RaizesDoNordeste.Domain.Entities;

namespace RaizesDoNordeste.Application.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("Formato de e-mail inválido.")
                .MaximumLength(150).WithMessage("O e-mail deve ter no máximo 150 caracteres.");

            RuleFor(u => u.SenhaHash)
                .NotEmpty().WithMessage("A senha é obrigatória.");

            RuleFor(u => u.Role)
                .IsInEnum().WithMessage("Perfil de usuário inválido.");

            RuleFor(u => u.ConsentimentoLGPD)
                .Equal(true).WithMessage("É obrigatório fornecer o consentimento para a LGPD.");
        }
    }
}

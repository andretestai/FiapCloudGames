using Core.Entities.InsertEntities;
using FluentValidation;

namespace Services.Services.Validator
{
    public class UserValidator : AbstractValidator<UserInsert>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email).EmailAddress().WithMessage("Formato invalido!");
            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
                .Must(CaraterEspecial)
                .WithMessage("A senha deve conter pelo menos um caractere especial.");
        }

        private bool CaraterEspecial(string password)
        {
            return password.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/`~".Contains(ch));
        }
    }
}

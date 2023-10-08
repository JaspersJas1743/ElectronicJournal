using ElectronicJournal.Models;
using FluentValidation;

namespace ElectronicJournal.Utilities.Validator
{
    public class AuthorizationModelValidator : AbstractValidator<AuthorizationModel>
    {
        public AuthorizationModelValidator() 
        {
            RuleFor(authorizationModel => authorizationModel.Login)
                .NotNull().WithMessage(errorMessage: "Поле \"Логин\" является обязательным")
                .MinimumLength(minimumLength: 4).WithMessage("Минимальная длина логина - 4 символа");

            RuleFor(authorizationModel => authorizationModel.Password)
                .NotNull().WithMessage(errorMessage: "Поле \"Пароль\" является обязательным")
                .MinimumLength(minimumLength: 6).WithMessage("Минимальная длина пароля - 6 символов");
        }
    }
}

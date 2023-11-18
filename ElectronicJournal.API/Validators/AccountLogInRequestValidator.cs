using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class AccountLogInRequestValidator : AbstractValidator<AccountController.LogInRequest>
    {
        public AccountLogInRequestValidator()
        {
            string msg = "Логин является обязательным к заполнению";
            RuleFor(expression: logIn => logIn.Login)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: l => !String.IsNullOrWhiteSpace(value: l)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 4).WithMessage(errorMessage: "Минимальная длина логина - 4 символа");

            msg = "Пароль является обязательным к заполнению";
            RuleFor(expression: logInData => logInData.Password)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов");
        }
    }
}

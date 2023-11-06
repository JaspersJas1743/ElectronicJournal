using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class AccountSignUpRequestValidator : AbstractValidator<AccountController.SignUpRequest>
    {
        public AccountSignUpRequestValidator()
        {
            RuleFor(expression: signUp => signUp.RegistrationCode)
                .NotNull().WithMessage(errorMessage: "Регистрационный код не может быть пустым")
                .Length(exactLength: 6).WithMessage(errorMessage: "Длина регистрационного кода должна быть равна 6");

            RuleFor(expression: signUp => signUp.Login)
                .NotNull().WithMessage(errorMessage: "Логин является обязательным к заполнению")
                .MinimumLength(minimumLength: 4).WithMessage("Минимальная длина логина - 4 символа");

            RuleFor(expression: signUp => signUp.Password)
                .NotNull().WithMessage(errorMessage: "Пароль является обязательным к заполнению")
                .MinimumLength(minimumLength: 6).WithMessage("Минимальная длина пароля - 6 символа");
        }
    }
}

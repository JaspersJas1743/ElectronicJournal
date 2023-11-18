using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class AccountSignUpRequestValidator : AbstractValidator<AccountController.SignUpRequest>
    {
        public AccountSignUpRequestValidator()
        {
            string msg = "Регистрационный код является обязательным к заполнению";
            RuleFor(expression: signUp => signUp.RegistrationCode)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: rc => !String.IsNullOrWhiteSpace(value: rc)).WithMessage(errorMessage: msg)
                .Length(exactLength: 6).WithMessage(errorMessage: "Длина регистрационного кода должна быть равна 6");

            msg = "Логин является обязательным к заполнению";
            RuleFor(expression: signUp => signUp.Login)
               .NotNull().WithMessage(errorMessage: msg)
               .NotEmpty().WithMessage(errorMessage: msg)
               .Must(predicate: l => !String.IsNullOrWhiteSpace(value: l)).WithMessage(errorMessage: msg)
               .MinimumLength(minimumLength: 4).WithMessage(errorMessage: "Минимальная длина логина - 4 символа");

            msg = "Пароль является обязательным к заполнению";
            RuleFor(expression: signUp => signUp.Password)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символа");
        }
    }
}

using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class AccountRegistrationCodeRequestValidator : AbstractValidator<AccountController.RegistrationCodeRequest>
    {
        public AccountRegistrationCodeRequestValidator()
        {
            string msg = "Регистрационный код является обязательным к заполнению";
            RuleFor(expression: registrationCode => registrationCode.RegistrationCode)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: rc => !String.IsNullOrWhiteSpace(value: rc)).WithMessage(errorMessage: msg)
                .Length(exactLength: 6).WithMessage(errorMessage: "Длина регистрационного кода равна 6");
        }
    }
}

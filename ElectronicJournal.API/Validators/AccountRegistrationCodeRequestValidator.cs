using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class AccountRegistrationCodeRequestValidator: AbstractValidator<AccountController.RegistrationCodeRequest>
    {
        public AccountRegistrationCodeRequestValidator() 
        {
            RuleFor(registrationCode => registrationCode.RegistrationCode)
                .NotNull().WithMessage(errorMessage: "Регистрационный код является обязательным к заполнению")
                .Length(exactLength: 6).WithMessage(errorMessage: "Длина регистрационного кода равна 6");
        }
    }
}

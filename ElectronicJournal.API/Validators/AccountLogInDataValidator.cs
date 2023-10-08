using FluentValidation;
using ElectronicJournal.API.Controllers;

namespace ElectronicJournal.API.Validators
{
    public class AccountLogInDataValidator: AbstractValidator<AccountController.LogInData>
    {
        public AccountLogInDataValidator() 
        {
            RuleFor(logIn => logIn.Login)
                .NotNull().WithMessage(errorMessage: "Логин является обязательным к заполнению")
                .MinimumLength(minimumLength: 4).WithMessage("Минимальная длина логина - 4 символа");

            RuleFor(logInData => logInData.Password)
                .NotNull().WithMessage(errorMessage: "Пароль является обязательным к заполнению")
                .MinimumLength(minimumLength: 6).WithMessage("Минимальная длина пароля - 6 символов");
        }
    }
}

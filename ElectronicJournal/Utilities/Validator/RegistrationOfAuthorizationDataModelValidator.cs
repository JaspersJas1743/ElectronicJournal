using ElectronicJournal.Models;
using FluentValidation;

namespace ElectronicJournal.Utilities.Validator
{
    public class RegistrationOfAuthorizationDataModelValidator : AbstractValidator<RegistrationOfAuthorizationDataModel>
    {
        public RegistrationOfAuthorizationDataModelValidator()
        {
            RuleFor(ROADM => ROADM.Login)
                .NotNull().WithMessage(errorMessage: "Поле \"Логин\" является обязательным")
                .MinimumLength(minimumLength: 4).WithMessage(errorMessage: "Минимальная длина логина - 4 символа");

            RuleFor(ROADM => ROADM.Password)
                .NotNull().WithMessage(errorMessage: "Поле \"Пароль\" является обязательным")
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов");

            RuleFor(ROADM => ROADM.PasswordConfirmation)
                .NotNull().WithMessage(errorMessage: "Поле \"Подтвердите пароль\" является обязательным")
                .Equal(expression: ROADM => ROADM.Password).WithMessage(errorMessage: "Поле \"Пароль\" и \"Подтвердите пароль\" не совпадают");
        }
    }
}

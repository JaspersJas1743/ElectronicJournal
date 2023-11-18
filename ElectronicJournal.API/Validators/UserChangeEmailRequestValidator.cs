using ElectronicJournal.API.Controllers;
using FluentValidation;
using FluentValidation.Validators;

namespace ElectronicJournal.API.Validators
{
    public class UserChangeEmailRequestValidator : AbstractValidator<UserController.ChangeEmailRequest>
    {
        public UserChangeEmailRequestValidator()
        {
            string msg = "Новый адрес электронной почты является обязательным к заполнению";
            RuleFor(expression: CER => CER.NewEmail)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: e => !String.IsNullOrWhiteSpace(value: e)).WithMessage(errorMessage: msg)
                .EmailAddress(mode: EmailValidationMode.Net4xRegex).WithMessage(errorMessage: "Некорректный формат электронной почты");
        }
    }
}

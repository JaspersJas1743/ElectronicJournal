using ElectronicJournal.API.Controllers;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ElectronicJournal.API.Validators
{
    public class UserChangePhoneRequestValidator : AbstractValidator<UserController.ChangePhoneRequest>
    {
        public UserChangePhoneRequestValidator() 
        {
            string msg = "Новый номер телефона является обязательным к заполнению";
            RuleFor(expression: CPR => CPR.NewPhone)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .Length(exactLength: 15).WithMessage(errorMessage: "Длина номера телефона равна 15")
                .Matches(regex: new Regex(pattern: "\\+7\\(\\d{3}\\)\\d{3}-\\d{4}")).WithMessage(errorMessage: "Номер телефона должен иметь формат +7(###)###-####");
        }
    }
}

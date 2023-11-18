using ElectronicJournal.Models;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace ElectronicJournal.Utilities.Validator
{
    public class ProfileModelPhoneValidator : AbstractValidator<ProfileModel.PhoneBlock>
    {
        public ProfileModelPhoneValidator()
        {
            string msg = "Поле \"Телефон\" является обязательным";
            RuleFor(expression: PB => PB.Phone)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .Length(exactLength: 15).WithMessage(errorMessage: "Длина номера телефона равна 15")
                .Matches(regex: new Regex(pattern: "\\+7\\(\\d{3}\\)\\d{3}-\\d{4}")).WithMessage(errorMessage: "Номер телефона должен иметь формат +7(###)###-####");
        }
    }
}

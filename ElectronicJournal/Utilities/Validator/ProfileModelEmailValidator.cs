using ElectronicJournal.Models;
using FluentValidation;
using FluentValidation.Validators;
using System;

namespace ElectronicJournal.Utilities.Validator
{
    public class ProfileModelEmailValidator : AbstractValidator<ProfileModel.EmailBlock>
    {
        public ProfileModelEmailValidator()
        {
            string msg = "Поле \"Email\" является обязательным";
            RuleFor(expression: EB => EB.Email)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: e => !String.IsNullOrWhiteSpace(value: e)).WithMessage(errorMessage: msg)
                .EmailAddress(mode: EmailValidationMode.Net4xRegex).WithMessage(errorMessage: "Некорректный формат электронной почты");
        }
    }
}

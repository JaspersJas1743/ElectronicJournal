using ElectronicJournal.Models;
using ElectronicJournal.Resources.CustomElements;
using FluentValidation;
using System;

namespace ElectronicJournal.Utilities.Validator
{
    public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
    {
        public RegistrationModelValidator() 
        {
            string msg = "Поле \"Регистрационный код\" является обязательным";
            RuleFor(expression: RM => RM.Code)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: c => !String.IsNullOrWhiteSpace(value: c)).WithMessage(errorMessage: msg)
                .Length(exactLength: CodeEntryPanel.MaxCountOfCell).WithMessage(errorMessage: "Длина регистрационного кода равна 6");
        }
    }
}

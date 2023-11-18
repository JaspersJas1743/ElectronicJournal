using ElectronicJournal.Models;
using FluentValidation;
using System;

namespace ElectronicJournal.Utilities.Validator
{
    public class RegistrationOfAuthorizationDataModelValidator : AbstractValidator<RegistrationOfAuthorizationDataModel>
    {
        public RegistrationOfAuthorizationDataModelValidator()
        {
            string msg = "Поле \"Логин\" является обязательным";
            RuleFor(expression: ROADM => ROADM.Login)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 4).WithMessage(errorMessage: "Минимальная длина логина - 4 символа");

            msg = "Поле \"Пароль\" является обязательным";
            RuleFor(expression: ROADM => ROADM.Password)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов");

            msg = "Поле \"Подтвердите пароль\" является обязательным";
            RuleFor(expression: ROADM => ROADM.PasswordConfirmation)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .Equal(expression: ROADM => ROADM.Password).WithMessage(errorMessage: "Поля \"Пароль\" и \"Подтвердите пароль\" не совпадают");
        }
    }
}

using ElectronicJournal.Models;
using FluentValidation;
using System;

namespace ElectronicJournal.Utilities.Validator
{
    public class AuthorizationModelValidator : AbstractValidator<AuthorizationModel>
    {
        public AuthorizationModelValidator()
        {
            string msg = "Поле \"Логин\" является обязательным";
            RuleFor(expression: AM => AM.Login)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: l => !String.IsNullOrWhiteSpace(value: l)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 4).WithMessage(errorMessage: "Минимальная длина логина - 4 символа");

            msg = "Поле \"Пароль\" является обязательным";
            RuleFor(expression: AM => AM.Password)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов");
        }
    }
}

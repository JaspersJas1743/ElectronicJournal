using ElectronicJournal.Models;
using FluentValidation;
using System;

namespace ElectronicJournal.Utilities.Validator
{
    public class ProfileModelSecurityValidator : AbstractValidator<ProfileModel.SecurityBlock>
    {
        public ProfileModelSecurityValidator()
        {
            string msg = "Поле \"Текущий пароль\" является обязательным";
            RuleFor(expression: SB => SB.CurrentPassword)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов");

            msg = "Поле \"Новый пароль\" является обязательным";
            RuleFor(expression: SB => SB.NewPassword)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов")
                .NotEqual(expression: SB => SB.CurrentPassword).WithMessage(errorMessage: "Новый пароль должен быть отличен от предыдущего");

            msg = "Поле \"Подтвердите пароль\" является обязательным";
            RuleFor(expression: SB => SB.ConfirmatedPassword)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов")
                .Equal(expression: SB => SB.NewPassword).WithMessage(errorMessage: "Поля \"Новый пароль\" и \"Подтвердите пароль\" не совпадают");
        }
    }
}

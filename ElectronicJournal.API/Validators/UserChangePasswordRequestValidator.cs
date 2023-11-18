using FluentValidation;
using ElectronicJournal.API.Controllers;

namespace ElectronicJournal.API.Validators
{
    public class UserChangePasswordRequestValidator : AbstractValidator<UserController.ChangePasswordRequest>
    {
        public UserChangePasswordRequestValidator() 
        {
            string msg = "Текущий пароль является обязательным к заполнению";
            RuleFor(expression: CPR => CPR.CurrentPassword)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов");

            msg = "Новый пароль является обязательным к заполнению";
            RuleFor(expression: CPR => CPR.NewPassword)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 6).WithMessage(errorMessage: "Минимальная длина пароля - 6 символов")
                .NotEqual(expression: CPR => CPR.CurrentPassword).WithMessage(errorMessage: "Новый пароль должен быть отличен от старого");
        }
    }
}

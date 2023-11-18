using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class MessagesMessageReceiversRequestValidator : AbstractValidator<MessagesController.MessageReceiversRequest>
    {
        public MessagesMessageReceiversRequestValidator() 
        {
            string msg = "Фильтр не может быть пустым";
            RuleFor(MRR => MRR.Filter)
                .NotNull().WithMessage(errorMessage: msg)
                .NotEmpty().WithMessage(errorMessage: msg)
                .Must(predicate: p => !String.IsNullOrWhiteSpace(value: p)).WithMessage(errorMessage: msg)
                .MinimumLength(minimumLength: 3).WithMessage(errorMessage: "Длина фильтра должна быть больше 2");
        }
    }
}

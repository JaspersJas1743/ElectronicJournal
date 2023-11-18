using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class MessagesSendMessageRequestValidator : AbstractValidator<MessagesController.SendMessageRequest>
    {
        public MessagesSendMessageRequestValidator() 
        {
            RuleFor(expression: SMR => SMR.ReceiverId)
                .GreaterThan(valueToCompare: 0).WithMessage(errorMessage: "Идентификатор получателя должен быть больше 0");
        }
    }
}

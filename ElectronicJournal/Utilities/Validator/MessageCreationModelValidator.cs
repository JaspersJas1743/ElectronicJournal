using ElectronicJournal.Models;
using FluentValidation;

namespace ElectronicJournal.Utilities.Validator
{
    public class MessageCreationModelValidator : AbstractValidator<MessageCreationModel>
    {
        public MessageCreationModelValidator() 
        {
            RuleFor(expression: MCM => MCM.SelectedUser)
                .NotNull().WithMessage("Необходимо выбрать получателя");
        }
    }
}

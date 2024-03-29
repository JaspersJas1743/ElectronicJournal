﻿using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class MessagesGetMessagesRequestValidator : AbstractValidator<MessagesController.GetMessagesRequest>
    {
        public MessagesGetMessagesRequestValidator() 
        {
            RuleFor(expression: GMR => GMR.UserId)
                .GreaterThanOrEqualTo(valueToCompare: 0).WithMessage(errorMessage: "Идентификатор должен быть больше 0");

            RuleFor(expression: GMR => GMR.Offset)
                .GreaterThanOrEqualTo(valueToCompare: 0).WithMessage(errorMessage: "Отступ должен быть больше 0");

            RuleFor(expression: GMR => GMR.Count)
                .GreaterThan(valueToCompare: 0).WithMessage(errorMessage: "Количество должен быть больше 0");
        }
    }
}

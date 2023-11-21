using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class MarksGetMarksRequest : AbstractValidator<MarksController.GetMarksRequest>
    {
        public MarksGetMarksRequest() 
        {
            RuleFor(expression: GMR => GMR.LessonId)
                .LessThanOrEqualTo(valueToCompare: 0).WithMessage(errorMessage: "Некорректный идентификатор");
        }
    }
}

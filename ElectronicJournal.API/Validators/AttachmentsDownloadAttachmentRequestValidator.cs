using ElectronicJournal.API.Controllers;
using FluentValidation;

namespace ElectronicJournal.API.Validators
{
    public class AttachmentsDownloadAttachmentRequestValidator : AbstractValidator<AttachmentsController.DownloadAttachmentRequest>
    {
        public AttachmentsDownloadAttachmentRequestValidator() 
        {
            RuleFor(expression: DAR => DAR.Id)
                .GreaterThan(valueToCompare: 0).WithMessage(errorMessage: "Идентификатор не должен быть больше 0");
        }
    }
}

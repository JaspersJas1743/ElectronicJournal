namespace ElectronicJournal.API.DBModels;

public partial class Homework
{
    public int Id { get; set; }

    public int? Lesson { get; set; }

    public int? Group { get; set; }

    public string Text { get; set; } = null!;

    public DateTime CompletionDate { get; set; }

    public int? Attachment { get; set; }

    public virtual Attachment? AttachmentNavigation { get; set; }

    public virtual Group? GroupNavigation { get; set; }

    public virtual Lesson? LessonNavigation { get; set; }
}

namespace ElectronicJournal.API.DBModels;

public partial class Mark
{
	public int Id { get; set; }

	public int Student { get; set; }

	public int Teacher { get; set; }

	public int Lesson { get; set; }

	public string Mark1 { get; set; } = null!;

	public string? Description { get; set; }

	public virtual Lesson LessonNavigation { get; set; } = null!;

	public virtual Student StudentNavigation { get; set; } = null!;

	public virtual Teacher TeacherNavigation { get; set; } = null!;
}

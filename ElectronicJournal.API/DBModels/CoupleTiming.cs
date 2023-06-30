namespace ElectronicJournal.API.DBModels;

public partial class CoupleTiming
{
	public int Id { get; set; }

	public int Lesson { get; set; }

	public TimeSpan Start { get; set; }

	public TimeSpan End { get; set; }

	public string DayOfWeek { get; set; } = null!;

	public int Group { get; set; }

	public string Auditorium { get; set; } = null!;

	public virtual Group GroupNavigation { get; set; } = null!;

	public virtual Lesson LessonNavigation { get; set; } = null!;
}

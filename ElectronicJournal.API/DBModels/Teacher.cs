namespace ElectronicJournal.API.DBModels;

public partial class Teacher
{
	public int Id { get; set; }

	public int User { get; set; }

	public virtual Group? Group { get; set; }

	public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

	public virtual User UserNavigation { get; set; } = null!;

	public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

	public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

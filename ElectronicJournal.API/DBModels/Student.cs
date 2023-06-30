namespace ElectronicJournal.API.DBModels;

public partial class Student
{
	public int Id { get; set; }

	public int User { get; set; }

	public int Group { get; set; }

	public virtual Group GroupNavigation { get; set; } = null!;

	public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

	public virtual ICollection<Parent> Parents { get; set; } = new List<Parent>();

	public virtual User UserNavigation { get; set; } = null!;
}

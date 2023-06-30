namespace ElectronicJournal.API.DBModels;

public partial class Specialisation
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<Group> Groups { get; set; } = new List<Group>();

	public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

namespace ElectronicJournal.API.DBModels;

public partial class Lesson
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<CoupleTiming> CoupleTimings { get; set; } = new List<CoupleTiming>();

	public virtual ICollection<Homework> Homeworks { get; set; } = new List<Homework>();

	public virtual ICollection<Mark> Marks { get; set; } = new List<Mark>();

	public virtual ICollection<Specialisation> Specialisations { get; set; } = new List<Specialisation>();

	public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}

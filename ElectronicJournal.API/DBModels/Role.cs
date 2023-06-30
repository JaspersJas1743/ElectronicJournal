namespace ElectronicJournal.API.DBModels;

public partial class Role
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();
}

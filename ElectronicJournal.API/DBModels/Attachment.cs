namespace ElectronicJournal.API.DBModels;

public partial class Attachment
{
    public int Id { get; set; }

    public string? Path { get; set; }

    public virtual Message? Message { get; set; }

    public virtual Homework Homework { get; set; }
}

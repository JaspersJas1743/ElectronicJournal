using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.Models;

public partial class ElectronicJournalContext : DbContext
{
	public ElectronicJournalContext()
	{
	}

	public ElectronicJournalContext(DbContextOptions<ElectronicJournalContext> options)
		: base(options)
	{
	}

	public virtual DbSet<Admin> Admins { get; set; }

	public virtual DbSet<Attachment> Attachments { get; set; }

	public virtual DbSet<CoupleTiming> CoupleTimings { get; set; }

	public virtual DbSet<Group> Groups { get; set; }

	public virtual DbSet<Homework> Homeworks { get; set; }

	public virtual DbSet<Lesson> Lessons { get; set; }

	public virtual DbSet<Mark> Marks { get; set; }

	public virtual DbSet<Message> Messages { get; set; }

	public virtual DbSet<Parent> Parents { get; set; }

	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<Specialisation> Specialisations { get; set; }

	public virtual DbSet<Student> Students { get; set; }

	public virtual DbSet<Teacher> Teachers { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Admin>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Admins__3214EC27005CBC75");

			entity.HasIndex(e => e.User, "UQ__Admins__BD20C6F176F78631").IsUnique();

			entity.Property(e => e.Id).HasColumnName("ID");

			entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Admins)
				.HasForeignKey(d => d.Role)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Admins__Role__59FA5E80");

			entity.HasOne(d => d.UserNavigation).WithOne(p => p.Admin)
				.HasForeignKey<Admin>(d => d.User)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Admins__User__59063A47");
		});

		modelBuilder.Entity<Attachment>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC276A4ED7C0");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Path).HasMaxLength(100);

			entity.HasMany(d => d.Messages).WithMany(p => p.Attachments)
				.UsingEntity<Dictionary<string, object>>(
					"AttachmentsMessage",
					r => r.HasOne<Message>().WithMany()
						.HasForeignKey("MessagesId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Attachmen__Messa__6D0D32F4"),
					l => l.HasOne<Attachment>().WithMany()
						.HasForeignKey("AttachmentsId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Attachmen__Attac__6C190EBB"),
					j =>
					{
						j.HasKey("AttachmentsId", "MessagesId").HasName("PK__Attachme__F9D9A2E231052F3A");
						j.ToTable("Attachments_Messages");
						j.IndexerProperty<int>("AttachmentsId").HasColumnName("Attachments_ID");
						j.IndexerProperty<int>("MessagesId").HasColumnName("Messages_ID");
					});
		});

		modelBuilder.Entity<CoupleTiming>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__CoupleTi__3214EC278E3AF25F");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Auditorium).HasMaxLength(5);
			entity.Property(e => e.DayOfWeek).HasMaxLength(255);

			entity.HasOne(d => d.GroupNavigation).WithMany(p => p.CoupleTimings)
				.HasForeignKey(d => d.Group)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__CoupleTim__Group__693CA210");

			entity.HasOne(d => d.LessonNavigation).WithMany(p => p.CoupleTimings)
				.HasForeignKey(d => d.Lesson)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__CoupleTim__Lesso__68487DD7");
		});

		modelBuilder.Entity<Group>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Groups__3214EC2787B547FE");

			entity.HasIndex(e => e.ClassTeacher, "UQ__Groups__9CAC3D257BC2C412").IsUnique();

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Name).HasMaxLength(50);

			entity.HasOne(d => d.ClassTeacherNavigation).WithOne(p => p.Group)
				.HasForeignKey<Group>(d => d.ClassTeacher)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Groups__ClassTea__5FB337D6");

			entity.HasOne(d => d.SpecialisationNavigation).WithMany(p => p.Groups)
				.HasForeignKey(d => d.Specialisation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Groups__Speciali__60A75C0F");
		});

		modelBuilder.Entity<Homework>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Homework__3214EC277039E204");

			entity.Property(e => e.Id).HasColumnName("ID");

			entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Homeworks)
				.HasForeignKey(d => d.Group)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Homeworks__Group__656C112C");

			entity.HasOne(d => d.LessonNavigation).WithMany(p => p.Homeworks)
				.HasForeignKey(d => d.Lesson)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Homeworks__Lesso__6477ECF3");

			entity.HasMany(d => d.Attachments).WithMany(p => p.Homeworks)
				.UsingEntity<Dictionary<string, object>>(
					"HomeworksAttachment",
					r => r.HasOne<Attachment>().WithMany()
						.HasForeignKey("AttachmentsId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Homeworks__Attac__787EE5A0"),
					l => l.HasOne<Homework>().WithMany()
						.HasForeignKey("HomeworksId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Homeworks__Homew__778AC167"),
					j =>
					{
						j.HasKey("HomeworksId", "AttachmentsId").HasName("PK__Homework__107051EC05741EF1");
						j.ToTable("Homeworks_Attachments");
						j.IndexerProperty<int>("HomeworksId").HasColumnName("Homeworks_ID");
						j.IndexerProperty<int>("AttachmentsId").HasColumnName("Attachments_ID");
					});
		});

		modelBuilder.Entity<Lesson>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Lessons__3214EC277AC59F42");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Name).HasMaxLength(70);
		});

		modelBuilder.Entity<Mark>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Marks__3214EC277AE3F314");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Description).HasMaxLength(50);
			entity.Property(e => e.Mark1)
				.HasMaxLength(255)
				.HasColumnName("Mark");

			entity.HasOne(d => d.LessonNavigation).WithMany(p => p.Marks)
				.HasForeignKey(d => d.Lesson)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Marks__Lesson__6383C8BA");

			entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Marks)
				.HasForeignKey(d => d.Student)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Marks__Student__619B8048");

			entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.Marks)
				.HasForeignKey(d => d.Teacher)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Marks__Teacher__628FA481");
		});

		modelBuilder.Entity<Message>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC27446B713C");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.ReadDatetime).HasColumnType("datetime");
			entity.Property(e => e.SendDatetime).HasColumnType("datetime");

			entity.HasOne(d => d.ReceiverNavigation).WithMany(p => p.MessageReceiverNavigations)
				.HasForeignKey(d => d.Receiver)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Messages__Receiv__66603565");

			entity.HasOne(d => d.SenderNavigation).WithMany(p => p.MessageSenderNavigations)
				.HasForeignKey(d => d.Sender)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Messages__Sender__6754599E");
		});

		modelBuilder.Entity<Parent>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Parents__3214EC274422AE31");

			entity.HasIndex(e => e.User, "UQ__Parents__BD20C6F1E94ACEA7").IsUnique();

			entity.Property(e => e.Id).HasColumnName("ID");

			entity.HasOne(d => d.ChildNavigation).WithMany(p => p.Parents)
				.HasForeignKey(d => d.Child)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Parents__Child__5EBF139D");

			entity.HasOne(d => d.UserNavigation).WithOne(p => p.Parent)
				.HasForeignKey<Parent>(d => d.User)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Parents__User__5DCAEF64");
		});

		modelBuilder.Entity<Role>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC27FC10B286");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Name).HasMaxLength(50);
		});

		modelBuilder.Entity<Specialisation>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Speciali__3214EC27F14D6030");

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.Name).HasMaxLength(50);

			entity.HasMany(d => d.Lessons).WithMany(p => p.Specialisations)
				.UsingEntity<Dictionary<string, object>>(
					"SpecialisationsLesson",
					r => r.HasOne<Lesson>().WithMany()
						.HasForeignKey("LessonsId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Specialis__Lesso__7C4F7684"),
					l => l.HasOne<Specialisation>().WithMany()
						.HasForeignKey("SpecialisationsId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Specialis__Speci__7B5B524B"),
					j =>
					{
						j.HasKey("SpecialisationsId", "LessonsId").HasName("PK__Speciali__CA6DB7B385AED312");
						j.ToTable("Specialisations_Lessons");
						j.IndexerProperty<int>("SpecialisationsId").HasColumnName("Specialisations_ID");
						j.IndexerProperty<int>("LessonsId").HasColumnName("Lessons_ID");
					});
		});

		modelBuilder.Entity<Student>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Students__3214EC272E07BAE9");

			entity.HasIndex(e => e.User, "UQ__Students__BD20C6F176094BAC").IsUnique();

			entity.Property(e => e.Id).HasColumnName("ID");

			entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Students)
				.HasForeignKey(d => d.Group)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Students__Group__5CD6CB2B");

			entity.HasOne(d => d.UserNavigation).WithOne(p => p.Student)
				.HasForeignKey<Student>(d => d.User)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Students__User__5BE2A6F2");
		});

		modelBuilder.Entity<Teacher>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC2760F04C5A");

			entity.HasIndex(e => e.User, "UQ__Teachers__BD20C6F1D6F993A4").IsUnique();

			entity.Property(e => e.Id).HasColumnName("ID");

			entity.HasOne(d => d.UserNavigation).WithOne(p => p.Teacher)
				.HasForeignKey<Teacher>(d => d.User)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__Teachers__User__5AEE82B9");

			entity.HasMany(d => d.Groups).WithMany(p => p.Teachers)
				.UsingEntity<Dictionary<string, object>>(
					"TeachersGroup",
					r => r.HasOne<Group>().WithMany()
						.HasForeignKey("GroupsId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Teachers___Group__74AE54BC"),
					l => l.HasOne<Teacher>().WithMany()
						.HasForeignKey("TeachersId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Teachers___Teach__73BA3083"),
					j =>
					{
						j.HasKey("TeachersId", "GroupsId").HasName("PK__Teachers__C94C3195A53FA712");
						j.ToTable("Teachers_Groups");
						j.IndexerProperty<int>("TeachersId").HasColumnName("Teachers_ID");
						j.IndexerProperty<int>("GroupsId").HasColumnName("Groups_ID");
					});

			entity.HasMany(d => d.Lessons).WithMany(p => p.Teachers)
				.UsingEntity<Dictionary<string, object>>(
					"TeachersLesson",
					r => r.HasOne<Lesson>().WithMany()
						.HasForeignKey("LessonsId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Teachers___Lesso__70DDC3D8"),
					l => l.HasOne<Teacher>().WithMany()
						.HasForeignKey("TeachersId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__Teachers___Teach__6FE99F9F"),
					j =>
					{
						j.HasKey("TeachersId", "LessonsId").HasName("PK__Teachers__0FF214AED2C27BC8");
						j.ToTable("Teachers_Lessons");
						j.IndexerProperty<int>("TeachersId").HasColumnName("Teachers_ID");
						j.IndexerProperty<int>("LessonsId").HasColumnName("Lessons_ID");
					});
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27C555FB1D");

			entity.HasIndex(e => e.AuthKey, "UK_AuthKey").IsUnique();

			entity.HasIndex(e => e.Password, "UK_Password").IsUnique();

			entity.Property(e => e.Id).HasColumnName("ID");
			entity.Property(e => e.AuthKey).HasMaxLength(255);
			entity.Property(e => e.Birthday).HasColumnType("date");
			entity.Property(e => e.Email).HasMaxLength(50);
			entity.Property(e => e.Name).HasMaxLength(50);
			entity.Property(e => e.Password).HasMaxLength(255);
			entity.Property(e => e.Patronymic).HasMaxLength(50);
			entity.Property(e => e.Phone)
				.HasMaxLength(15)
				.IsUnicode(false);
			entity.Property(e => e.Photo).HasMaxLength(100);
			entity.Property(e => e.RegistrationCode).HasMaxLength(255);
			entity.Property(e => e.Role).HasMaxLength(255);
			entity.Property(e => e.Sex).HasMaxLength(255);
			entity.Property(e => e.Surname).HasMaxLength(50);
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

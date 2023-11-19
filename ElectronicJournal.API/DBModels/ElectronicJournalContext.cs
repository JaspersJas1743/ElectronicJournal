using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ElectronicJournal.API.DBModels;

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

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Homework> Homeworks { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonTopic> LessonTopics { get; set; }

    public virtual DbSet<Mark> Marks { get; set; }

    public virtual DbSet<MarksEnum> MarksEnums { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Parent> Parents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admins__3214EC27532B9E84");

            entity.HasIndex(e => e.User, "UQ__Admins__BD20C6F155B1E28B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Admins)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admins__Role__38B96646");

            entity.HasOne(d => d.UserNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.User)
                .HasConstraintName("FK__Admins__User__39AD8A7F");
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attachme__3214EC27299A51CA");

            entity.HasIndex(e => e.Path, "UQ__Attachme__A15FA6CB89E8722D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Path).HasMaxLength(100);
        });

        modelBuilder.Entity<CoupleTiming>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CoupleTi__3214EC2773BDAEA0");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Auditorium).HasMaxLength(5);
            entity.Property(e => e.Date).HasColumnType("date");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.CoupleTimings)
                .HasForeignKey(d => d.Group)
                .HasConstraintName("FK__CoupleTim__Group__3C89F72A");

            entity.HasOne(d => d.LessonNavigation).WithMany(p => p.CoupleTimings)
                .HasForeignKey(d => d.Lesson)
                .HasConstraintName("FK__CoupleTim__Lesso__2E70E1FD");

            entity.HasOne(d => d.TopicNavigation).WithMany(p => p.CoupleTimings)
                .HasForeignKey(d => d.Topic)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CoupleTim__Topic__3E723F9C");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genders__3214EC2795BE775B");

            entity.HasIndex(e => e.Name, "UQ__Genders__737584F67F316710").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(3)
                .HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Groups__3214EC27FD6E4F1A");

            entity.HasIndex(e => e.Name, "UQ__Groups__737584F64748FB92").IsUnique();

            entity.HasIndex(e => e.ClassTeacher, "UQ__Groups__9CAC3D25DD98815B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(11);

            entity.HasOne(d => d.ClassTeacherNavigation).WithOne(p => p.Group)
                .HasForeignKey<Group>(d => d.ClassTeacher)
                .HasConstraintName("FK__Groups__ClassTea__3F6663D5");

            entity.HasOne(d => d.SpecializationNavigation).WithMany(p => p.Groups)
                .HasForeignKey(d => d.Specialization)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groups__Speciali__405A880E");
        });

        modelBuilder.Entity<Homework>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Homework__3214EC2761D26139");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CompletionDate).HasColumnType("datetime");

            entity.HasOne(d => d.AttachmentNavigation).WithOne(p => p.Homework)
                .HasForeignKey<Homework>(d => d.Attachment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Homeworks__Attach");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Homeworks)
                .HasForeignKey(d => d.Group)
                .HasConstraintName("FK__Homeworks__Group__414EAC47");

            entity.HasOne(d => d.LessonNavigation).WithMany(p => p.Homeworks)
                .HasForeignKey(d => d.Lesson)
                .HasConstraintName("FK__Homeworks__Lesso__2B947552");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lessons__3214EC270EF1F0AB");

            entity.HasIndex(e => e.Name, "UQ__Lessons__737584F6D55BB868").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(70);
        });

        modelBuilder.Entity<LessonTopic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LessonTo__3214EC27DE81D917");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Topic).HasMaxLength(255);

            entity.HasOne(d => d.LessonNavigation).WithMany(p => p.LessonTopics)
                .HasForeignKey(d => d.Lesson)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LessonTop__Lesso__26CFC035");
        });

        modelBuilder.Entity<Mark>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Marks__3214EC27CF1A17DC");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Mark1).HasColumnName("Mark");

            entity.HasOne(d => d.LessonNavigation).WithMany(p => p.Marks)
                .HasForeignKey(d => d.Lesson)
                .HasConstraintName("FK__Marks__Lesson__29AC2CE0");

            entity.HasOne(d => d.Mark1Navigation).WithMany(p => p.Marks)
                .HasForeignKey(d => d.Mark1)
                .HasConstraintName("FK__Marks__Mark__4707859D");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Marks)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__Marks__Student__47FBA9D6");

            entity.HasOne(d => d.TeacherNavigation).WithMany(p => p.Marks)
                .HasForeignKey(d => d.Teacher)
                .HasConstraintName("FK__Marks__Teacher__48EFCE0F");
        });

        modelBuilder.Entity<MarksEnum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MarksEnu__3214EC272C74BE46");

            entity.ToTable("MarksEnum");

            entity.HasIndex(e => e.Value, "UQ__MarksEnu__07D9BBC2D294B526").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Value).HasMaxLength(1);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC2738827A57");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.ReadDatetime).HasColumnType("datetime");
            entity.Property(e => e.SendDatetime).HasColumnType("datetime");

            entity.HasOne(d => d.AttachmentNavigation).WithOne(p => p.Message)
                .HasForeignKey<Message>(d => d.Attachment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Messages__Attach__51300E55");

            entity.HasOne(d => d.SenderNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Sender)
                .HasConstraintName("FK__Messages__Sender__49E3F248");

            entity.HasMany(d => d.Receivers).WithMany(p => p.MessagesNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "MessagesUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Messages___Users__4BCC3ABA"),
                    l => l.HasOne<Message>().WithMany()
                        .HasForeignKey("MessagesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Messages___Messa__4AD81681"),
                    j =>
                    {
                        j.HasKey("MessagesId", "UsersId").HasName("PK__Messages__2EFA06DB2371EF3F");
                        j.ToTable("Messages_Users");
                        j.IndexerProperty<int>("MessagesId").HasColumnName("Messages_ID");
                        j.IndexerProperty<int>("UsersId").HasColumnName("Users_ID");
                    });
        });

        modelBuilder.Entity<Parent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Parents__3214EC274248229F");

            entity.HasIndex(e => e.User, "UQ__Parents__BD20C6F1C1666574").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.ChildNavigation).WithMany(p => p.Parents)
                .HasForeignKey(d => d.Child)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Parents__Child__4CC05EF3");

            entity.HasOne(d => d.UserNavigation).WithOne(p => p.Parent)
                .HasForeignKey<Parent>(d => d.User)
                .HasConstraintName("FK__Parents__User__4DB4832C");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC27BCC9239E");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F6B40B989A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Speciali__3214EC27783A3067");

            entity.HasIndex(e => e.Name, "UQ__Speciali__737584F62E4C0FD6").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(7);

            entity.HasMany(d => d.Lessons).WithMany(p => p.Specializations)
                .UsingEntity<Dictionary<string, object>>(
                    "SpecializationsLesson",
                    r => r.HasOne<Lesson>().WithMany()
                        .HasForeignKey("LessonsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Specializ__Lesso__436BFEE3"),
                    l => l.HasOne<Specialization>().WithMany()
                        .HasForeignKey("SpecializationsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Specializ__Speci__4F9CCB9E"),
                    j =>
                    {
                        j.HasKey("SpecializationsId", "LessonsId").HasName("PK__Speciali__04B712E6BC5F669E");
                        j.ToTable("Specializations_Lessons");
                        j.IndexerProperty<int>("SpecializationsId").HasColumnName("Specializations_ID");
                        j.IndexerProperty<int>("LessonsId").HasColumnName("Lessons_ID");
                    });
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC274160EE4F");

            entity.HasIndex(e => e.User, "UQ__Students__BD20C6F1DE793D3D").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.GroupNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Group)
                .HasConstraintName("FK__Students__Group__5090EFD7");

            entity.HasOne(d => d.UserNavigation).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.User)
                .HasConstraintName("FK__Students__User__51851410");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC2753E0F5AA");

            entity.HasIndex(e => e.User, "UQ__Teachers__BD20C6F16ABA7B71").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.HasOne(d => d.UserNavigation).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.User)
                .HasConstraintName("FK__Teachers__User__52793849");

            entity.HasMany(d => d.Groups).WithMany(p => p.Teachers)
                .UsingEntity<Dictionary<string, object>>(
                    "TeachersGroup",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Teachers___Group__536D5C82"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeachersId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Teachers___Teach__546180BB"),
                    j =>
                    {
                        j.HasKey("TeachersId", "GroupsId").HasName("PK__Teachers__C94C3195A842312D");
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
                        .HasConstraintName("FK__Teachers___Lesso__37FA4C37"),
                    l => l.HasOne<Teacher>().WithMany()
                        .HasForeignKey("TeachersId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Teachers___Teach__5649C92D"),
                    j =>
                    {
                        j.HasKey("TeachersId", "LessonsId").HasName("PK__Teachers__0FF214AE05203EE4");
                        j.ToTable("Teachers_Lessons");
                        j.IndexerProperty<int>("TeachersId").HasColumnName("Teachers_ID");
                        j.IndexerProperty<int>("LessonsId").HasColumnName("Lessons_ID");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27021659CD");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.ConfirmationCode).HasMaxLength(5);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(3);
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Patronymic).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Photo).HasMaxLength(255);
            entity.Property(e => e.RegistrationCode).HasMaxLength(6);
            entity.Property(e => e.Surname).HasMaxLength(50);

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Gender)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__Gender__573DED66");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__UserRole__5832119F");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRole__3214EC27C4C88E57");

            entity.HasIndex(e => e.Name, "UQ__UserRole__737584F6C6C87564").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

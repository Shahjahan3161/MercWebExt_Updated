using MercWebExt.Migrations;
using Microsoft.EntityFrameworkCore;

namespace MercWebExt.Models.DataBase
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public static DatabaseContext CreateInstance(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new DatabaseContext(optionsBuilder.Options);
        }

        public virtual DbSet<Career> Career { get; set; }
        public virtual DbSet<InductionInduction> InductionInduction { get; set; }
        public virtual DbSet<InductionQuestion> InductionQuestion { get; set; }
        public virtual DbSet<Policies> Policies { get; set; }
        public virtual DbSet<UsersRole> UsersRole { get; set; }
        public virtual DbSet<UsersUser> UsersUser { get; set; }
		public DbSet<Categories> Categories { get; set; }
		public DbSet<Categories2> Categories2 { get; set; }
		public DbSet<Categories3> Categories3 { get; set; }
		public DbSet<AboutUs> AboutUs { get; set; }
		public DbSet<Refrigerated> Refrigerated { get; set; }
		public DbSet<Certified> Certifieds { get; set; }
		public DbSet<Settings> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Database context is not configured.");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "MercWebAdmin");

            modelBuilder.Entity<Career>(entity =>
            {
                entity.HasKey(e => e.Cid);
                entity.ToTable("Career", "dbo");
                entity.Property(e => e.Company)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsActivated).HasDefaultValueSql("((1))");
                entity.Property(e => e.IsDisplayed).HasDefaultValueSql("((0))");
                entity.Property(e => e.IsNonExpired).HasDefaultValueSql("((0))");

                entity.Property(e => e.JobDescription)
                    .HasMaxLength(5000)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('<br /><br /><br />')");

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InductionInduction>(entity =>
            {
                entity.HasKey(e => e.Iid);

                entity.ToTable("Induction.Induction", "dbo");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Company)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DriverEmail)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.DriverMobile)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.ForkliftNo)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Fr1)
                    .HasColumnName("fr_1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr2)
                    .HasColumnName("fr_2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr3)
                    .HasColumnName("fr_3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr4)
                    .HasColumnName("fr_4")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr5)
                    .HasColumnName("fr_5")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr6)
                    .HasColumnName("fr_6")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr7)
                    .HasColumnName("fr_7")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fr8)
                    .HasColumnName("fr_8")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Qr1)
                    .HasColumnName("qr_1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr10)
                    .HasColumnName("qr_10")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr11)
                    .HasColumnName("qr_11")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr12)
                    .HasColumnName("qr_12")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr13)
                    .HasColumnName("qr_13")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr14)
                    .HasColumnName("qr_14")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr15)
                    .HasColumnName("qr_15")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr16)
                    .HasColumnName("qr_16")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr17)
                    .HasColumnName("qr_17")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr18)
                    .HasColumnName("qr_18")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr19)
                    .HasColumnName("qr_19")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr2)
                    .HasColumnName("qr_2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr20)
                    .HasColumnName("qr_20")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr3)
                    .HasColumnName("qr_3")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr4)
                    .HasColumnName("qr_4")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr5)
                    .HasColumnName("qr_5")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr6)
                    .HasColumnName("qr_6")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr7)
                    .HasColumnName("qr_7")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr8)
                    .HasColumnName("qr_8")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qr9)
                    .HasColumnName("qr_9")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RegoNumber)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Response1)
                    .HasColumnName("response1")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Response2)
                    .HasColumnName("response2")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<InductionQuestion>(entity =>
            {
                entity.HasKey(e => e.Qid);

                entity.ToTable("Induction.Question", "dbo");

                entity.Property(e => e.AnswerLabel1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('True')");

                entity.Property(e => e.AnswerLabel2)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('False')");

                entity.Property(e => e.AnswerLabel3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.IsActivated)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsNotUseFile).HasDefaultValueSql("((1))");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Policies>(entity =>
            {
                entity.HasKey(e => e.Pid);

                entity.ToTable("Policies", "dbo");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FileExt)
                    .HasColumnName("FIleExt")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FilePath)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.IsUsed)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Owner)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersRole>(entity =>
            {
                entity.HasKey(e => e.Rid);

                entity.ToTable("Users.Role", "dbo");

                entity.Property(e => e.DateUpdated).HasDefaultValueSql("(sysdatetime())");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsersUser>(entity =>
            {
                entity.HasKey(e => e.Uid);

                entity.ToTable("Users.User", "dbo");

                entity.HasIndex(e => e.Uid)
                    .HasName("UK_Uid")
                    .IsUnique();

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateLastLogin).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateUpdated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActived).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsNonExpired).HasDefaultValueSql("((1))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}

using EnglishCentralManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishCentralManagement.Data
{
    public class EnglishCentreDbContext : DbContext
    {
        public EnglishCentreDbContext(DbContextOptions<EnglishCentreDbContext> options)
        : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Staff> Staffs => Set<Staff>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Class> Classes => Set<Class>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<PaymentSchedule> PaymentSchedules => Set<PaymentSchedule>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<EventCalendar> EventCalendars => Set<EventCalendar>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<ClassSession> ClassSessions => Set<ClassSession>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<FooterItem> FooterItems => Set<FooterItem>();
        public DbSet<HeaderBodySection> HeaderBodyItems => Set<HeaderBodySection>();
        public DbSet<SectionItem> SectionItems => Set<SectionItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Soft delete global filter
            modelBuilder.Entity<Staff>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Class>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Enrollment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PaymentSchedule>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Payment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<EventCalendar>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Expense>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Attendance>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ClassSession>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<FooterItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<HeaderBodySection>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PaymentSchedule>()
                    .HasOne(ps => ps.Enrollment)
                    .WithMany(e => e.PaymentSchedules)
                    .HasForeignKey(ps => ps.EnrollmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(x => x.DateOfBirth)
                      .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(x => x.DateOfBirth)
                      .HasColumnType("timestamp without time zone");
            });
            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.StudentId, e.ClassId })
                .IsUnique();
            modelBuilder.Entity<EventCalendar>()
                .HasOne(e => e.Staff)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HeaderBodySection>()
                .HasMany(h => h.SectionItems)
                .WithOne(s => s.HeaderBodySection)
                .HasForeignKey(s => s.HeaderBodySectionId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}

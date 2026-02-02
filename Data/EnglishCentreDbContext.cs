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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Soft delete global filter
            modelBuilder.Entity<Staff>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Student>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Class>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Enrollment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDeleted);
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
            base.OnModelCreating(modelBuilder);
        }
    }
}

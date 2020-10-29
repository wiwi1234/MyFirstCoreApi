using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MyFirstCoreData.Models
{
    public partial class WilliamHighSchoolContext : DbContext
    {
        public WilliamHighSchoolContext()
        {
        }

        public WilliamHighSchoolContext(DbContextOptions<WilliamHighSchoolContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Lesson> Lesson { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentLessonMapping> StudentLessonMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_lesson");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<StudentLessonMapping>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Period).HasComment("第幾節課");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

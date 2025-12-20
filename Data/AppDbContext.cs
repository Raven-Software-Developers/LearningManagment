using Microsoft.EntityFrameworkCore;
using LearningManagement.Models;
using System.Diagnostics; // Подключите модели, которые создадим ниже

namespace LearningManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Путь к БД в AppData для всех платформ
            string projectPath = Path.GetDirectoryName(typeof(AppDbContext).Assembly.Location);
            string dbPath = Path.Combine(projectPath, "learning.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связи: Оценка принадлежит студенту и предмету
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentId);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Subject)
                .WithMany(sub => sub.Grades)
                .HasForeignKey(g => g.SubjectId);
        }
    }
}
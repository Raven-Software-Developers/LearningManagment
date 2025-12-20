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
            string basePath = AppContext.BaseDirectory;

            // Или более надёжный вариант — путь к папке проекта (работает и в Debug, и при запуске)
            // string basePath = Path.GetDirectoryName(typeof(AppDbContext).Assembly.Location)!;

            string dbPath = Path.Combine(basePath, "Data", "learning.db");

            // Создаём папку Data, если её вдруг нет
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

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
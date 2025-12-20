using Microsoft.EntityFrameworkCore;
using LearningManagement.Models;
using System.Diagnostics; // Подключите модели, которые создадим ниже

namespace LearningManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            // Пытаемся положить БД в папку Data проекта
            try
            {
                // Надёжный путь к корню проекта
                string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string binDirectory = Path.GetDirectoryName(assemblyPath)!;
                string projectDirectory = Directory.GetParent(binDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;

                string dataFolder = Path.Combine(projectDirectory, "Data");
                string dbPath = Path.Combine(dataFolder, "learning.db");

                // Создаём папку Data, если её нет
                Directory.CreateDirectory(dataFolder);

                // Проверяем, можем ли писать в эту папку (пробный файл)
                string testFile = Path.Combine(dataFolder, ".write_test");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile); // Если дошло сюда — права есть

                System.Diagnostics.Debug.WriteLine($"БД успешно в проекте: {dbPath}");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
                return;
            }
            catch (Exception ex)
            {
                // Если не удалось писать в папку Data — падаем обратно на безопасный путь
                System.Diagnostics.Debug.WriteLine($"Нет прав на запись в папку Data: {ex.Message}. Используем AppDataDirectory.");
            }
#endif

            // Запасной вариант — стандартный путь MAUI (всегда работает)
            string safePath = Path.Combine(FileSystem.AppDataDirectory, "learning.db");
            System.Diagnostics.Debug.WriteLine($"БД в безопасной папке: {safePath}");
            optionsBuilder.UseSqlite($"Data Source={safePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
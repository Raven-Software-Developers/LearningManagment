using LearningManagement.Data;
using LearningManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningManagment;

public partial class StudentsGradesPage : ContentPage
{
    private AppDbContext _dbContext;

    public StudentsGradesPage()
    {
        InitializeComponent();
        _dbContext = new AppDbContext();
        LoadStudents();
    }

    private void LoadStudents()
    {
        var students = _dbContext.Students
            .Include(s => s.Grades)
            .ToList();

        StudentsList.ItemsSource = students;
    }

    private async void OnStudentTapped(object sender, TappedEventArgs e)
    {
        if ((sender as Grid)?.BindingContext is Student selectedStudent)
        {
            // Загружаем предметы для оценок
            foreach (var grade in selectedStudent.Grades)
            {
                _dbContext.Entry(grade).Reference(g => g.Subject).Load();
            }

            await Navigation.PushAsync(new StudentDetailPage(selectedStudent));
        }
    }
}
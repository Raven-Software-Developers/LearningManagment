using LearningManagement.Data;
using LearningManagement.Models;
using Microsoft.EntityFrameworkCore;


namespace LearningManagment;

public partial class MainPage : ContentPage
{
    private AppDbContext _dbContext;

    public MainPage()
    {
        InitializeComponent();
        _dbContext = new AppDbContext();
        _dbContext.Database.EnsureCreated(); // Создаёт БД, если её нет
        LoadData();
    }

    private void LoadData()
    {
        var students = _dbContext.Students.Include(s => s.Grades).ToList();

        StudentPicker.ItemsSource = students;
        StudentPicker.ItemDisplayBinding = new Binding("Name");

        SubjectPicker.ItemsSource = _dbContext.Subjects.ToList();
        SubjectPicker.ItemDisplayBinding = new Binding("Name");
    }

    private async void AddStudent_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(StudentNameEntry.Text))
        {
            await DisplayAlertAsync("Ошибка", "Введите имя студента", "OK");
            return;
        }

        _dbContext.Students.Add(new Student { Name = StudentNameEntry.Text.Trim() });
        await _dbContext.SaveChangesAsync();

        StudentNameEntry.Text = string.Empty;
        LoadData();
    }

    private async void AddSubject_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SubjectNameEntry.Text))
        {
            await DisplayAlertAsync("Ошибка", "Введите название предмета", "OK");
            return;
        }

        _dbContext.Subjects.Add(new Subject { Name = SubjectNameEntry.Text.Trim() });
        await _dbContext.SaveChangesAsync();

        SubjectNameEntry.Text = string.Empty;
        LoadData();
    }

    private async void AddGrade_Clicked(object sender, EventArgs e)
    {
        if (StudentPicker.SelectedItem is not Student selectedStudent)
        {
            await DisplayAlertAsync("Ошибка", "Выберите студента", "OK");
            return;
        }

        if (SubjectPicker.SelectedItem is not Subject selectedSubject)
        {
            await DisplayAlertAsync("Ошибка", "Выберите предмет", "OK");
            return;
        }

        if (!double.TryParse(GradeValueEntry.Text, out double value) || value < 1 || value > 5)
        {
            await DisplayAlertAsync("Ошибка", "Введите корректную оценку (от 1 до 5)", "OK");
            return;
        }

        _dbContext.Grades.Add(new Grade
        {
            Value = value,
            StudentId = selectedStudent.Id,
            SubjectId = selectedSubject.Id
        });

        await _dbContext.SaveChangesAsync();

        GradeValueEntry.Text = string.Empty;
        LoadData();
    }

    private void RefreshList_Clicked(object sender, EventArgs e)
    {
        LoadData();
    }

}
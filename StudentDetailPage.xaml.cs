using LearningManagement.Models;

namespace LearningManagment;

public partial class StudentDetailPage : ContentPage
{
    public StudentDetailPage(Student student)
    {
        InitializeComponent();
        BindingContext = student;  // Передаём выбранного студента
    }
}
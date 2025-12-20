using System.Collections.Generic;
using System.Linq;

namespace LearningManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Grade> Grades { get; set; } = new List<Grade>();

        // Автоматический расчёт среднего балла (on the fly)
        public double AverageGrade => Grades.Any() ? Grades.Average(g => g.Value) : 0;
    }
}
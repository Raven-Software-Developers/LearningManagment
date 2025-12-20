using System.Collections.Generic;

namespace LearningManagement.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
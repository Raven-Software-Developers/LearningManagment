namespace LearningManagement.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public double Value { get; set; } // Оценка, например, от 1 до 5
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
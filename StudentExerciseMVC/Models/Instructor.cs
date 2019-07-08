namespace StudentExerciseMVC.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Slack { get; set; }
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
        public void AssignExercises(Student student, Exercise exercise)
        {
            student.StudentExercises.Add(exercise);
            // exercise.StudentExercises.Add(student);
        }
    }
}
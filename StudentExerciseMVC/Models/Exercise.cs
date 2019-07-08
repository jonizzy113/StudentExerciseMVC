using System.Collections.Generic;

namespace StudentExerciseMVC.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseLanguage { get; set; }

        public List<Student> StudentExercises { get; set; }
    }
}
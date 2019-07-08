using System.Collections.Generic;

namespace StudentExerciseMVC.Models
{
    public class Cohort
    {
        public int Id { get; set; }
        public string CohortName { get; set; }
        public List<Student> StudentList { get; set; }
        public List<Instructor> InstructorList { get; set; }
    }
}
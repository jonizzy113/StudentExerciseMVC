﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVC.Models
{
    public class Student
    {
        public int Id { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Slack { get; set; }

        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
        public List<Exercise> StudentExercises { get; set; }
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }



    }


}

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExerciseMVC.Models.ViewModels
{
    public class StudentEditViewModel
    {
        private string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }
        public List<int> SelectedValues { get; set; } = new List<int>();
        public Student Student { get; set; }
        //public List<Cohort> AvaliableCohorts { get; set; }
        //public List<Exercise> AvaliableExercise { get; set; }
        public List<SelectListItem> Cohorts { get; set; }
        public List<SelectListItem> Exercises { get; set; }
        public StudentEditViewModel(string connectionString)
        {
            _connectionString = connectionString;

            Cohorts = GetAllCohorts()
                .Select(c => new SelectListItem
                {
                    Text = c.CohortName,
                    Value = c.Id.ToString()
                }).ToList();

            Cohorts
                .Insert(0, new SelectListItem
                {
                    Text = "Choose cohort...",
                    Value = "0"
                });

            Exercises = GetAllExercises()
                .Select(c => new SelectListItem
                {
                    Text = c.ExerciseName,
                    Value = c.Id.ToString()
                }).ToList();

            Exercises
                .Insert(0, new SelectListItem
                {
                   Text = "Choose exercises...",
                    Value = "0"
                });

        }
        //public List<MultiSelectList> AvailableExerciseSelectList
        //{
        //    get
        //    {
        //        if (AvaliableExercise == null)
        //        {
        //            return null;
        //        }
        //        return AvaliableExercise
        //               .Select(e => new MultiSelectList(e.ExerciseName, e.Id.ToString()))
        //               .ToList();
        //    }
        //}
        //public List<SelectListItem> AvailableCohortsSelectList
        //{
        //    get
        //    {
        //        if (AvaliableCohorts == null)
        //        {
        //            return null;
        //        }
        //        return AvaliableCohorts
        //               .Select(c => new SelectListItem(c.CohortName, c.Id.ToString()))
        //               .ToList();
        //    }
        //}
        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, CohortName FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }
        private List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, ExerciseName, ExerciseLanguage FROM Exercise";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        exercises.Add(new Exercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                            ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                        });
                    }

                    reader.Close();

                    return exercises;
                }
            }
        }


    }
}

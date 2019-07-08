using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExerciseMVC.Models;
using StudentExerciseMVC.Models.ViewModels;

namespace StudentExerciseMVC.Controllers
{

    public class StudentsController : Controller
    {
        private readonly IConfiguration _config;

        public StudentsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Students
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT s.Id,
                                            s.FirstName,
                                            s.LastName,
                                            s.Slack,
                                            s.CohortId
                                        FROM Student s
                                    ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Slack = reader.GetString(reader.GetOrdinal("Slack")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return View(students);
                }
            }
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            Student student = GetStudentById(id);
            return View(student);
            
        }

            // GET: Students/Create
        public ActionResult Create()
        {
            StudentCreateViewModel studentCreateViewModel = new StudentCreateViewModel(_config.GetConnectionString("DefaultConnection"));

            return View(studentCreateViewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StudentCreateViewModel model)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Student (FirstName, LastName, Slack, CohortId)
                                                     VALUES (@FirstName, @LastName, @Slack, @CohortId)";

                        cmd.Parameters.Add(new SqlParameter("@FirstName", model.student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", model.student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@Slack", model.student.Slack));
                        cmd.Parameters.Add(new SqlParameter("@CohortId", model.student.CohortId));


                        await cmd.ExecuteNonQueryAsync();

                        return RedirectToAction(nameof(Index));

                    }
                }   
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            List<Cohort> cohorts = GetAllCohorts();

            Student student = GetStudentById(id);

            StudentEditViewModel StudentEditViewModel = new StudentEditViewModel(_config.GetConnectionString("DefaultConnection"));

            StudentEditViewModel.Student = GetStudentById(id);


            return View(StudentEditViewModel);

        }

        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.CohortName FROM Cohort c";
                    List<Cohort> cohorts = new List<Cohort>();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                         Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };
                    cohorts.Add(cohort);
                    }
                    return cohorts;
                }
            }
                throw new NotImplementedException();
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, StudentEditViewModel viewModel)
        {
            Student student = viewModel.Student;
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Student
                                            SET FirstName = @FirstName,
                                            LastName = @LastName,
                                            Slack = @Slack,                
                                            CohortId = @CohortId
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@FirstName", student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@Slack", student.Slack));
                        cmd.Parameters.Add(new SqlParameter("@id", id));
                        cmd.Parameters.Add(new SqlParameter("@CohortId", student.CohortId));

                        cmd.ExecuteNonQuery();
                        
                        
                       return RedirectToAction(nameof(Index));
                        
                        
                    }   
                }
            }

            catch
            {
                return View();
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            Student student = GetStudentById(id);
            return View(student);
         
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudent(int id)
        {
            try
            {
                    using (SqlConnection conn = Connection)
                    {
                        conn.Open();
                        using (SqlCommand cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = @"DELETE FROM StudentExercise WHERE StudentId= @id;
                                                DELETE FROM Student WHERE Id = @id";
                            cmd.Parameters.Add(new SqlParameter("@id", id));

                            cmd.ExecuteNonQuery();

                            return RedirectToAction(nameof(Index));
                        }
                    }

            }
            catch
            {
                return View();
            }
        }
        private Student GetStudentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, LastName, Slack, CohortId
                        FROM Student
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;

                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Slack = reader.GetString(reader.GetOrdinal("Slack")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
                        };
                    }
                    reader.Close();

                    return student;
                }
            }
        }

    }
}
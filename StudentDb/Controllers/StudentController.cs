using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using StudentDb.Models;

namespace StudentDb.Controllers
{
    public class StudentController : Controller
    {
        private string connectionString =
            "server=localhost;database=db_sql;user=root;password=;";
        public IActionResult Index()
        {
            List<Student> students = new List<Student>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                EnsureStudentsTableExists(con);

                const string query = "SELECT Id, Name, Age FROM students";

                using MySqlCommand command = new MySqlCommand(query, con);

                using MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = new Student();

                    student.Id = Convert.ToInt32(reader["Id"]);
                    student.Name = reader["Name"].ToString();
                    student.Age = Convert.ToInt32(reader["Age"]);
                    // student.Address = reader["Address"].ToString(); 
                    students.Add(student);
                }
            }

            return View(students);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                using (MySqlConnection con =
                    new MySqlConnection(connectionString))
                {
                    EnsureStudentsTableExists(con);

                    const string query =
                        "INSERT INTO students (Name, Age) VALUES (@Name, @Age)";

                    using MySqlCommand command = new MySqlCommand(query, con);

                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@Age", student.Age);
                    // command.Parameters.AddWithValue("@Address", student.Address);

                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using MySqlConnection con = new MySqlConnection(connectionString);
            EnsureStudentsTableExists(con);

            const string query = "SELECT Id, Name, Age FROM students WHERE Id = @Id";

            using MySqlCommand command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue("@Id", id);

            using MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound();
            }

            Student student = new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Age = Convert.ToInt32(reader["Age"])
            };

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            using MySqlConnection con = new MySqlConnection(connectionString);
            EnsureStudentsTableExists(con);

            const string query = """
                UPDATE students
                SET Name = @Name, Age = @Age
                WHERE Id = @Id
                """;

            using MySqlCommand command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Age", student.Age);
            command.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            using MySqlConnection con = new MySqlConnection(connectionString);
            EnsureStudentsTableExists(con);

            const string query = "SELECT Id, Name, Age FROM students WHERE Id = @Id";

            using MySqlCommand command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue("@Id", id);

            using MySqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return NotFound();
            }

            Student student = new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Age = Convert.ToInt32(reader["Age"])
            };

            return View(student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using MySqlConnection con = new MySqlConnection(connectionString);
            EnsureStudentsTableExists(con);

            const string query = "DELETE FROM students WHERE Id = @Id";

            using MySqlCommand command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        private static void EnsureStudentsTableExists(MySqlConnection connection)
        {
            const string query = """
                CREATE TABLE IF NOT EXISTS students (
                    Id INT NOT NULL AUTO_INCREMENT,
                    Name VARCHAR(100) NOT NULL,
                    Age INT NOT NULL,
                    PRIMARY KEY (Id)
                )
                """;

            connection.Open();

            using MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }

}

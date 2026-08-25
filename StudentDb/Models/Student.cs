using System.ComponentModel.DataAnnotations;

namespace StudentDb.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(5, 100, ErrorMessage = "Age must be between 5 and 100")]
        public int Age { get; set; }
    }
}

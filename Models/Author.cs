using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        public ICollection<Book>? Books { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"\S+.*", ErrorMessage = "Name cannot be just whitespace")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [RegularExpression(@"\S+.*", ErrorMessage = "Description cannot be just whitespace")]
        public string Description { get; set; } = string.Empty;

        public ICollection<Book>? Books { get; set; } = new List<Book>();
    }
}

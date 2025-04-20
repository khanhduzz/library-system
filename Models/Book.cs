using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select an author.")]
        [Display(Name = "Author")]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}

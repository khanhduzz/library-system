using System.ComponentModel.DataAnnotations;
using LibrarySystem.Enums;

namespace LibrarySystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select an author.")]
        [Display(Name = "Author")]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
        public byte[]? Image { get; set; }
        [Required]
        public string ISBN { get; set; } = string.Empty;

        public DateTime? PublishDate { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; } = new List<BookGenre>();

        public bool IsAvailable { get; set; } = true;

        public DateTime? BorrowedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public int? BorrowerId { get; set; }

        public int BorrowCount { get; set; } = 0;

        public BookCondition Condition { get; set; } = BookCondition.New;
        public int TotalCopies { get; set; } = 0;
        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();

    }
}

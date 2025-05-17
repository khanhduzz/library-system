using LibrarySystem.Enums;

namespace LibrarySystem.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public BookCondition Condition { get; set; } = BookCondition.New;

        public ICollection<BorrowedBook> BorrowedBooks { get; set; } = new List<BorrowedBook>();
    }
}

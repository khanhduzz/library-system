namespace LibrarySystem.Models
{
    public class BookDetailsViewModel
    {
        public Book Book { get; set; }
        public bool IsBorrowedByUser { get; set; }
        public bool IsAvailableForRent {  get; set; }
    }
}

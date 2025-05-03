namespace LibrarySystem.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public int AvailableCopies { get; set; }
    }
}

using LibrarySystem.Models;

namespace LibrarySystem.Generic
{
    public class HomeViewModel
    {
        public List<Book> Books { get; set; } = new List<Book>();
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}

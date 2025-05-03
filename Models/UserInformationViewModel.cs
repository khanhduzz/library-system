namespace LibrarySystem.Models
{
    public class UserInformationViewModel
    {
        public User User { get; set; }
        public List<BorrowedBook> BorrowedBooks { get; set; }
    }
}

using LibrarySystem.Enums;

namespace LibrarySystem.Models
{
    public class Role
    {
        public int Id { get; set; }
        public UserRole UserRole { get; set; }
    }
}

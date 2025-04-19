using System.ComponentModel.DataAnnotations;
using LibrarySystem.Enums;

namespace LibrarySystem.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public UserRole UserRole { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}

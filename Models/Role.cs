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
        public static IEnumerable<Role> GetRoles()
        {
            // Assuming you're getting roles from a static source or a database
            return Enum.GetValues(typeof(UserRole))
                       .Cast<UserRole>()
                       .Select(role => new Role { UserRole = role });
        }
    }
}

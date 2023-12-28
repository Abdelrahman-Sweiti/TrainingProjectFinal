using Microsoft.AspNetCore.Identity;

namespace PersonalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string? Image { get; set; }
    }
}

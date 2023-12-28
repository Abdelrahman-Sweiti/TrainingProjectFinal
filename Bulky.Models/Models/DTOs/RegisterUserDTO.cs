using System.ComponentModel.DataAnnotations;

namespace PersonalProject.Models.DTOs
{
    public class RegisterUserDTO
    {

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }

        public string? Image { get; set; }
        public string? Roles { get; set; }

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "password and Confirm Password does not match")]
        public string ConfirmPassword { get; set; }

    }
}

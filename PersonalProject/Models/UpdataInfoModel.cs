using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalProject.Models
{
    public class UpdataInfoModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        public string? Gender { get; set; }

        public string? Image { get; set; }



    }
}

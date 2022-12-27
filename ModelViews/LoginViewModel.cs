using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppAspNetCore.ModelViews
{
    public class LoginViewModel
    {
        [Key]
        [Display(Name = "Email")]
        [MaxLength(100)]
        [Required(ErrorMessage = "Email is required!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required!")]
        [MinLength(5, ErrorMessage = "Password must be greater than or equal to 5 characters!")]
        public string Password { get; set; }
    }
}
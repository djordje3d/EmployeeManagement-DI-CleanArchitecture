using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords doooooo not match")] // Compare - proverava da li su lozinke iste
        public string ConfirmPassword { get; set; }
    }
}

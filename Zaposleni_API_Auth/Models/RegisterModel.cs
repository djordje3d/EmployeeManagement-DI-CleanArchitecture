using System.ComponentModel.DataAnnotations;

namespace Zaposleni_API_Auth.Models
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

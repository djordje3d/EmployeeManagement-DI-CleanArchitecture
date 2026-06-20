using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        public string Password { get; set; }
    }
}

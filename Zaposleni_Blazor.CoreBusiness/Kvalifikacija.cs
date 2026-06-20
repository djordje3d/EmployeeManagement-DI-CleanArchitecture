using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Blazor.CoreBusiness
{
    public class Kvalifikacija
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(4)] // Ograničava nvarchar na 4 karaktera
        public string? LicniStepenKv { get; set; }

        [Required]
        [MaxLength(30)] // Ograničava nvarchar na 30 karaktera
        public string? Naziv { get; set; }

        public ICollection<Zaposlen> Zaposleni { get; set; } = new List<Zaposlen>();

    }
}
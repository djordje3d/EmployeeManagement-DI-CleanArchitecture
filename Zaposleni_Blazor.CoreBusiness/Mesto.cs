using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Blazor.CoreBusiness
{
    public class Mesto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] // Ograničava nvarchar na 50 karaktera
        public string? Naziv { get; set; }

        [MaxLength(50)] // Ograničava nvarchar na 50 karaktera
        public string? Opstina { get; set; }

        [MaxLength(10)] // Ograničava nvarchar na 10 karaktera
        public string? PostanskiBroj { get; set; }

        public ICollection<Zaposlen> Zaposleni { get; set; } = new List<Zaposlen>();
    }
}
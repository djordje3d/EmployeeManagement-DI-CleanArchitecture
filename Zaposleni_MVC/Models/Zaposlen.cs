using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Clean_MVC_API.Models
{
    public class Zaposlen
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ime je obavezan unos.")]
        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? Ime { get; set; }

        [Required(ErrorMessage = "Prezime je obavezan unos.")]
        [MaxLength(20)] // Ograničava nvarchar na 20 karaktera
        public string? Prezime { get; set; }

        [Required(ErrorMessage = "Roditelj je obavezan unos.")]
        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? Roditelj { get; set; }

        [Required(ErrorMessage = "Datum rođenja je obavezan unos.")]
        public DateTime DatumRodjenja { get; set; }

        [MaxLength(36)] // Ograničava nvarchar na 36 karaktera
        public string? Adresa { get; set; }

        //public string Mesto { get; set; }

        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? Telefon { get; set; }

        [Required(ErrorMessage = "JMBG je obavezan.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG mora sadržavati tačno 13 cifara.")]
        [MaxLength(13)] // Ograničava nvarchar na 13 karaktera
        public string? JMBG { get; set; }

        public DateTime Pocetak_RadnogOd { get; set; } = DateTime.Now;

        public DateTime? Kraj_RadnogOd { get; set; }

        public string? Vrsta_RadnogOdnosa { get; set; }

        [MaxLength(1)] // Ograničava nvarchar na 1 karaktera
        public string? A_P { get; set; }

        public int? KvalifikacijaId { get; set; }

        [Required(ErrorMessage = "Mesto je obavezan unos.")]
        public int? MestoId { get; set; }
        public int? SistematizacijeId { get; set; }

        public Kvalifikacija? Kvalifikacija { get; set; }

        public Mesto? Mesto { get; set; }

        public Sistematizacija? Sistematizacije { get; set; }

    }
}

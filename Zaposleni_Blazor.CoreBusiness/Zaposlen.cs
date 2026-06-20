using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Blazor.CoreBusiness
{
    public class Zaposlen
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? Ime { get; set; }

        [Required]
        [MaxLength(20)] // Ograničava nvarchar na 20 karaktera
        public string? Prezime { get; set; }

        [Required]
        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? Roditelj { get; set; }

        [Required]
        public DateTime DatumRodjenja { get; set; } = new DateTime(1987, 1, 1);

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

        
        public int? MestoId { get; set; }

        public int? SistematizacijeId { get; set; }

        public Kvalifikacija? Kvalifikacija { get; set; }

        public Mesto? Mesto { get; set; }

        public Sistematizacija? Sistematizacije { get; set; }

    }
}

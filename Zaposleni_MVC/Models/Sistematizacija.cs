using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Clean_MVC_API.Models
{
    public class Sistematizacija
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)] // Ograničava nvarchar na 16 karaktera
        public string? NazivRadnogMesta { get; set; }

        [Column(TypeName = "decimal(6,3)")]
        public decimal? Koeficijent { get; set; }

        [MaxLength(10)] // Ograničava nvarchar na 10 karaktera
        public string? Radno_Iskustvo { get; set; }

        public int? Beneficirani_Radni_Staz { get; set; }

        public int? Bodovi { get; set; } // Služi za obračun plata

        [MaxLength(3000)] // Ograničava nvarchar na 3000 karaktera
        public string? Opis { get; set; }

        [Required(ErrorMessage = "Organizaciona jedinica je obavezna.")]
        public int? OrganizacioneJediniceId { get; set; }
        public int? KvalifikacijaId { get; set; }
        public int? KlasifikacijaZanimanjaId { get; set; }



        public OrganizacioneJedinice? OrganizacionaJedinica { get; set; }
        public Kvalifikacija? Kvalifikacija { get; set; }
    }
}

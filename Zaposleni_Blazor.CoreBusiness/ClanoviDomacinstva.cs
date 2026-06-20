using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Blazor.CoreBusiness
{
    public class ClanoviDomacinstva
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? ImeClana { get; set; }

        [MaxLength(1)] // Ograničava na polje char 1 karakter
        [Column(TypeName = "NCHAR")]
        public string? PolClana { get; set; }

        [MaxLength(10)] // Ograničava nvarchar na 10 karaktera
        public string? SroClana { get; set; }

        public DateTime? DatumRodjenjaClana { get; set; }

        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? StatusClana { get; set; }

        [Required(ErrorMessage = "JMBG je obavezan.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "JMBG mora sadržavati tačno 13 cifara.")]
        [MaxLength(13)] // Ograničava nvarchar na 13 karaktera
        public string? JMBG { get; set; }

        [MaxLength(16)] // Ograničava nvarchar na 16 karaktera
        public string? Roditelj { get; set; }

        public Zaposlen? Zaposlen { get; set; }

        public int? ZaposlenId { get; set; }

    }
}

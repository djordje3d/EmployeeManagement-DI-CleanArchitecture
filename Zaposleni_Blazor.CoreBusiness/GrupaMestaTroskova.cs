using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Blazor.CoreBusiness
{
    public class GrupaMestaTroskova
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(2)] // Ograničava nvarchar na 2 karaktera
        public string? Grupa { get; set; }

        [Required]
        [MaxLength(40)] // Ograničava nvarchar na 40 karaktera
        public string? Naziv { get; set; }

        public ICollection<OrganizacioneJedinice>? OrganizacioneJedinice { get; set; } = new List<OrganizacioneJedinice>();

    }
}

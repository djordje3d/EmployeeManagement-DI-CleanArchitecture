using System.ComponentModel.DataAnnotations;

namespace Zaposleni_Clean_MVC_API.Models
{
    public class OrganizacioneJedinice
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(6)] // Ograničava nvarchar na 6 karaktera
        public string? OJ { get; set; }

        [Required]
        [MaxLength(40)] // Ograničava nvarchar na 40 karaktera
        public string? Naziv { get; set; }

        [Required]
        public int? GrMestaTroskovaId { get; set; }

        public GrupaMestaTroskova? GrMestaTroskova { get; set; }
    }
}

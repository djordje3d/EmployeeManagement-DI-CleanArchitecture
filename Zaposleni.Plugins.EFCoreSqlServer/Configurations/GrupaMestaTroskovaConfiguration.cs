using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    public class GrupaMestaTroskovaConfiguration : IEntityTypeConfiguration<GrupaMestaTroskova>
    {
        public void Configure(EntityTypeBuilder<GrupaMestaTroskova> builder)
        {
            //builder.HasKey(g => g.Id);
            //builder.Property(g => g.Naziv).IsRequired().HasMaxLength(40);
            //builder.Property(g => g.Grupa).HasMaxLength(2);

            // Inicijalni podaci
            builder.HasData(
                new GrupaMestaTroskova { Id = 1, Grupa = "01", Naziv = "FIRMA" },
                new GrupaMestaTroskova { Id = 2, Grupa = "02", Naziv = "GENERALNI DIREKTOR" },
                new GrupaMestaTroskova { Id = 3, Grupa = "03", Naziv = "PROIZVODNJA" }
            );
        }
    }
}

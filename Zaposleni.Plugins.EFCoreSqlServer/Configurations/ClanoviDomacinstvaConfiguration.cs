using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    public class ClanoviDomacinstvaConfiguration : IEntityTypeConfiguration<ClanoviDomacinstva>
    {
        public void Configure(EntityTypeBuilder<ClanoviDomacinstva> builder)
        {
            //builder.HasKey(c => c.Id);
            //builder.Property(c => c.ImeClana).IsRequired().HasMaxLength(16);
            //builder.Property(c => c.PolClana).IsRequired().HasMaxLength(20);
            //builder.Property(c => c.JMBG).IsRequired().HasMaxLength(13);
            //builder.Property(c => c.SroClana).HasMaxLength(36);
            //builder.Property(c => c.DatumRodjenjaClana).HasMaxLength(16);
            //builder.Property(c => c.StatusClana).HasMaxLength(16);
            //builder.Property(c => c.Roditelj).HasMaxLength(16);

            // Inicijalni podaci
            builder.HasData(
                new ClanoviDomacinstva { Id = 1, ImeClana = "Mirjana", PolClana = "Ž", SroClana = "Ćerka", DatumRodjenjaClana = new DateTime(1998, 02, 12), StatusClana = "", JMBG = "1202998123456", Roditelj = "Jelena", ZaposlenId = 1 },
                new ClanoviDomacinstva { Id = 2, ImeClana = "Tihomir", PolClana = "M", SroClana = "Sin", DatumRodjenjaClana = new DateTime(1997, 09, 23), StatusClana = "", JMBG = "2309997654987", Roditelj = "Jelena", ZaposlenId = 1 },
                new ClanoviDomacinstva { Id = 3, ImeClana = "Goran", PolClana = "M", SroClana = "Sin", DatumRodjenjaClana = new DateTime(2018, 06, 19), StatusClana = "", JMBG = "0906018557789", Roditelj = "Ivana", ZaposlenId = 2 }
            );
        }
    }
}

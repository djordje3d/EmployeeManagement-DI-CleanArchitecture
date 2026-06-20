using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    public class KvalifikacijaConfiguration : IEntityTypeConfiguration<Kvalifikacija>
    {
        public void Configure(EntityTypeBuilder<Kvalifikacija> builder)
        {
            //builder.HasKey(k => k.Id);
            //builder.Property(k => k.Naziv).IsRequired().HasMaxLength(30);
            //builder.Property(k => k.LicniStepenKv).HasMaxLength(4);

            // Kvalifikacija → Zaposleni (one-to-many)
            builder.HasMany(k => k.Zaposleni)
                   .WithOne(z => z.Kvalifikacija)
                   .HasForeignKey(z => z.KvalifikacijaId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Inicijalni podaci
            builder.HasData(
                  new Kvalifikacija { Id = 1, LicniStepenKv = "I", Naziv = "Visoka stručna sprema" },
                  new Kvalifikacija { Id = 2, LicniStepenKv = "II", Naziv = "Srednja stručna sprema" }
            );
        }
    }
}

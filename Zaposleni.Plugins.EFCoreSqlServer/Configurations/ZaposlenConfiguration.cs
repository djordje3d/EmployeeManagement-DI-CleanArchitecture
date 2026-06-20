using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    public class ZaposlenConfiguration : IEntityTypeConfiguration<Zaposlen>
    {
        public void Configure(EntityTypeBuilder<Zaposlen> builder)
        {
            //builder.HasKey(z => z.Id);
            //builder.Property(z => z.Ime).IsRequired().HasMaxLength(16);
            //builder.Property(z => z.Prezime).IsRequired().HasMaxLength(20);
            //builder.Property(z => z.JMBG).IsRequired().HasMaxLength(13);
            //builder.Property(z => z.Adresa).HasMaxLength(36);
            //builder.Property(z => z.Telefon).HasMaxLength(16);

            // Zaposlen → Kvalifikacija (many-to-one)
            builder.HasOne(z => z.Kvalifikacija)
                   .WithMany(k => k.Zaposleni)
                   .HasForeignKey(z => z.KvalifikacijaId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Zaposlen → Mesto (many-to-one)
            builder.HasOne(z => z.Mesto)
                   .WithMany(m => m.Zaposleni) // Ovde je ključ
                   .HasForeignKey(z => z.MestoId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Inicijalni podaci
            builder.HasData(
                new Zaposlen { Id = 1, Ime = "Marko", Prezime = "Negovanovic", Roditelj = "Uros", DatumRodjenja = new DateTime(1987, 11, 23), JMBG = "2311487134221", MestoId = 1, SistematizacijeId = 1 },
                new Zaposlen { Id = 2, Ime = "Ivan", Prezime = "Lazarevic", Roditelj = "Dragan", DatumRodjenja = new DateTime(1960, 10, 02), JMBG = "0210960849567", MestoId = 1, SistematizacijeId = 1 }
            );
        }
    }
}

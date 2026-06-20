using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    
    public class SistematizacijaConfiguration : IEntityTypeConfiguration<Sistematizacija> // IEntityTypeConfiguration je interfejs koji se koristi za konfiguraciju entiteta u EF Core umesto
                                                                                          // unosa unutar OnModelCreating metode u ApplicationDbContext klasi, radi bolje organizacije, modularnosti i clean arhitekture koda.
    {
        public void Configure(EntityTypeBuilder<Sistematizacija> builder)
        {
            // Sistematizacija → OrganizacioneJedinice (many-to-one)
            //modelBuilder.Entity<Sistematizacija>()
            //    .OnDelete(DeleteBehavior.Restrict) // ili .Cascade ako želiš kaskadno brisanje
            //    .HasOne(s => s.OrganizacionaJedinica)
            //    .WithMany() // Ako budeš imao `public ICollection<Sistematizacija> Sistematizacije` u OJ klasi, možeš ovde staviti `.WithMany(oj => oj.Sistematizacije)`
            //    .HasForeignKey(s => s.OrganizacioneJediniceId);

            builder.HasOne(s => s.OrganizacionaJedinica)
                   .WithMany()
                   .HasForeignKey(s => s.OrganizacioneJediniceId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Kvalifikacija)
                   .WithMany()
                   .HasForeignKey(s => s.KvalifikacijaId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new Sistematizacija { Id = 1, NazivRadnogMesta = "Programer", Koeficijent = 2.5m, Radno_Iskustvo = "5 godina", Beneficirani_Radni_Staz = 0, Bodovi = 100, Opis = "Opis radnog mesta za programera.", OrganizacioneJediniceId = 28, KvalifikacijaId = 1 },
                new Sistematizacija { Id = 2, NazivRadnogMesta = "Tehničar", Koeficijent = 1.85m, Radno_Iskustvo = "3 godine", Beneficirani_Radni_Staz = 0, Bodovi = 80, Opis = "Opis za tehničara.", OrganizacioneJediniceId = 29, KvalifikacijaId = 1 }
            );
        }
    }

}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    public class OrganizacioneJediniceConfiguration : IEntityTypeConfiguration<OrganizacioneJedinice>
    {
        public void Configure(EntityTypeBuilder<OrganizacioneJedinice> builder)
        {
            builder.HasKey(oj => oj.Id);

            builder.Property(oj => oj.Naziv).IsRequired().HasMaxLength(40);
            builder.Property(oj => oj.OJ).IsRequired().HasMaxLength(6);

            // Eksplicitno mapiranje naziva kolone u bazi (jer se u API bazi koristi drugačiji naziv)
            builder.Property(oj => oj.GrupaMestaTroskovaId)
                   .HasColumnName("GrMestaTroskovaId");  // Pravilna veza sa bazom!

            builder.HasOne(oj => oj.GrupaMestaTroskova)
       .WithMany(g => g.OrganizacioneJedinice) // Eksplicitna veza !!
       .HasForeignKey(oj => oj.GrupaMestaTroskovaId)
       .OnDelete(DeleteBehavior.Restrict);

            // Inicijalni podaci
            builder.HasData(
                new OrganizacioneJedinice { Id = 28, OJ = "0.5.00", Naziv = "IT sektor", GrupaMestaTroskovaId = 1 },
                new OrganizacioneJedinice { Id = 29, OJ = "1.0.03", Naziv = "ProizvodnjaBG", GrupaMestaTroskovaId = 3 }
            );
        }
    }
}

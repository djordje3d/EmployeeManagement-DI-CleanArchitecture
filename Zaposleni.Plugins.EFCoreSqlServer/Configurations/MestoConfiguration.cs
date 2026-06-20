using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni.Plugins.EFCoreSqlServer.Configurations
{
    public class MestoConfiguration : IEntityTypeConfiguration<Mesto>
    {
        public void Configure(EntityTypeBuilder<Mesto> builder)
        {
            builder.HasData(
            new Mesto { Id = 1, Naziv = "Beograd", Opstina = "Vračar", PostanskiBroj = "11000" },
            new Mesto { Id = 2, Naziv = "Novi Sad", Opstina = "Stari Grad", PostanskiBroj = "21000" }
            );
        }
    }
}

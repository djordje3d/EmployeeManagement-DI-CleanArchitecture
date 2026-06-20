using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    // 📌 Ova klasa je potrebna za migracije i druge dizajn-time operacije sa DbContext-om
    // IDesignTimeDbContextFactory<T> interfejs omogućava kreiranje DbContext-a van vremena izvođenja aplikacije
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>  // Omogućava kreiranje DbContext-a u vreme dizajna (design time)
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // 📌 Eksplicitno navodimo put do Blazor Server projekta
            var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName ?? "", "Zaposleni_Blazor");

            var configuration = new ConfigurationBuilder()  // Konfigurišemo čitač konfiguracionih fajlova
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Koristi produkcioni fajl
                .AddJsonFile("appsettings.Development.json", optional: true) // Učitaj razvojni fajl ako postoji
                .Build();


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(optionsBuilder.Options, configuration);
        }
    }
}

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> // Ova klasa se koristi za kreiranje instance ApplicationDbContext-a tokom dizajniranja (npr. migracije)
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {

            // Proverite da li je appsettings.json ispravno postavljen i da li sadrži konekcioni string
            Console.WriteLine("Trenutni direktorijum: " + Directory.GetCurrentDirectory()); 

            if (!File.Exists("appsettings.json"))
            {
                Console.WriteLine("Fajl appsettings.json ne postoji!");
            }
            else
            {
                Console.WriteLine("Fajl appsettings.json je pronađen.");
            }
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Console.WriteLine($"Trenutno okruženje: {environment}");

            var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .Build(); // Kreiramo konfiguraciju koja učitava appsettings.json i appsettings.Development.json (ili drugo okruženje)

            string jsonContent = File.ReadAllText("appsettings.json");
            Console.WriteLine($"Sadržaj appsettings.json:\n{jsonContent}"); // Prikazuje sadržaj appsettings.json

            // Dohvatanje i prikazivanje svih ključeva i vrednosti u sekciji ConnectionStrings
            foreach (var item in configuration.GetSection("ConnectionStrings").GetChildren())
            {
                Console.WriteLine($"Key: {item.Key}, Value: {item.Value}"); // Prikazuje sve ključeve i vrednosti
            }

            var connectionString = configuration.GetConnectionString("ZaposleniConnection");
            Console.WriteLine($"Konekcioni string iz config-a: {connectionString}");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options); // Kreiramo novi ApplicationDbContext sa prosleđenim opcijama
        }
    }
}

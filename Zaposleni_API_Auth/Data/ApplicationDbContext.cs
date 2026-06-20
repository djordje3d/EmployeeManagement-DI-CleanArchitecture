using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zaposleni_API_Auth.Models;
//using Zaposleni_API_Auth_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;


namespace Zaposleni_API_Auth.Data
{
    //    public class ApplicationDbContext : IdentityDbContext // Nasleđujemo IdentityDbContext za ASP.NET Identity funkcionalnosti
    // i.e. korisničke naloge, autentifikaciju itd. 
    // Omogućava ASP.NET Identity-u da automatski dodaje svoje TABELE poput AspNetUsers, AspNetRoles, itd.
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> // Nasleđujemo IdentityDbContext za ASP.NET Identity funkcionalnosti

    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)  // Ovde se prosleđuju opcije za DbContext
        {
        }

        public DbSet<Zaposlen> Zaposleni { get; set; }
        public DbSet<Kvalifikacija> Kvalifikacije { get; set; }
        public DbSet<Mesto> Mesta { get; set; }
        public DbSet<ClanoviDomacinstva> ClanoviDomacinstva { get; set; }
        public DbSet<GrupaMestaTroskova> GrupeMestaTroskova { get; set; }
        public DbSet<OrganizacioneJedinice> OrganizacioneJedinice { get; set; }
        public DbSet<Sistematizacija> Sistematizacije { get; set; }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracija relacija i inicijalni podaci (HasData)
            modelBuilder.Entity<Zaposlen>()
                .HasOne(z => z.Mesto)
                .WithMany(m => m.Zaposleni)
                .HasForeignKey(z => z.MestoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany()
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany()
                .HasForeignKey(up => up.PermissionId);


            base.OnModelCreating(modelBuilder); // Važno: Ovaj poziv mora ostati kako bi Identity mogao da konfiguriše svoje tabele

            modelBuilder.Entity<Permission>().HasData(
    new Permission { PermissionId = 1, Name = "CanAccessKvalifikacije" },
    new Permission { PermissionId = 2, Name = "CanAccessOrganizacioneJedinice" },
    new Permission { PermissionId = 3, Name = "CanAccessKvalifikacije" },
    new Permission { PermissionId = 4, Name = "CanAddKvalifikacije" }
);

            // Inicijalni podaci (HasData)
            modelBuilder.Entity<Kvalifikacija>().HasData(
                new Kvalifikacija { Id = 1, LicniStepenKv = "I", Naziv = "Visoka stručna sprema" },
                new Kvalifikacija { Id = 2, LicniStepenKv = "II", Naziv = "Srednja stručna sprema" }
            );

            modelBuilder.Entity<OrganizacioneJedinice>().HasData(
                new OrganizacioneJedinice { Id = 1, OJ = "0.5.00", Naziv = "IT sektor", GrupaMestaTroskovaId = 1 }
            );

            modelBuilder.Entity<Sistematizacija>().HasData(
                new Sistematizacija
                {
                    Id = 1,
                    NazivRadnogMesta = "Programer",
                    Koeficijent = 2.500m,
                    Radno_Iskustvo = "5 godina",
                    Beneficirani_Radni_Staz = 0,
                    Bodovi = 100,
                    Opis = "Opis radnog mesta za programera.",
                    OrganizacioneJediniceId = 1,
                    KvalifikacijaId = 1
                },
                new Sistematizacija
                {
                    Id = 2,
                    NazivRadnogMesta = "Tehničar",
                    Koeficijent = 1.850m,
                    Radno_Iskustvo = "3 godine",
                    Beneficirani_Radni_Staz = 0,
                    Bodovi = 80,
                    Opis = "Opis za tehničara.",
                    OrganizacioneJediniceId = 1,
                    KvalifikacijaId = 1
                }
            );

            modelBuilder.Entity<Mesto>().HasData(
                new Mesto { Id = 1, Naziv = "Beograd", Opstina = "Vračar", PostanskiBroj = "11000" },
                new Mesto { Id = 2, Naziv = "Novi Sad", Opstina = "Stari Grad", PostanskiBroj = "21000" }
            );

            modelBuilder.Entity<Zaposlen>().HasData(
                new Zaposlen { Id = 1, Ime = "Marko", Prezime = "Negovanovic", Roditelj = "Uros", DatumRodjenja = new DateTime(1987, 11, 23), JMBG = "2311487134221", MestoId = 1, SistematizacijeId = 1 },
                new Zaposlen { Id = 2, Ime = "Ivan", Prezime = "Lazarevic", Roditelj = "Dragan", DatumRodjenja = new DateTime(1960, 10, 02), JMBG = "0210960849567", MestoId = 2, SistematizacijeId = 1 }
            );

            modelBuilder.Entity<GrupaMestaTroskova>().HasData(
                new GrupaMestaTroskova { Id = 1, Grupa = "01", Naziv = "FIRMA" },
                new GrupaMestaTroskova { Id = 2, Grupa = "02", Naziv = "GENERALNI DIREKTOR" },
                new GrupaMestaTroskova { Id = 3, Grupa = "03", Naziv = "PROIZVODNJA" }
            );
        }
    }

}

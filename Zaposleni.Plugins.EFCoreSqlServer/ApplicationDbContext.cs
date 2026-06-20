using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.CoreBusiness.APICore;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>

    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            //var dbVariant = _configuration.GetValue<string>("DatabaseVariant");


            //modelBuilder.Entity<OrganizacioneJedinice>(entity =>
            //{
            //    if (dbVariant == "ZaposleniAPI")
            //    {
            //        entity.Property(e => e.GrMestaTroskovaId).HasColumnName("GrMestaTroskovaId");
            //    }
            //    else
            //    {
            //        entity.Property(e => e.GrMestaTroskovaId).HasColumnName("GrupaMestaTroskovaId");
            //    }
            //});

            base.OnModelCreating(modelBuilder);

            // Konfiguracija za UserPermission
            modelBuilder.Entity<UserPermission>()
                .HasKey(up => new { up.UserId, up.PermissionId });

            modelBuilder.Entity<UserPermission>()  // 
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId);

            // Konfiguracija za RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany() // ako ne mapiraš navigaciju ka RolePermissions
                .HasForeignKey(rp => rp.RoleId);
        }
    }
}

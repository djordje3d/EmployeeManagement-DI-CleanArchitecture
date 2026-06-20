using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class SistematizacijaEFCoreRepository : ISistematizacijaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SistematizacijaEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddSistematizacijaAsync(Sistematizacija sistematizacija)
        {
            try
            {
                dbContext.Sistematizacije.Add(sistematizacija);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteSistematizacijaAsync(int id)
        {
            var existing = await dbContext.Sistematizacije.FindAsync(id);

            if (existing is null)
                return false;

            dbContext.Sistematizacije.Remove(existing);

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Kvalifikacija>> GetAllKvalifikacijeAsync()
        {
            return await dbContext.Kvalifikacije.ToListAsync();
        }

        public async Task<List<OrganizacioneJedinice>> GetAllOrganizacioneJediniceAsync()
        {
            return await dbContext.OrganizacioneJedinice.ToListAsync();
        }

        public Task<Sistematizacija?> GetSistematizacijaByIdAsync(int id)
        {
            if (id <= 0) return Task.FromResult<Sistematizacija?>(null);

            return dbContext.Sistematizacije
                            .Include(k => k.Kvalifikacija)
                            .Include(o => o.OrganizacionaJedinica)
                            .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Sistematizacija>> GetSistematizacijaListAsync()
        {
            return await dbContext.Sistematizacije
                                  .Include(k => k.Kvalifikacija).Include(o => o.OrganizacionaJedinica)
                                  .ToListAsync();
        }

        public async Task<bool> UpdateSistematizacijaAsync(Sistematizacija sistematizacija)
        {
            var existing = await dbContext.Sistematizacije.FindAsync(sistematizacija.Id);

            if (existing is null)
                return false;

            existing.NazivRadnogMesta = sistematizacija.NazivRadnogMesta;
            existing.Koeficijent = sistematizacija.Koeficijent;
            existing.Radno_Iskustvo = sistematizacija.Radno_Iskustvo;
            existing.Beneficirani_Radni_Staz = sistematizacija.Beneficirani_Radni_Staz;
            existing.Bodovi = sistematizacija.Bodovi;
            existing.Opis = sistematizacija.Opis;

            existing.KvalifikacijaId = sistematizacija.KvalifikacijaId;
            existing.OrganizacioneJediniceId = sistematizacija.OrganizacioneJediniceId;

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

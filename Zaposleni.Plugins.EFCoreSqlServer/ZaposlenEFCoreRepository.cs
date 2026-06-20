using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class ZaposlenEFCoreRepository : IZaposlenRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ZaposlenEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddZaposlenAsync(Zaposlen zaposlen)
        {
            try
            {
                dbContext.Zaposleni.Add(zaposlen);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteZaposlenAsync(int id)
        {
            var existing = dbContext.Zaposleni.Find(id);
            if (existing is null)
                return false;

            dbContext.Zaposleni.Remove(existing);

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

        public async Task<IEnumerable<Zaposlen>> GetZaposlenListAsync()
        {
            return await dbContext.Zaposleni
                .Include(z => z.Kvalifikacija)
                .Include(z => z.Mesto)
                .Include(z => z.Sistematizacije)
                .ToListAsync();
        }

        // vrati zaposlenog po ID sa svim povezanim entitetima
        // ukljucujuci Kvalifikacija, Mesto i Sistematizacije
        // Ako se ne uradi iclude, vraca se null za te entitete
        public Task<Zaposlen?> GetZaposlenByIdAsync(int id)
        {
            if (id <= 0) return null;

            return dbContext.Zaposleni  // vrati zaposlenog sa svim povezanim entitetima
                .Include(z => z.Kvalifikacija)
                .Include(z => z.Mesto)
                .Include(z => z.Sistematizacije)
                .FirstOrDefaultAsync(z => z.Id == id);
        }

        public async Task<bool> UpdateZaposlenAsync(Zaposlen zaposlen)
        {
            var existing = dbContext.Zaposleni.Find(zaposlen.Id);
            if (existing is null)
                return false;

            existing.Ime = zaposlen.Ime;
            existing.Prezime = zaposlen.Prezime;
            existing.Roditelj = zaposlen.Roditelj;
            existing.DatumRodjenja = zaposlen.DatumRodjenja;
            existing.Adresa = zaposlen.Adresa;
            existing.Telefon = zaposlen.Telefon;
            existing.JMBG = zaposlen.JMBG;
            existing.Pocetak_RadnogOd = zaposlen.Pocetak_RadnogOd;
            existing.Kraj_RadnogOd = zaposlen.Kraj_RadnogOd;
            existing.Vrsta_RadnogOdnosa = zaposlen.Vrsta_RadnogOdnosa;
            existing.A_P = zaposlen.A_P;
            existing.KvalifikacijaId = zaposlen.KvalifikacijaId;
            existing.MestoId = zaposlen.MestoId;
            existing.SistematizacijeId = zaposlen.SistematizacijeId;

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

        public async Task<IEnumerable<Kvalifikacija>> GetAllKvalifikacijeAsync()
        {
            return await dbContext.Kvalifikacije.ToListAsync();
        }

        public async Task<IEnumerable<Mesto>> GetAllMesta()
        {
            return await dbContext.Mesta.ToListAsync();
        }

        public async Task<IEnumerable<Sistematizacija>> GetAllSistematizacija()
        {
            return await dbContext.Sistematizacije.ToListAsync();
        }
    }

}

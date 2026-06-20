using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class ClanoviDomacinstvaEFCoreRepository : IClanDomacinstvaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ClanoviDomacinstvaEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddClanDomacinstvaAsync(ClanoviDomacinstva clan)
        {
            dbContext.ClanoviDomacinstva.Add(clan);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteClanDomacinstvaAsync(int id)
        {
            var existing = await dbContext.ClanoviDomacinstva.FindAsync(id);
            if (existing is null)
                return false;

            dbContext.ClanoviDomacinstva.Remove(existing);

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

        public async Task<ClanoviDomacinstva?> GetClanDomacinstvaByIdAsync(int id)
        {
            return await dbContext.ClanoviDomacinstva
                .Include(c => c.Zaposlen)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<ClanoviDomacinstva>> GetClanoviDomacinstvaListAsync()
        {
            return await dbContext.ClanoviDomacinstva
                .Include(c => c.Zaposlen)
                .ToListAsync();
        }

        public async Task<bool> UpdateClanDomacinstvaAsync(ClanoviDomacinstva clan)
        {
            var existing = await dbContext.ClanoviDomacinstva.FindAsync(clan.Id);
            if (existing is null)
                return false;

            existing.ImeClana = clan.ImeClana;
            existing.PolClana = clan.PolClana;
            existing.JMBG = clan.JMBG;
            existing.SroClana = clan.SroClana;
            existing.StatusClana = clan.StatusClana;
            existing.Roditelj = clan.Roditelj;
            existing.ZaposlenId = clan.ZaposlenId;

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

        public async Task<IEnumerable<Zaposlen>> GetZaposleniListAsync()
        {
            return await dbContext.Zaposleni.ToListAsync();
        }
    }
}

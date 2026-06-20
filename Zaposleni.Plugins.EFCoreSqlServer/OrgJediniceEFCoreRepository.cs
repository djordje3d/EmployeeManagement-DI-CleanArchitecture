using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class OrgJediniceEFCoreRepository : IOrganizacionaJedinicaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OrgJediniceEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<OrganizacioneJedinice>> GetOrganizacioneJediniceListAsync()
        {
            return await dbContext.OrganizacioneJedinice
                                  .Include(o => o.GrupaMestaTroskova)
                                  .ToListAsync();
        }

        public async Task<OrganizacioneJedinice?> GetOrganizacionaJedinicaByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await dbContext.OrganizacioneJedinice
                                  .Include(o => o.GrupaMestaTroskova)
                                  .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> AddOrganizacionaJedinicaAsync(OrganizacioneJedinice oj)
        {
            try
            {
                dbContext.OrganizacioneJedinice.Add(oj);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateOrganizacionaJedinicaAsync(OrganizacioneJedinice oj)
        {
            var existing = await dbContext.OrganizacioneJedinice.FindAsync(oj.Id);
            if (existing is null)
                return false;

            existing.OJ = oj.OJ;
            existing.Naziv = oj.Naziv;
            existing.GrupaMestaTroskovaId = oj.GrupaMestaTroskovaId;

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

        public async Task<bool> DeleteOrganizacionaJedinicaAsync(int id)
        {
            var existing = await dbContext.OrganizacioneJedinice.FindAsync(id);
            if (existing is null)
                return false;

            dbContext.OrganizacioneJedinice.Remove(existing);

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

        public async Task<IEnumerable<GrupaMestaTroskova>> GetGrupaMestaTroskovaListAsync()
        {
            return await dbContext.GrupeMestaTroskova.ToListAsync();
        }
    }
}

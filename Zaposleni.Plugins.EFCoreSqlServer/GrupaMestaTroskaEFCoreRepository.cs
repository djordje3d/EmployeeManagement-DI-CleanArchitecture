using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class GrupaMestaTroskaEFCoreRepository : IGrupaMestaTroskaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public GrupaMestaTroskaEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<GrupaMestaTroskova>> GetGrupaMestaTroskaListAsync()
        {
            return await dbContext.GrupeMestaTroskova
                                  .OrderBy(g => g.Grupa)
                                  .ToListAsync();
        }

        public async Task<GrupaMestaTroskova?> GetGrupaMestaTroskaByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await dbContext.GrupeMestaTroskova.FindAsync(id);
        }

        public async Task<bool> AddGrupaMestaTroskaAsync(GrupaMestaTroskova grupaMestaTroskova)
        {
            try
            {
                dbContext.GrupeMestaTroskova.Add(grupaMestaTroskova);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateGrupaMestaTroskaAsync(GrupaMestaTroskova grupaMestaTroskova)
        {
            var entity = await dbContext.GrupeMestaTroskova.FindAsync(grupaMestaTroskova.Id);
            if (entity == null) return false;

            entity.Grupa = grupaMestaTroskova.Grupa;
            entity.Naziv = grupaMestaTroskova.Naziv;

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

        public async Task<bool> DeleteGrupaMestaTroskaAsync(int id)
        {
            var entity = await dbContext.GrupeMestaTroskova.FindAsync(id);
            if (entity == null) return false;

            dbContext.GrupeMestaTroskova.Remove(entity);

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

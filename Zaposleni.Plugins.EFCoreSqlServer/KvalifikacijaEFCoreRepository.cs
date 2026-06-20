using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni.Plugins.EFCoreSqlServer
{
    public class KvalifikacijaEFCoreRepository : IKvalifikacijaRepository
    {
        private readonly ApplicationDbContext dbContext;

        public KvalifikacijaEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Kvalifikacija>> GetKvalifikacijaListAsync()
        {
            return await dbContext.Kvalifikacije.ToListAsync();
        }
        public async Task<bool> AddKvalifikacijaAsync(Kvalifikacija kvalifikacija)
        {
            try
            {
                dbContext.Kvalifikacije.Add(kvalifikacija);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Kvalifikacija?> GetKvalifikacijaByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await dbContext.Kvalifikacije.FindAsync(id);
        }

        public async Task<bool> UpdateKvalifikacijaAsync(Kvalifikacija kvalifikacija)
        {
            var kvalifikacijaToUpdate = await dbContext.Kvalifikacije.FindAsync(kvalifikacija.Id);

            if (kvalifikacijaToUpdate is null)
                return false;

            kvalifikacijaToUpdate.LicniStepenKv = kvalifikacija.LicniStepenKv;
            kvalifikacijaToUpdate.Naziv = kvalifikacija.Naziv;

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom ažuriranja: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteKvalifikacijaAsync(int id)
        {
            var kvalifikacijaToDelete = dbContext.Kvalifikacije.Find(id);

            if (kvalifikacijaToDelete is null)
                return false;

            dbContext.Kvalifikacije.Remove(kvalifikacijaToDelete);

            try
            {
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom brisanja: {ex.Message}");
                return false;
            }
        }
    }
}
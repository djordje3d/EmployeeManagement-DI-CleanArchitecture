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
    public class MestoEFCoreRepository : IMestoRepository
    {
        private readonly ApplicationDbContext dbContext;

        public MestoEFCoreRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Mesto>> GetMestoListAsync()
        {
            return await dbContext.Mesta.ToListAsync();
        }

        //public Task AddMestoAsync(Mesto mesto)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> AddMestoAsync(Mesto mesto)
        {
            try
            {
                dbContext.Mesta.Add(mesto);
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Mesto?> GetMestoByIdAsync(int id)
        {
            if (id <= 0) return null;

            return await dbContext.Mesta.FindAsync(id);
        }

        //public Task UpdateMestoAsync(Mesto mesto)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<bool> UpdateMestoAsync(Mesto mesto)
        {
            var mestoToUpdate = await dbContext.Mesta.FindAsync(mesto.Id);

            if (mestoToUpdate is null)
                return false;

            mestoToUpdate.Naziv = mesto.Naziv;
            mestoToUpdate.Opstina = mesto.Opstina;
            mestoToUpdate.PostanskiBroj = mesto.PostanskiBroj;

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
        public async Task<bool> DeleteMestoAsync(int id)
        {
            var mestoToDelete = await dbContext.Mesta.FindAsync(id);

            if (mestoToDelete == null)
                return false;

            dbContext.Mesta.Remove(mestoToDelete);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.OrganizacionaJedinica
{
    public class CrudOrganizacionaJedinicaUseCases : ICrudOrganizacionaJedinicaUseCases
    {
        private readonly IOrganizacionaJedinicaRepository organizacionaJedinicaRepository;

        public CrudOrganizacionaJedinicaUseCases(IOrganizacionaJedinicaRepository organizacionaJedinicaRepository)
        {
            this.organizacionaJedinicaRepository = organizacionaJedinicaRepository;
        }
        public async Task<IEnumerable<OrganizacioneJedinice>> GetAllAsync()
        {
            return await organizacionaJedinicaRepository.GetOrganizacioneJediniceListAsync();
        }

        public async Task<OrganizacioneJedinice?> GetByIdAsync(int id)
        {
            return await organizacionaJedinicaRepository.GetOrganizacionaJedinicaByIdAsync(id);
        }

        public async Task<bool> AddAsync(OrganizacioneJedinice organizacionaJedinica)
        {
            return await organizacionaJedinicaRepository.AddOrganizacionaJedinicaAsync(organizacionaJedinica);
        }

        public async Task<bool> UpdateAsync(OrganizacioneJedinice organizacionaJedinica)
        {
            return await organizacionaJedinicaRepository.UpdateOrganizacionaJedinicaAsync(organizacionaJedinica);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await organizacionaJedinicaRepository.DeleteOrganizacionaJedinicaAsync(id);
        }

        // Nova metoda za dobavljanje svih grupa mesta troškova
        public async Task<IEnumerable<GrupaMestaTroskova>> GetAllGrupaMestaTroskovaAsync()
        {
            return await organizacionaJedinicaRepository.GetGrupaMestaTroskovaListAsync();
        }
    }
}

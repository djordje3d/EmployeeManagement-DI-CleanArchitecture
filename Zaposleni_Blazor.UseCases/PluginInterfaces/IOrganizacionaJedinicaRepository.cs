using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface IOrganizacionaJedinicaRepository
    {
        Task<List<OrganizacioneJedinice>> GetOrganizacioneJediniceListAsync();
        Task<OrganizacioneJedinice?> GetOrganizacionaJedinicaByIdAsync(int id);
        Task<bool> AddOrganizacionaJedinicaAsync(OrganizacioneJedinice oj);
        Task<bool> UpdateOrganizacionaJedinicaAsync(OrganizacioneJedinice oj);
        Task<bool> DeleteOrganizacionaJedinicaAsync(int id);

        // Metoda za dobavljanje svih grupa mesta troškova
        Task<IEnumerable<GrupaMestaTroskova>> GetGrupaMestaTroskovaListAsync();
    }

}

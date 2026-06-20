using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface IClanDomacinstvaRepository
    {
        Task<List<ClanoviDomacinstva>> GetClanoviDomacinstvaListAsync();
        Task<ClanoviDomacinstva?> GetClanDomacinstvaByIdAsync(int id);
        Task<bool> AddClanDomacinstvaAsync(ClanoviDomacinstva clan);
        Task<bool> UpdateClanDomacinstvaAsync(ClanoviDomacinstva clan);
        Task<bool> DeleteClanDomacinstvaAsync(int id);

        // Metoda za dobavljanje svih zaposlenih
        Task<IEnumerable<Zaposlen>> GetZaposleniListAsync();
    }
}

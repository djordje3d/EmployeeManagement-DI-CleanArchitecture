using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface IZaposlenRepository
    {
        Task<IEnumerable<Zaposlen>> GetZaposlenListAsync();
        Task<Zaposlen?> GetZaposlenByIdAsync(int id);
        Task<bool> AddZaposlenAsync(Zaposlen zaposlen);
        Task<bool> UpdateZaposlenAsync(Zaposlen zaposlen);
        Task<bool> DeleteZaposlenAsync(int id);

        // Metoda za dobavljanje svih kvalifikacija
        Task<IEnumerable<Kvalifikacija>> GetAllKvalifikacijeAsync();

        // Metoda za dobavljanje svih mesta
        Task<IEnumerable<Mesto>> GetAllMesta();

        // Metoda za dobavljanje svih sistematizacija
        Task<IEnumerable<Sistematizacija>> GetAllSistematizacija();
    }
}

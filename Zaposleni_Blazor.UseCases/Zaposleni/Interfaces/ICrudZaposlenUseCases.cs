using System.Collections;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Zaposleni.Interfaces
{
    public interface ICrudZaposlenUseCases
    {
        Task<IEnumerable<Zaposlen>> GetAllAsync();
        Task<Zaposlen?> GetByIdAsync(int id);
        Task<bool> AddAsync(Zaposlen zaposlen);
        Task<bool> UpdateAsync(Zaposlen zaposlen);
        Task<bool> DeleteAsync(int id);

        // Metoda za dobavljanje svih kvalifikacija
        Task<IEnumerable<Kvalifikacija>> GetAllKvalifikacijeAsync();

        // Metoda za dobavljanje svih mesta
        Task<IEnumerable<Mesto>> GetAllMestaAsync();

        // Metoda za dobavljanje svih sistematizacija
        Task<IEnumerable<Sistematizacija>> GetAllSistematizacija();
    }
}

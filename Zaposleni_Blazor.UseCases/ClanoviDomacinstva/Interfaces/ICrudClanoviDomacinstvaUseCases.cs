using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.ClanDomacinstva.Interfaces
{
    public interface ICrudClanoviDomacinstvaUseCases
    {
        Task<IEnumerable<ClanoviDomacinstva>> GetAllAsync();
        Task<ClanoviDomacinstva?> GetByIdAsync(int id);
        Task<bool> AddAsync(ClanoviDomacinstva clan);
        Task<bool> UpdateAsync(ClanoviDomacinstva clan);
        Task<bool> DeleteAsync(int id);

        // Metoda za dobavljanje svih zaposlenih
        Task<IEnumerable<Zaposlen>> GetAllZaposleniAsync();
    }
}

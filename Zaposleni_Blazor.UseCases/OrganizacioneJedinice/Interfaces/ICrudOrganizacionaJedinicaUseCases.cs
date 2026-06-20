using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.OrganizacionaJedinica.Interfaces
{
    public interface ICrudOrganizacionaJedinicaUseCases
    {
        Task<IEnumerable<OrganizacioneJedinice>> GetAllAsync();
        Task<OrganizacioneJedinice?> GetByIdAsync(int id);
        Task<bool> AddAsync(OrganizacioneJedinice oj);
        Task<bool> UpdateAsync(OrganizacioneJedinice oj);
        Task<bool> DeleteAsync(int id);

        // Metoda za dobavljanje svih grupa mesta troškova
        Task<IEnumerable<GrupaMestaTroskova>> GetAllGrupaMestaTroskovaAsync();
    }

}

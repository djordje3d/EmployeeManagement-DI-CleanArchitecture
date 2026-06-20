using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Clean_MVC_API.UseCases.Interfaces
{
    public interface ICrudOrganizacionaJedinicaUseCases
    {
        Task<IEnumerable<OrganizacioneJedinice>> GetAllAsync();
        Task<OrganizacioneJedinice?> GetByIdAsync(int id);
        Task<OrganizacioneJedinice> AddAsync(OrganizacioneJedinice oj);
        Task<bool> UpdateAsync(OrganizacioneJedinice oj);
        Task DeleteAsync(int id);

        // Metoda za dobavljanje svih grupa mesta troškova
        Task<IEnumerable<GrupaMestaTroskova>> GetAllGrupaMestaTroskovaAsync();
    }
}

using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Sistematizacije.Interfaces
{
    public interface ICrudSistematizacijaUseCases
    {
        Task<IEnumerable<Sistematizacija>> GetAllAsync();
        Task<Sistematizacija?> GetByIdAsync(int id);
        Task<bool> AddAsync(Sistematizacija sistematizacija);
        Task<bool> UpdateAsync(Sistematizacija sistematizacija);
        Task<bool> DeleteAsync(int id);

        // Metoda za dobavljanje svih organizacionih jedinica
        Task<IEnumerable<OrganizacioneJedinice>> GetAllOrganizacioneJediniceAsync();
        
        // Metoda za dobavljanje svih kvalifikacija
        Task<IEnumerable<Kvalifikacija>> GetAllKvalifikacijeAsync();
    }
}

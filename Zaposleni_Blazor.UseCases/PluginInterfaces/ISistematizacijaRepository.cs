using Zaposleni_Blazor.CoreBusiness;


namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface ISistematizacijaRepository
    {
        Task<bool> AddSistematizacijaAsync(Sistematizacija sistematizacija);
        Task<Sistematizacija?> GetSistematizacijaByIdAsync(int id);
        Task<bool> UpdateSistematizacijaAsync(Sistematizacija sistematizacija);
        Task<List<Sistematizacija>> GetSistematizacijaListAsync();
        Task<bool> DeleteSistematizacijaAsync(int id);

        // Metoda za dobavljanje svih organizacionih jedinica
        Task<List<OrganizacioneJedinice>> GetAllOrganizacioneJediniceAsync();

        // Metoda za dobavljanje svih kvalifikacija
        Task<List<Kvalifikacija>> GetAllKvalifikacijeAsync();

    }
}

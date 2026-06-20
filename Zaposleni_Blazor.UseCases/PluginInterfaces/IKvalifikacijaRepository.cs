using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface IKvalifikacijaRepository
    {
        Task <bool>AddKvalifikacijaAsync(Kvalifikacija kvalifikacija);
        Task<Kvalifikacija?> GetKvalifikacijaByIdAsync(int id);
        Task <bool>UpdateKvalifikacijaAsync(Kvalifikacija kvalifikacija);
        Task<IEnumerable<Kvalifikacija>> GetKvalifikacijaListAsync();
        Task<bool> DeleteKvalifikacijaAsync(int id);
    }
}

using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface IMestoRepository
    {
        Task <bool>AddMestoAsync(Mesto mesto);
        Task<Mesto?> GetMestoByIdAsync(int id);
        Task <bool>UpdateMestoAsync(Mesto mesto);
        Task<IEnumerable<Mesto>> GetMestoListAsync();

        Task<bool> DeleteMestoAsync(int id);
    }
}

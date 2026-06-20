using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.PluginInterfaces
{
    public interface IGrupaMestaTroskaRepository
    {
        Task <bool> AddGrupaMestaTroskaAsync(GrupaMestaTroskova grupaMestaTroskova);
        Task<GrupaMestaTroskova?> GetGrupaMestaTroskaByIdAsync(int id);
        Task <bool> UpdateGrupaMestaTroskaAsync(GrupaMestaTroskova grupaMestaTroskova);
        Task<IEnumerable<GrupaMestaTroskova>> GetGrupaMestaTroskaListAsync();
        Task<bool> DeleteGrupaMestaTroskaAsync(int id);
    }
}
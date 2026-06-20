using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova
{
    public class ListGrupaMestaTroskaUseCase : IListGrupaMestaTroskaUseCase
    {
        private readonly IGrupaMestaTroskaRepository grupaMestaTroskaRepository;

        public ListGrupaMestaTroskaUseCase(IGrupaMestaTroskaRepository grupaMestaTroskaRepository)
        {
            this.grupaMestaTroskaRepository = grupaMestaTroskaRepository;
        }
        public async Task<IEnumerable<GrupaMestaTroskova>> ExecuteAsync()
        {
            return await grupaMestaTroskaRepository.GetGrupaMestaTroskaListAsync();
        }
    }
}

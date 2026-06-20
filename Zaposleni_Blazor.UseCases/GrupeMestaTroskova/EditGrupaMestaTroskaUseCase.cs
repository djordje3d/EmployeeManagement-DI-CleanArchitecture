using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova
{
    public class EditGrupaMestaTroskaUseCase : IEditGrupaMestaTroskaUseCase
    {
        private readonly IGrupaMestaTroskaRepository grupaMestaTroskaRepository;

        public EditGrupaMestaTroskaUseCase(IGrupaMestaTroskaRepository grupaMestaTroskaRepository)
        {
            this.grupaMestaTroskaRepository = grupaMestaTroskaRepository;
        }
        public async Task<bool> ExecuteAsync(GrupaMestaTroskova grupaMestaTroskova)
        {
            return await grupaMestaTroskaRepository.UpdateGrupaMestaTroskaAsync(grupaMestaTroskova);
        }
    }
}

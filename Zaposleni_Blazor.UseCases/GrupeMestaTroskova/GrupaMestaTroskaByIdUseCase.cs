using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova
{
    public class GrupaMestaTroskaByIdUseCase : IGrupaMestaTroskaByIdUseCase
    {
        private readonly IGrupaMestaTroskaRepository grupaMestaTroskaRepository;

        public GrupaMestaTroskaByIdUseCase(IGrupaMestaTroskaRepository grupaMestaTroskaRepository)
        {
            this.grupaMestaTroskaRepository = grupaMestaTroskaRepository;
        }
        public async Task<GrupaMestaTroskova?> ExecuteAsync(int grupaMestaTroskaId)
        {
            return await grupaMestaTroskaRepository.GetGrupaMestaTroskaByIdAsync(grupaMestaTroskaId);
        }
    }
}

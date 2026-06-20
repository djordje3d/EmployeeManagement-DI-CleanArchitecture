using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije
{
    public class AddKvalifikacijaUseCase : IAddKvalifikacijaUseCase
    {
        private readonly IKvalifikacijaRepository kvalifikacijaRepository;

        public AddKvalifikacijaUseCase(IKvalifikacijaRepository kvalifikacijaRepository)
        {
            this.kvalifikacijaRepository = kvalifikacijaRepository;
        }
        public async Task<bool> ExecuteAsync(Kvalifikacija kvalifikacija)
        {
            return await kvalifikacijaRepository.AddKvalifikacijaAsync(kvalifikacija);
        }
    }
}

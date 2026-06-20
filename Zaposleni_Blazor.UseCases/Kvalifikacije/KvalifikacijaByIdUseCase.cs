using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije
{
    public class KvalifikacijaByIdUseCase : IKvalifikacijaByIdUseCase
    {
        private readonly IKvalifikacijaRepository kvalifikacijaRepository;

        public KvalifikacijaByIdUseCase(IKvalifikacijaRepository kvalifikacijaRepository)
        {
            this.kvalifikacijaRepository = kvalifikacijaRepository;
        }
        public async Task<Kvalifikacija?> ExecuteAsync(int kvalifikacijaId)
        {
            return await kvalifikacijaRepository.GetKvalifikacijaByIdAsync(kvalifikacijaId);
        }
    }
}

using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije
{
    public class ListKvalifikacijaUseCase : IListKvalifikacijaUseCase
    {
        private readonly IKvalifikacijaRepository kvalifikacijaRepository;

        public ListKvalifikacijaUseCase(IKvalifikacijaRepository kvalifikacijaRepository)
        {
            this.kvalifikacijaRepository = kvalifikacijaRepository;
        }
        public async Task<IEnumerable<Kvalifikacija>> ExecuteAsync()
        {
            return await kvalifikacijaRepository.GetKvalifikacijaListAsync();
        }
    }
}

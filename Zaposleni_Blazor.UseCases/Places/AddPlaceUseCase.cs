using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Places.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Places
{
    public class AddPlaceUseCase : IAddPlaceUseCase
    {
        private readonly IMestoRepository placeRepository;

        public AddPlaceUseCase(IMestoRepository placeRepository)
        {
            this.placeRepository = placeRepository;
        }
        public async Task<bool> ExecuteAsync(Mesto mesto)
        {
            return await placeRepository.AddMestoAsync(mesto);
        }
    }
}

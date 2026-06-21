using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Places.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Places
{
    public class PlaceByIdUseCase : IPlaceByIdUseCase
    {
        private readonly IMestoRepository placeRepository;

        public PlaceByIdUseCase(IMestoRepository placeRepository)
        {
            this.placeRepository = placeRepository;
        }
        public async Task<Mesto?> ExecuteAsync(int mestoId)
        {
            return await placeRepository.GetMestoByIdAsync(mestoId);
        }
    }
}

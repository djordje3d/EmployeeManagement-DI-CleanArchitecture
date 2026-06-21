using Zaposleni_Blazor.UseCases.Places.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Places
{
    public class DeletePlaceUseCase : IDeletePlaceUseCase
    {
        private readonly IMestoRepository placeRepository;

        public DeletePlaceUseCase(IMestoRepository placeRepository)
        {
            this.placeRepository = placeRepository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            if (id <= 0) return false;

            return await placeRepository.DeleteMestoAsync(id);
        }
    }
}

using Zaposleni_Blazor.UseCases.Mesta.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Mesta
{
    public class DeleteMestoUseCase : IDeleteMestoUseCase
    {
        private readonly IMestoRepository mestoRepository;

        public DeleteMestoUseCase(IMestoRepository mestoRepository)
        {
            this.mestoRepository = mestoRepository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            if (id <= 0) return false;

            return await mestoRepository.DeleteMestoAsync(id);
        }
    }
}

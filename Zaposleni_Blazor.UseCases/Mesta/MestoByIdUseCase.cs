using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Mesta
{
    public class MestoByIdUseCase : IMestoByIdUseCase
    {
        private readonly IMestoRepository mestoRepository;

        public MestoByIdUseCase(IMestoRepository mestoRepository)
        {
            this.mestoRepository = mestoRepository;
        }
        public async Task<Mesto?> ExecuteAsync(int mestoId)
        {
            return await mestoRepository.GetMestoByIdAsync(mestoId);
        }
    }
}

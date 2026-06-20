using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Mesta
{
    public class AddMestoUseCase : IAddMestoUseCase
    {
        private readonly IMestoRepository mestoRepository;

        public AddMestoUseCase(IMestoRepository mestoRepository)
        {
            this.mestoRepository = mestoRepository;
        }
        public async Task<bool> ExecuteAsync(Mesto mesto)
        {
            return await mestoRepository.AddMestoAsync(mesto);
        }
    }
}

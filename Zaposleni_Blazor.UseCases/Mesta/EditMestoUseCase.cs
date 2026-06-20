using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Mesta
{
    public class EditMestoUseCase : IEditMestoUseCase
    {
        private readonly IMestoRepository mestoRepository;

        public EditMestoUseCase(IMestoRepository mestoRepository)
        {
            this.mestoRepository = mestoRepository;
        }
        public async Task<bool> ExecuteAsync(Mesto mesto)
        {
            return await mestoRepository.UpdateMestoAsync(mesto);
        }
    }
}

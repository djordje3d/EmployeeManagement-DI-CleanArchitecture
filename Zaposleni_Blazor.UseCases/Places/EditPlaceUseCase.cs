using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Places.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Places
{
    public class EditPlaceUseCase : IEditPlaceUseCase
    {
        private readonly IMestoRepository placeRepository;

        public EditPlaceUseCase(IMestoRepository placeRepository)
        {
            this.placeRepository = placeRepository;
        }
        public async Task<bool> ExecuteAsync(Mesto mesto)
        {
            return await placeRepository.UpdateMestoAsync(mesto);
        }
    }
}

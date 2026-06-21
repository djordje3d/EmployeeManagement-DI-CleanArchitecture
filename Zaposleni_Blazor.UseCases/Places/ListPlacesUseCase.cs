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
    public class ListPlacesUseCase : IListPlacesUseCase
    {
        private readonly IMestoRepository placeRepository;

        public ListPlacesUseCase(IMestoRepository placeRepository)
        {
            this.placeRepository = placeRepository;
        }
        public async Task<IEnumerable<Mesto>> ExecuteAsync()
        {
            return await placeRepository.GetMestoListAsync();
        }
    }
}

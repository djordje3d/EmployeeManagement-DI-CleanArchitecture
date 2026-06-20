using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova
{
    public class DeleteGrupaMestaTroskaUseCase : IDeleteGrupaMestaTroskaUseCase
    {
        private readonly IGrupaMestaTroskaRepository grupaMestaTroskaRepository;

        public DeleteGrupaMestaTroskaUseCase(IGrupaMestaTroskaRepository grupaMestaTroskaRepository)
        {
            this.grupaMestaTroskaRepository = grupaMestaTroskaRepository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            return await grupaMestaTroskaRepository.DeleteGrupaMestaTroskaAsync(id);
        }
    }
}

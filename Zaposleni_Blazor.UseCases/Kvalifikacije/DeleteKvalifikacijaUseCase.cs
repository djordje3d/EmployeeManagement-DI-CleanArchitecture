using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije
{
    public class DeleteKvalifikacijaUseCase : IDeleteKvalifikacijaUseCase
    {
        private readonly IKvalifikacijaRepository kvalifikacijaRepository;

        public DeleteKvalifikacijaUseCase(IKvalifikacijaRepository kvalifikacijaRepository)
        {
            this.kvalifikacijaRepository = kvalifikacijaRepository;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            if (id <= 0) return false;

            return await kvalifikacijaRepository.DeleteKvalifikacijaAsync(id);
        }
    }
}

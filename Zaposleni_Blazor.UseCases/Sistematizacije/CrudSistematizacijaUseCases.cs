using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;
using Zaposleni_Blazor.UseCases.Sistematizacije.Interfaces;

namespace Zaposleni_Blazor.UseCases.Sistematizacije
{
    public class CrudSistematizacijaUseCases : ICrudSistematizacijaUseCases
    {
        private readonly ISistematizacijaRepository sistematizacijaRepository;

        public CrudSistematizacijaUseCases(ISistematizacijaRepository sistematizacijaRepository)
        {
            this.sistematizacijaRepository = sistematizacijaRepository;
        }
        public async Task<bool> AddAsync(Sistematizacija sistematizacija)
        {
            return await sistematizacijaRepository.AddSistematizacijaAsync(sistematizacija);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await sistematizacijaRepository.DeleteSistematizacijaAsync(id);
        }

        public async Task<IEnumerable<Sistematizacija>> GetAllAsync()
        {
            return await sistematizacijaRepository.GetSistematizacijaListAsync();
        }

        public async Task<Sistematizacija?> GetByIdAsync(int id)
        {
            return await sistematizacijaRepository.GetSistematizacijaByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Sistematizacija sistematizacija)
        {
            return await sistematizacijaRepository.UpdateSistematizacijaAsync(sistematizacija);
        }
        public async Task<IEnumerable<Kvalifikacija>> GetAllKvalifikacijeAsync()
        {
            return await sistematizacijaRepository.GetAllKvalifikacijeAsync();
        }

        public async Task<IEnumerable<OrganizacioneJedinice>> GetAllOrganizacioneJediniceAsync()
        {
            return await sistematizacijaRepository.GetAllOrganizacioneJediniceAsync();
        }
    }
}
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.PluginInterfaces;
using Zaposleni_Blazor.UseCases.Zaposleni.Interfaces;

namespace Zaposleni_Blazor.UseCases.Zaposleni
{
    public class CrudZaposlenUseCases : ICrudZaposlenUseCases
    {
        private readonly IZaposlenRepository zaposlenRepository;

        public CrudZaposlenUseCases(IZaposlenRepository zaposlenRepository)
        {
            this.zaposlenRepository = zaposlenRepository;
        }

        public async Task<bool> AddAsync(Zaposlen zaposlen)
        {
            return await zaposlenRepository.AddZaposlenAsync(zaposlen);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await zaposlenRepository.DeleteZaposlenAsync(id);
        }

        public async Task<IEnumerable<Zaposlen>> GetAllAsync()
        {
            return await zaposlenRepository.GetZaposlenListAsync();
        }

        public async Task<Zaposlen?> GetByIdAsync(int id)
        {
            return await zaposlenRepository.GetZaposlenByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Zaposlen zaposlen)
        {
            return await zaposlenRepository.UpdateZaposlenAsync(zaposlen);
        }

        public async Task<IEnumerable<Kvalifikacija>> GetAllKvalifikacijeAsync()
        {
            return await zaposlenRepository.GetAllKvalifikacijeAsync();
        }

        public async Task<IEnumerable<Mesto>> GetAllMestaAsync()
        {
            return await zaposlenRepository.GetAllMesta();
        }

        public async Task<IEnumerable<Sistematizacija>> GetAllSistematizacija()
        {
            return await zaposlenRepository.GetAllSistematizacija();
        }
    }
}

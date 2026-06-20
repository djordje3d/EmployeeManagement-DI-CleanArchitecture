using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.ClanDomacinstva.Interfaces;
using Zaposleni_Blazor.UseCases.PluginInterfaces;

namespace Zaposleni_Blazor.UseCases.ClanDomacinstva
{
    public class CrudClanoviDomacinstvaUseCases : ICrudClanoviDomacinstvaUseCases
    {
        private readonly IClanDomacinstvaRepository clanDomacinstvaRepository;

        public CrudClanoviDomacinstvaUseCases(IClanDomacinstvaRepository clanDomacinstvaRepository)
        {
            this.clanDomacinstvaRepository = clanDomacinstvaRepository;
        }
        public async Task<bool> AddAsync(ClanoviDomacinstva clan)
        {
            return await clanDomacinstvaRepository.AddClanDomacinstvaAsync(clan);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await clanDomacinstvaRepository.DeleteClanDomacinstvaAsync(id);
        }

        public async Task<IEnumerable<ClanoviDomacinstva>> GetAllAsync()
        {
            return await clanDomacinstvaRepository.GetClanoviDomacinstvaListAsync();
        }

        public async Task<IEnumerable<Zaposlen>> GetAllZaposleniAsync()
        {
            return await clanDomacinstvaRepository.GetZaposleniListAsync();
        }

        public async Task<ClanoviDomacinstva?> GetByIdAsync(int id)
        {
            return await clanDomacinstvaRepository.GetClanDomacinstvaByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(ClanoviDomacinstva clan)
        {
            return await clanDomacinstvaRepository.UpdateClanDomacinstvaAsync(clan);
        }
    }
}

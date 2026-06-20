using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.UseCases.Interfaces;

namespace Zaposleni_Clean_MVC_API.UseCases
{
    public class CrudOrganizacionaJedinicaUseCases : ICrudOrganizacionaJedinicaUseCases
    {
        private readonly IWebApiExecuter _webApiExecuter;

        public CrudOrganizacionaJedinicaUseCases(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }
        public async Task<OrganizacioneJedinice?> AddAsync(OrganizacioneJedinice orgJed)
        {
            return await _webApiExecuter.InvokePost<OrganizacioneJedinice>("organizacionejedinice", orgJed);
        }

        public async Task DeleteAsync(int orgJedId)
        {
           await _webApiExecuter.InvokeDelete($"organizacionejedinice/{orgJedId}");
        }

        public async Task<IEnumerable<OrganizacioneJedinice>> GetAllAsync()
        {
            return await _webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
        }

        public async Task<IEnumerable<GrupaMestaTroskova>> GetAllGrupaMestaTroskovaAsync()
        {
            return await _webApiExecuter.InvokeGet<List<GrupaMestaTroskova>>("organizacionejedinice");
        }

        public async Task<OrganizacioneJedinice?> GetByIdAsync(int orgJedId)
        {
            return await _webApiExecuter.InvokeGet<OrganizacioneJedinice>($"organizacionejedinice/{orgJedId}");
        }

        public async Task<bool> UpdateAsync(OrganizacioneJedinice orgJed)
        {
            try
            {
                await _webApiExecuter.InvokePut($"organizacionejedinice/{orgJed.Id}", orgJed);
                return true;
            }
            catch (WebApiException ex)
            {
                // Opcionalno logovanje ili ponovno bacanje
                Console.WriteLine(ex.Message); // ili koristi logger
                return false;
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class OJ_ValidateUpdateOJFilterAttribute : Attribute, IAsyncActionFilter // IAsyncActionFilter omogućava asinhrono izvršavanje filtera
    {
        private readonly ApplicationDbContext db;

        public OJ_ValidateUpdateOJFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Provera da li su prisutni id i model
            if (!context.ActionArguments.TryGetValue("id", out var idObj) || idObj is not int id)
            {
                context.Result = new BadRequestObjectResult("ID nije prosleđen ili nije validan.");
                return;
            }

            if (!context.ActionArguments.TryGetValue("orgJed", out var orgJedObj) || orgJedObj is not OrganizacioneJedinice orgJed)
            {
                context.Result = new BadRequestObjectResult("Telo zahteva je prazno ili nije validno.");
                return;
            }

            // Poklapanje ID-jeva
            if (id != orgJed.Id)
            {
                context.ModelState.AddModelError("Id", "ID iz URL-a se ne poklapa sa ID-jem iz tela zahteva.");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }

            // Provera duplikata u bazi
            var postojiOJ = db.OrganizacioneJedinice.FirstOrDefault(k =>
                k.Id != orgJed.Id &&
                k.OJ!.ToLower() == orgJed.OJ!.ToLower() &&
                k.Naziv!.ToLower() == orgJed.Naziv!.ToLower());

            if (postojiOJ != null)
            {
                context.ModelState.AddModelError("OrganizacioneJedinice", "OJ sa istim oznakama već postoji."); 
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState));
                return;
            }

            // Sve validacije prošle – nastavi sa izvršavanjem akcije
            await next();
        }
    }
}

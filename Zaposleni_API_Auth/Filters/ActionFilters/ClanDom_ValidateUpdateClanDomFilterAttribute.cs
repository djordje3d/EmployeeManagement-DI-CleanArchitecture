using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
//using Zaposleni_API_Auth.Models;
using Zaposleni.Plugins.EFCoreSqlServer;
using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class ClanDom_ValidateUpdateClanDomFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ApplicationDbContext db;

        public ClanDom_ValidateUpdateClanDomFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) // OnActionExecuting → metoda koja se izvršava pre izvršenja akcije kontrolera.
                                                                                                               // svrha u ovoj klasi je da: Validira da li je id iz URL-a jednak Id iz modela(zaposlen).
        {
            // Proveri da li su prosleđeni parametri prisutni
            if (!context.ActionArguments.TryGetValue("id", out var idObj) || !(idObj is int id) ||
                !context.ActionArguments.TryGetValue("clanDom", out var clanObj) || !(clanObj is ClanoviDomacinstva clanDomacinstva))
            {
                context.Result = new BadRequestObjectResult("Invalid request data.");
                return;
            }


            // Validacija ID-ja
            if (id != clanDomacinstva.Id)
            {
                context.ModelState.AddModelError("Id", "Id iz URL-a nije isti kao Id u telu zahteva.");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });
                return;
            }

            // Provera da li već postoji član sa istim podacima
            var existingClan = await db.ClanoviDomacinstva.FirstOrDefaultAsync(m =>
                m.Id != clanDomacinstva.Id &&
                (m.ImeClana ?? "").ToLower() == (clanDomacinstva.ImeClana ?? "").ToLower() &&
                (m.PolClana ?? "").ToLower() == (clanDomacinstva.PolClana ?? "").ToLower() &&
                (m.SroClana ?? "").ToLower() == (clanDomacinstva.SroClana ?? "").ToLower() &&
                m.DatumRodjenjaClana == clanDomacinstva.DatumRodjenjaClana &&
                (m.StatusClana ?? "").ToLower() == (clanDomacinstva.StatusClana ?? "").ToLower() &&
                (m.JMBG ?? "").ToLower() == (clanDomacinstva.JMBG ?? "").ToLower() &&
                (m.Roditelj ?? "").ToLower() == (clanDomacinstva.Roditelj ?? "").ToLower());

            if (existingClan != null)
            {
                context.ModelState.AddModelError("Član", "Član sa istim JMBG i podacima već postoji.");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });
                return;
            }

            // Ako postoje greške, prekidamo dalje izvršavanje
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });
                return;
            }

            // Ako su svi uslovi ispunjeni, nastavi sa izvršavanjem akcije

            await next(); // next() poziva sledeći filter ili akciju kontrolera. Ako nema više filtera, izvršava se akcija kontrolera.
        }
    }
}

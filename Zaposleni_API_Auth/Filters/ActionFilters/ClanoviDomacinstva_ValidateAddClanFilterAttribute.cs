using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class ClanoviDomacinstva_ValidateAddClanFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public ClanoviDomacinstva_ValidateAddClanFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var clanDomacinstva = context.ActionArguments["clanDom"] as ClanoviDomacinstva; // Argument (u ovom slučaju clanDom) se mora zvati isto kao parametar u API metodi 

            // Provera da li je objekat NULL
            if (clanDomacinstva == null)
            {
                context.ModelState.AddModelError("Clan", "Clan object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            // Provera da li već postoji mesto sa istim Naziv, Opstina i PostanskiBroj
            var existingClan = db.ClanoviDomacinstva.FirstOrDefault(m =>
                m.ImeClana!.ToLower() == clanDomacinstva.ImeClana!.ToLower() &&
                m.PolClana!.ToLower() == clanDomacinstva.PolClana!.ToLower() &&
                m.SroClana!.ToLower() == clanDomacinstva.SroClana!.ToLower() &&
                m.DatumRodjenjaClana == clanDomacinstva.DatumRodjenjaClana &&
                m.StatusClana!.ToLower() == clanDomacinstva.StatusClana!.ToLower() &&
                m.JMBG!.ToLower() == clanDomacinstva.JMBG!.ToLower() && 
                m.Roditelj!.ToLower() == clanDomacinstva.Roditelj!.ToLower());

            if (existingClan != null)
            {
                context.ModelState.AddModelError("Clan", "Clan with the same ???????????????????? already EXISTS!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            // Ako postoje greške, prekidamo dalje izvršavanje
            if (!context.ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class Kvalifikacija_ValidateAddKvalifikacijaFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Kvalifikacija_ValidateAddKvalifikacijaFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var kvalifikacija = context.ActionArguments["kvalifikacija"] as Kvalifikacija;

            // Provera da li je objekat NULL
            if (kvalifikacija == null)
            {
                context.ModelState.AddModelError("Kvalifikacija", "Kvalifikacija object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            // Provera da li već postoji kvalifikacija sa istim LicniStepenKv i Naziv
            var existingKvalifikacija = db.Kvalifikacije.FirstOrDefault(k =>
                k.LicniStepenKv!.ToLower() == kvalifikacija.LicniStepenKv!.ToLower() &&
                k.Naziv!.ToLower() == kvalifikacija.Naziv!.ToLower());

            if (existingKvalifikacija != null)
            {
                context.ModelState.AddModelError("Kvalifikacija", "Kvalifikacija with the same Licni Stepen Kv and Naziv already EXISTS!");

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

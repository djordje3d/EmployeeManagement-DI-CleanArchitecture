using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni_Blazor.CoreBusiness;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class OJ_ValidateAdd_OJFilterAttribute : ActionFilterAttribute
    {
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db;

        public OJ_ValidateAdd_OJFilterAttribute(Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var organizacionaJedinica = context.ActionArguments["orgJed"] as OrganizacioneJedinice;

            // Provera da li je objekat NULL
            if (organizacionaJedinica == null)
            {
                context.ModelState.AddModelError("OrganizacioneJedinice", "OJ object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            // Provera da li već postoji kvalifikacija sa istim LicniStepenKv i Naziv
            var existingOJ= db.OrganizacioneJedinice.FirstOrDefault(k =>
                k.OJ!.ToLower() == organizacionaJedinica.OJ!.ToLower() &&
                k.Naziv!.ToLower() == organizacionaJedinica.Naziv!.ToLower());

            if (existingOJ != null)
            {
                context.ModelState.AddModelError("OrganizacioneJedinice", "OJ Object with the same OJ and Naziv already EXISTS!");

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

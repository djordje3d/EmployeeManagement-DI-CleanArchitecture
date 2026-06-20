using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class GrMestaTroska_ValidateAdd_GrMTFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public GrMestaTroska_ValidateAdd_GrMTFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var grupaMestaTroskova = context.ActionArguments["grupamestatroskova"] as GrupaMestaTroskova;

            // Provera da li je objekat NULL
            if (grupaMestaTroskova == null)
            {
                context.ModelState.AddModelError("GrupaMestaTroskova", "GrupaMestaTroskova object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            // Provera da li već postoji kvalifikacija sa istim LicniStepenKv i Naziv
            var existingGrupaMestaTroska = db.GrupeMestaTroskova.FirstOrDefault(k =>
                (k.Grupa ?? "").ToLower() == (grupaMestaTroskova.Grupa ?? "").ToLower() &&
                (k.Naziv ?? "").ToLower() == (grupaMestaTroskova.Naziv ?? "").ToLower());

            if (existingGrupaMestaTroska != null)
            {
                context.ModelState.AddModelError("GrupaMestaTroskova", "GrupaMestaTroskova with the same Grupa, and Naziv already EXISTS!");

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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class Mesto_ValidateAddMestoFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Mesto_ValidateAddMestoFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var mesto = context.ActionArguments["mesto"] as Mesto;

            // Provera da li je objekat NULL
            if (mesto == null)
            {
                context.ModelState.AddModelError("Mesto", "Mesto object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }

            // Provera da li već postoji mesto sa istim Naziv, Opstina i PostanskiBroj
            var existingMesto = db.Mesta.FirstOrDefault(m =>
                m.Naziv!.ToLower() == mesto.Naziv!.ToLower() &&
                m.Opstina!.ToLower() == mesto.Opstina!.ToLower() &&
                m.PostanskiBroj!.ToLower() == mesto.PostanskiBroj!.ToLower());

            if (existingMesto != null)
            {
                context.ModelState.AddModelError("Mesto", "Mesto with the same Naziv, Opština, and Poštanski Broj already EXISTS!");

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

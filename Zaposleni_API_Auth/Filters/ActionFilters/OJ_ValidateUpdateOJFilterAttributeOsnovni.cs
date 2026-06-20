using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
//using Zaposleni_API_Auth.Data;
using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class OJ_ValidateUpdateOJFilterAttributeOsnovni : ActionFilterAttribute
    {
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db;

        public OJ_ValidateUpdateOJFilterAttributeOsnovni(Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db)
        {
            this.db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context) // OnActionExecuting → metoda koja se izvršava pre izvršenja akcije kontrolera.
                                                                               // svrha u ovoj klasi je da: Validira da li je id iz URL-a jednak Id iz modela(zaposlen).
        {
            base.OnActionExecuting(context);

            var id = context.ActionArguments["id"] as int?;
            var orgJed = context.ActionArguments["orgjed"] as OrganizacioneJedinice;

            if (id.HasValue && orgJed == null && id != orgJed.Id)
            {
                context.ModelState.AddModelError("Id", "Id is not same as id");
                var problemDetails = new ValidationProblemDetails(context.ModelState) // ValidationProblemDetails → ugrađena klasa koja se koristi za slanje grešaka vezanih za validaciju.
                // Osigurava da se greške odgovarajuće formatiraju u JSON odgovoru.
                // context.ModelState → sadrži sve greške validacije koje su se desile tokom model bindinga..
                {
                    Status = StatusCodes.Status400BadRequest // 
                };
                context.Result = new BadRequestObjectResult(problemDetails); // BadRequestObjectResult → ugrađena klasa koja se koristi za vraćanje HTTP 400 Bad Request statusa sa JSON telom (context).
            }

            // Provera da li već postoji kvalifikacija sa istim LicniStepenKv i Naziv
            var existingOJ = db.OrganizacioneJedinice.FirstOrDefault(k => k.Id != orgJed.Id &&
                k.OJ!.ToLower() == orgJed.OJ!.ToLower() &&
                k.Naziv!.ToLower() == orgJed.Naziv!.ToLower());

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

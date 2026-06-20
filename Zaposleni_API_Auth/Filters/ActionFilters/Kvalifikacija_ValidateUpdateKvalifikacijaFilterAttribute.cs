using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class Kvalifikacija_ValidateUpdateKvalifikacijaFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ApplicationDbContext db;

        public Kvalifikacija_ValidateUpdateKvalifikacijaFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var id = context.ActionArguments["id"] as int?;
            var kvalifikacija = context.ActionArguments["kvalifikacija"] as Kvalifikacija;

            if (id.HasValue && kvalifikacija != null && id != kvalifikacija.Id)
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

            // Provera da li već postoji Kvalifikacija sa istim Naziv i licni stepen kvalifikacije
            var existingKvalifikacija = db.Kvalifikacije.FirstOrDefault(m => m.Id != kvalifikacija.Id &&
                m.LicniStepenKv!.ToLower() == kvalifikacija.LicniStepenKv!.ToLower() &&
                m.Naziv!.ToLower() == kvalifikacija.Naziv!.ToLower());

            if (existingKvalifikacija != null)
            {
                context.ModelState.AddModelError("Kvalifikacija", "Kvalifikacija with the same 'Lični stepen kvalifikacije' and 'Naziv' already EXISTS!");

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
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });

                return;
            }

            await next(); // nastavi sa izvršavanjem akcije ako je sve u redu
        }
    }
}

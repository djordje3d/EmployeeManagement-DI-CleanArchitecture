using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni.Plugins.EFCoreSqlServer;
//using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class Mesto_ValidateUpdateMestoFilterAttribute : Attribute, IAsyncActionFilter
    {

        private readonly ApplicationDbContext db;

        public Mesto_ValidateUpdateMestoFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = context.ActionArguments["id"] as int?;
            var mesto = context.ActionArguments["mesto"] as Mesto ;

            if (id.HasValue && mesto != null && id != mesto.Id)
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
            var existingMesto = db.Mesta.FirstOrDefault(m => m.Id != mesto.Id &&
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
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });

                return;
            }

            // Ako je sve u redu, nastavi ka kontroleru
            await next();
        }
    }
}

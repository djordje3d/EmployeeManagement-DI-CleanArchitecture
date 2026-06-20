using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class GrMestaTroska_ValidateUpdateGrMTFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ApplicationDbContext db;

        public GrMestaTroska_ValidateUpdateGrMTFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var id = context.ActionArguments["id"] as int?;
            var grupaMestaTroskova = context.ActionArguments["grupaMestaTroskova"] as GrupaMestaTroskova; // Pod navodnicima isti naziv kao ulazni PARAMETAR METODE !!!

            if (id.HasValue && grupaMestaTroskova != null && id != grupaMestaTroskova.Id)
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
            var existingGrupaMestaTroska = db.GrupeMestaTroskova.FirstOrDefault(k => k.Id != grupaMestaTroskova.Id &&
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
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });

                return;
            }

            await next(); // sve prošlo, pozivamo sledeći middleware ili akciju
        }
    }
}

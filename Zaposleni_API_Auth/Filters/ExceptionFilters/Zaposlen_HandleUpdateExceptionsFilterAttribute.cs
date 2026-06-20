using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;

namespace Zaposleni_API_Auth.Filters.ExceptionFilters
{
    public class Zaposlen_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        // Umesto klasičnog try-catch bloka u kontroleru, koristi ExceptionFilterAttribute, što omogućava centralizovanu obradu izuzetaka u aplikaciji.

        public Zaposlen_HandleUpdateExceptionsFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnException(ExceptionContext context) // // context objekat sadrži informacije o izuzetku, ruti i HTTP zahtevu.
        {
            base.OnException(context); // osiguranje da osnovna implementacija ExceptionFilterAttribute bude izvršena pre prilagođenih provera.

            //pravimo umesto Catch bloka - provera da li je Zaposlen obrisan pre Update

            var strZaposlenId = context.RouteData.Values["id"] as string;
            // key je ime parametra - id, ovo će nam vratiti objekat

            if (int.TryParse(strZaposlenId, out int zaposlenId))
            {
                if (db.Zaposleni.FirstOrDefault(x => x.Id == zaposlenId) == null) // Ako je neko obrisao Zaposlen pre nego je uradjen Update iz Try bloka
                {
                    context.ModelState.AddModelError("Id", "Zaposlen doesn't exist anymore"); // 
                    
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        //Status = StatusCodes.Status400BadRequest
                        Status = StatusCodes.Status404NotFound //u Controlleru je u catch bloku bilo Notfound()
                    };
                    context.Result = new NotFoundObjectResult(problemDetails); // Postavlja rezultat odgovora tako da klijent dobije JSON sa detaljima greške.
                                                                               // NotFoundObjectResult(problemDetails) – Vraća HTTP odgovor 404 Not Found sa podacima iz problemDetails.
                }
            }
        }
    }
}

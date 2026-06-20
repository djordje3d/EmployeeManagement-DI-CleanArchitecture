using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Zaposleni.Plugins.EFCoreSqlServer;
//using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth.Filters.ExceptionFilters
{
    public class Mesto_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        // Umesto klasičnog try-catch bloka u kontroleru, koristi ExceptionFilterAttribute, što omogućava centralizovanu obradu izuzetaka u aplikaciji.

        public Mesto_HandleUpdateExceptionsFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnException(ExceptionContext context) // // context objekat sadrži informacije o izuzetku, ruti i HTTP zahtevu.
        {
            base.OnException(context); // osiguranje da osnovna implementacija ExceptionFilterAttribute bude izvršena pre prilagođenih provera.

            //pravimo umesto Catch bloka - provera da li je Mesto obrisano pre Update

            var strMestoId = context.RouteData.Values["id"] as string;
            // key je ime parametra - id, ovo će nam vratiti objekat

            if (int.TryParse(strMestoId, out int mestoId))
            {
                if (db.Mesta.FirstOrDefault(x => x.Id == mestoId) == null) // Ako je neko obrisao Mesto pre nego je uradjen Update iz Try bloka
                {
                    context.ModelState.AddModelError("Id", "Mesto doesn't exist anymore"); // 

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

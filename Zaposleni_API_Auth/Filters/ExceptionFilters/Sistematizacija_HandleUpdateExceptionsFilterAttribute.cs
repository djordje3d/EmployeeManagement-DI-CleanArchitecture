using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
//using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth.Filters.ExceptionFilters
{
    public class Sistematizacija_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute // ExceptionFilterAttribute omogućava centralizovanu obradu izuzetaka u ASP.NET Core aplikacijama.
                                                                                                  // Ovaj filter se može koristiti za hvatanje i obradu izuzetaka koji se javljaju tokom izvršavanja akcija u kontrolerima.
                                                                                                  // U ovom slučaju, koristi se za proveru da li je Sistematizacija obrisana pre nego što se pokuša ažuriranje.
    {
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db;

        // Umesto klasičnog try-catch bloka u kontroleru, koristi ExceptionFilterAttribute, što omogućava centralizovanu obradu izuzetaka u aplikaciji.

        public Sistematizacija_HandleUpdateExceptionsFilterAttribute(Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnException(ExceptionContext context) // // context objekat sadrži informacije o izuzetku, ruti i HTTP zahtevu.
        {
            base.OnException(context); // osiguranje da osnovna implementacija ExceptionFilterAttribute bude izvršena pre prilagođenih provera.

            //pravimo umesto Catch bloka - provera da li je Sistematizacija obrisan pre Update

            var strSistematizacijaId = context.RouteData.Values["id"] as string;
            // key je ime parametra - id, ovo će nam vratiti objekat

            if (int.TryParse(strSistematizacijaId, out int sistematizacijaId))
            {
                if (db.Sistematizacije.FirstOrDefault(x => x.Id == sistematizacijaId) == null) // Ako je neko obrisao Sistematizacija pre nego je uradjen Update iz Try bloka
                {
                    context.ModelState.AddModelError("Id", "Sistematizacija doesn't exist anymore"); // 

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

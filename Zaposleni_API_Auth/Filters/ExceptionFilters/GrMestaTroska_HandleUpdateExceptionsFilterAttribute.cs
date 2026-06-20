using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zaposleni_API_Auth.Filters.ExceptionFilters
{
    public class GrMestaTroska_HandleUpdateExceptionsFilterAttribute : ExceptionFilterAttribute
    {
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db;

        // Umesto klasičnog try-catch bloka u kontroleru, koristi ExceptionFilterAttribute, što omogućava centralizovanu obradu izuzetaka u aplikaciji.

        public GrMestaTroska_HandleUpdateExceptionsFilterAttribute(Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnException(ExceptionContext context) // // context objekat sadrži informacije o izuzetku, ruti i HTTP zahtevu.
        {
            base.OnException(context); // osiguranje da osnovna implementacija ExceptionFilterAttribute bude izvršena pre prilagođenih provera.

            //pravimo umesto Catch bloka - provera da li je Grupa MT obrisano pre Update

            var strGrMTId = context.RouteData.Values["id"] as string;
            // key je ime parametra - id, ovo će nam vratiti objekat

            if (int.TryParse(strGrMTId, out int grMtId))
            {
                if (db.Mesta.FirstOrDefault(x => x.Id == grMtId) == null) // Ako je neko obrisao Grupu MT pre nego je uradjen Update iz Try bloka
                {
                    context.ModelState.AddModelError("Id", "Grupa MT doesn't exist anymore"); // 

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

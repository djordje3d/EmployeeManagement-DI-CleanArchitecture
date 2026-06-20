using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
////using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class GrMestaTroska_ValidateGrMestaTroskaIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;


        // Ova klasa nasleđuje ActionFilterAttribute, što znači da može biti primenjena na API akcije i da će se njen kod izvršiti pre ili posle akcije.
        // Postoji mnogo metoda koje se mogu uporebiti za validaciju, ali ovde uzimamo OnActionExecuting

        public GrMestaTroska_ValidateGrMestaTroskaIdFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context) // context sadrži podatke o trenutnom HTTP zahtevu, uključujući parametre metode API-ja.
                                                                               // OnActionExecuting je metoda koja se izvršava PRE nego što se akcija u kontroleru pokrene.
        {
            base.OnActionExecuting(context);

            var grupaMestaTroskaId = context.ActionArguments["id"] as int?;

            if (grupaMestaTroskaId.HasValue)
            {
                if (grupaMestaTroskaId.Value <= 0)
                {
                    context.ModelState.AddModelError("Id", "Id is invalid!");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else
                {
                    var grupaMestaTroska = db.GrupeMestaTroskova.Find(grupaMestaTroskaId.Value);

                    if (grupaMestaTroska == null)
                    {
                        context.ModelState.AddModelError("Id", "Mesto doesn't exist! ");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);  // Postavlja rezultat odgovora tako da klijent dobije JSON sa detaljima greške.
                    }
                    else
                    {
                        // 3:41  Prosleđivanje grupamestatroska objekta iz akcionog filtera u Controller
                        context.HttpContext.Items["grupamestatroska"] = grupaMestaTroska; //HttpContext privremeno upisuje(čuva) shirt objekat (Slično kao sesija u MVC)
                        // Pod navodnicima je proizvoljan naziv, ali mora biti isti i u Contorlleru gde se poziva objekat preko HttpkContexta
                    }

                }
            }

        }
    }
}

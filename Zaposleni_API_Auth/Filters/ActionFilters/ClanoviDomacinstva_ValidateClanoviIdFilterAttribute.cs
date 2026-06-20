using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Zaposleni.Plugins.EFCoreSqlServer;
//using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class ClanoviDomacinstva_ValidateClanoviIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        // Ova klasa nasleđuje ActionFilterAttribute, što znači da može biti primenjena na API akcije i da će se njen kod izvršiti pre ili posle akcije.
        // Postoji mnogo metoda koje se mogu uporebiti za validaciju, ali ovde uzimamo OnActionExecuting

        public ClanoviDomacinstva_ValidateClanoviIdFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context) // context sadrži podatke o trenutnom HTTP zahtevu, uključujući parametre metode API-ja.
                                                                               // OnActionExecuting je metoda koja se izvršava PRE nego što se akcija u kontroleru pokrene.
        {
            base.OnActionExecuting(context);

            var clanDomacinstvaId = context.ActionArguments["id"] as int?;

            if (clanDomacinstvaId.HasValue)
            {
                if (clanDomacinstvaId.Value <= 0)
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
                    var clanDomacinstva = db.ClanoviDomacinstva.Find(clanDomacinstvaId.Value);

                    if (clanDomacinstva == null)
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
                        // 3:41  Prosleđivanje shirt objekta iz akcionog filtera u Controller
                        context.HttpContext.Items["clanovidomacinstva"] = clanDomacinstva; //HttpContext privremeno upisuje(čuva) clanovidomacinstva objekat (Slično kao sesija u MVC)
                        // Pod navodnicima je proizvoljan naziv, ali mora biti isti i u Contorlleru gde se poziva objekat preko HttpkContexta
                    }

                }
            }

        }
    }
}

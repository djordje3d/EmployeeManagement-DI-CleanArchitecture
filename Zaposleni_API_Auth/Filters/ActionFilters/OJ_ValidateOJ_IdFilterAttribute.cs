using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
//using Zaposleni_API_Auth.Data;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class OJ_ValidateOJ_IdFilterAttribute : ActionFilterAttribute
    {
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db;

        public OJ_ValidateOJ_IdFilterAttribute(Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context) // context sadrži podatke o trenutnom HTTP zahtevu, uključujući parametre metode API-ja.
                                                                               // OnActionExecuting je metoda koja se izvršava PRE nego što se akcija u kontroleru pokrene.
        {
            base.OnActionExecuting(context);

            var orgJedId = context.ActionArguments["id"] as int?;

            if (orgJedId.HasValue)
            {
                if (orgJedId.Value <= 0)
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
                    var orgJedinica = db.OrganizacioneJedinice.Find(orgJedId.Value);

                    if (orgJedinica == null)
                    {
                        context.ModelState.AddModelError("Id", "OJ doesn't exist! ");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new BadRequestObjectResult(problemDetails);  // Postavlja rezultat odgovora tako da klijent dobije JSON sa detaljima greške.
                    }
                    else
                    {
                        // 3:41  Prosleđivanje shirt objekta iz akcionog filtera u Controller
                        context.HttpContext.Items["orgjed"] = orgJedinica; //HttpContext privremeno upisuje(čuva) orgJedinica objekat (Slično kao sesija u MVC)
                        // Pod navodnicima je proizvoljan naziv, ali mora biti isti i u Contorlleru gde se poziva objekat preko HttpkContexta
                    }

                }
            }

        }
    }
}

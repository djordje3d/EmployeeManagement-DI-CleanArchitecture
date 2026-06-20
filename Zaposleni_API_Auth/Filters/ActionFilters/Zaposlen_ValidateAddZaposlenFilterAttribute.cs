using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class Zaposlen_ValidateAddZaposlenFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Zaposlen_ValidateAddZaposlenFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var zaposlen = context.ActionArguments["zaposlen"] as Zaposlen;

            if (zaposlen == null)
            {
                context.ModelState.AddModelError("Zaposlen", "Zaposlen object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);

                return;
            }

            // Provera da li isti zaposlen već postoji. ?? da bi se izbegla greška ako je neko polje NULL, postavlja se na prazan string

            var existingZaposlen = db.Zaposleni.FirstOrDefault(x => 
                (x.Ime ?? "").ToLower() == (zaposlen.Ime ?? "").ToLower() &&
                (x.Prezime ?? "").ToLower() == (zaposlen.Prezime ?? "").ToLower() &&
                (x.Roditelj ?? "").ToLower() == (zaposlen.Roditelj ?? "").ToLower() &&
                (x.JMBG ?? "").ToLower() == (zaposlen.JMBG ?? "").ToLower());


            if (existingZaposlen != null) // Ako postoji zaposlen sa istim imenom, prezimenom, roditeljem i JMBG-om
            {
                context.ModelState.AddModelError("Zaposlen", "Zaposlen already EXISTS!");
            }

            // Provera datuma
            if (zaposlen.DatumRodjenja > DateTime.Today)
            {
                context.ModelState.AddModelError("DatumRodjenja", "Datum rođenja ne može biti u budućnosti.");
            }

            if (zaposlen.Pocetak_RadnogOd > zaposlen.Kraj_RadnogOd)
            {
                context.ModelState.AddModelError("RadniOdnos", "Datum početka radnog odnosa ne može biti posle kraja.");
            }

            // Provera MestoId (strani ključ)
            if (zaposlen.MestoId == null || !db.Mesta.Any(m => m.Id == zaposlen.MestoId)) // Ako je MestoId NULL ili ne postoji u bazi
            {
                context.ModelState.AddModelError("MestoId", "Odabrano mesto ne postoji u bazi.");
            }

            // Ako postoje greške, prekidamo dalje izvršavanje

            if (!context.ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState) // Pravi JSON objekat sa greškama. ModelState sadrži sve greške
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);
            }


        }
    }
}

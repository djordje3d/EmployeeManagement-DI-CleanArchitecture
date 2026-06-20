using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni.Plugins.EFCoreSqlServer;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class Zaposlen_ValidateUpdateZaposlenFilterAttributeOsnovni : Attribute, IAsyncActionFilter // IAsyncActionFilter omogućava asinhrono izvršavanje filtera
    {
        private readonly ApplicationDbContext db;

        public Zaposlen_ValidateUpdateZaposlenFilterAttributeOsnovni(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            var id = context.ActionArguments["id"] as int?;
            var zaposlen = context.ActionArguments["zaposlen"] as Zaposlen;

            if (id.HasValue && zaposlen != null && id != zaposlen.Id)
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

            //var existingZaposlen = db.Zaposleni.FirstOrDefault(x =>
            //    (x.Ime ?? "").ToLower() == (zaposlen.Ime ?? "").ToLower() &&
            //    (x.Prezime ?? "").ToLower() == (zaposlen.Prezime ?? "").ToLower() &&
            //    (x.Roditelj ?? "").ToLower() == (zaposlen.Roditelj ?? "").ToLower() &&
            //    (x.JMBG ?? "").ToLower() == (zaposlen.JMBG ?? "").ToLower());

            var existingZaposlen = db.Zaposleni.FirstOrDefault(x => x.Id != zaposlen.Id && (x.JMBG ?? "").ToLower() == (zaposlen.JMBG ?? "").ToLower());

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
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });
                return;
            }

            // Ako su sve provere prošle, pozivamo sledeću akciju u lancu
            await next();
        }
    }
}

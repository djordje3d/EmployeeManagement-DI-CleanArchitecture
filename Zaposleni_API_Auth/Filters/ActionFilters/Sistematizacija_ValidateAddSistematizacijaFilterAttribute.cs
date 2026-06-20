using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Zaposleni_Blazor.CoreBusiness;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    public class Sistematizacija_ValidateAddSistematizacijaFilterAttribute : ActionFilterAttribute
    {
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db;

        public Sistematizacija_ValidateAddSistematizacijaFilterAttribute(Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext db)
        {
            this.db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var sistematizacija = context.ActionArguments["sistematizacija"] as Sistematizacija;

            if (sistematizacija == null)
            {
                context.ModelState.AddModelError("Sistematizacija", "Sistematizacija object is NULL!");

                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };

                context.Result = new BadRequestObjectResult(problemDetails);

                return;
            }


            // Provera da li ista sistematizacija već postoji. ?? da bi se izbegla greška ako je neko polje NULL, postavlja se na prazan string

            var existingSistematizacija = db.Sistematizacije.FirstOrDefault(x =>
                (x.NazivRadnogMesta ?? "").ToLower() == (sistematizacija.NazivRadnogMesta ?? "").ToLower() &&
                (x.Koeficijent ?? 0) == (sistematizacija.Koeficijent ?? 0) &&
                (x.Radno_Iskustvo ?? "").ToLower() == (sistematizacija.Radno_Iskustvo ?? "").ToLower() &&
                (x.Beneficirani_Radni_Staz ?? 0) == (sistematizacija.Beneficirani_Radni_Staz ?? 0) &&
                (x.Bodovi ?? 0) == (sistematizacija.Bodovi ?? 0) &&
                (x.Opis ?? "").ToLower() == (sistematizacija.Opis ?? "").ToLower());


            if (existingSistematizacija != null) // Ako postoji zaposlen sa istim imenom, prezimenom, roditeljem i JMBG-om
            {
                context.ModelState.AddModelError("Sistematizacija", "Sistematizacija already EXISTS!");
            }

            // Provera KvalifikacijaId (strani ključ)
            if (sistematizacija.KvalifikacijaId == null || !db.Kvalifikacije.Any(m => m.Id == sistematizacija.KvalifikacijaId)) // Ako je KvalifikacijaId NULL ili ne postoji u bazi
            {
                context.ModelState.AddModelError("KvalifikacijaId", "Odabrana kvalifikacija ne postoji u bazi.");
            }

            // Provera KvalifikacijaId (strani ključ)
            if (sistematizacija.OrganizacioneJediniceId == null || !db.OrganizacioneJedinice.Any(m => m.Id == sistematizacija.OrganizacioneJediniceId)) // Ako je OrganizacioneJediniceId NULL ili ne postoji u bazi
            {
                context.ModelState.AddModelError("OrganizacioneJediniceId", "Odabrana OJ ne postoji u bazi.");
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

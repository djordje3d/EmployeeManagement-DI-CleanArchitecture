using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
//using Zaposleni_API_Auth.Models;
//using Zaposleni_API_Auth.Data;
using Microsoft.EntityFrameworkCore;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)] // Ovaj atribut se može koristiti samo na metodama. 
    public class Sistematizacija_ValidateUpdateSistematizacijaFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ApplicationDbContext db;

        public Sistematizacija_ValidateUpdateSistematizacijaFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var id = context.ActionArguments["id"] as int?;
            var sistematizacija = context.ActionArguments["sistematizacija"] as Sistematizacija;

            if (id.HasValue && sistematizacija != null && id != sistematizacija.Id)
            {
                context.ModelState.AddModelError("Id", "Id iz URL-a nije isti kao Id u modelu.");
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });
                return;
            }

            if (sistematizacija != null)
            {
                var postojiDuplikat = await db.Sistematizacije
                    .AsNoTracking() // Koristimo AsNoTracking da ne bi došlo do konflikta sa praćenjem entiteta
                    .AnyAsync(x => x.Id != sistematizacija.Id &&
                                   (x.NazivRadnogMesta ?? "").ToLower() == (sistematizacija.NazivRadnogMesta ?? "").ToLower() &&
                                   (x.Koeficijent ?? 0) == (sistematizacija.Koeficijent ?? 0) &&
                                   (x.Radno_Iskustvo ?? "").ToLower() == (sistematizacija.Radno_Iskustvo ?? "").ToLower() &&
                                   (x.Beneficirani_Radni_Staz ?? 0) == (sistematizacija.Beneficirani_Radni_Staz ?? 0) &&
                                   (x.Bodovi ?? 0) == (sistematizacija.Bodovi ?? 0) &&
                                   (x.Opis ?? "").ToLower() == (sistematizacija.Opis ?? "").ToLower());

                if (postojiDuplikat)
                {
                    context.ModelState.AddModelError("Sistematizacija", "Sistematizacija već postoji.");
                }

                var validnaKvalifikacija = await db.Kvalifikacije.AnyAsync(k => k.Id == sistematizacija.KvalifikacijaId);
                if (sistematizacija.KvalifikacijaId == null || !validnaKvalifikacija)
                {
                    context.ModelState.AddModelError("KvalifikacijaId", "Odabrana kvalifikacija ne postoji u bazi.");
                }

                var validnaOJ = await db.OrganizacioneJedinice.AnyAsync(o => o.Id == sistematizacija.OrganizacioneJediniceId);
                if (sistematizacija.OrganizacioneJediniceId == null || !validnaOJ)
                {
                    context.ModelState.AddModelError("OrganizacioneJediniceId", "Odabrana organizaciona jedinica ne postoji u bazi.");
                }
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                });
                return;
            }

            await next(); // Nastavlja ka sledećem middleware-u ili akciji
        }
    }
}

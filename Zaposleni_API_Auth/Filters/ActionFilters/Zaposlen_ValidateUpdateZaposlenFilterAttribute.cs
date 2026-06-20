using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Filters.ActionFilters;

[AttributeUsage(AttributeTargets.Method)]
public class Zaposlen_ValidateUpdateZaposlenFilterAttribute : Attribute, IAsyncActionFilter // IAsyncActionFilter → interfejs koji omogućava asinhrono izvršavanje akcije pre i posle izvršenja metode kontrolera.
{
    private readonly ApplicationDbContext db;

    public Zaposlen_ValidateUpdateZaposlenFilterAttribute(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) // OnActionExecutionAsync → metoda koja se izvršava pre i posle izvršenja akcije kontrolera.
                                                                                                           // ActionexecutionDelegate next → delegat koji se koristi za pozivanje sledećeg filtera ili akcije u lancu izvršavanja.
    {
        var id = context.ActionArguments["id"] as int?;
        var zaposlen = context.ActionArguments["zaposlen"] as Zaposlen;

        if (zaposlen is null)
        {
            context.ModelState.AddModelError("Zaposlen", "Zaposlen objekat je null.");
        }

        if (id.HasValue && zaposlen?.Id != id)
        {
            context.ModelState.AddModelError("Id", "Id iz URL-a nije isti kao Id u modelu.");
        }

        if (zaposlen is not null)
        {
            if (zaposlen.DatumRodjenja > DateTime.Today)
                context.ModelState.AddModelError("DatumRodjenja", "Datum rođenja ne može biti u budućnosti.");

            if (zaposlen.Pocetak_RadnogOd > zaposlen.Kraj_RadnogOd)
                context.ModelState.AddModelError("RadniOdnos", "Početak radnog odnosa ne može biti posle kraja.");

            if (zaposlen.MestoId == null || !await db.Mesta.AnyAsync(m => m.Id == zaposlen.MestoId))
                context.ModelState.AddModelError("MestoId", "Odabrano mesto ne postoji.");

            bool postojiDuplikat = await db.Zaposleni
                .AsNoTracking()
                .AnyAsync(x => x.Id != zaposlen.Id && (x.JMBG ?? "").ToLower() == (zaposlen.JMBG ?? "").ToLower());

            if (postojiDuplikat)
                context.ModelState.AddModelError("Zaposlen", "Zaposlen sa istim JMBG-om već postoji.");
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(context.ModelState)
            {
                Status = StatusCodes.Status400BadRequest
            });
            return;
        }

        await next(); // Nastavi na sledeći middleware ili akciju
    }
}

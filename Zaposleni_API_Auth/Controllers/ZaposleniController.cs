using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Zaposleni.Interfaces;

namespace Zaposleni_API_Auth.Controllers
{
    //[Authorize(Roles = "CanAccessZaposleni")]

    [ApiController]
    [Route("api/[controller]")]
    public class ZaposleniController : ControllerBase
    {
        private readonly ICrudZaposlenUseCases crudZaposlenUseCases;
        private readonly ILogger<ZaposleniController> _logger;


        public ZaposleniController(ICrudZaposlenUseCases crudZaposlenUseCases, ILogger<ZaposleniController> logger)
        {
            this.crudZaposlenUseCases = crudZaposlenUseCases;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetZaposleni()
        {
            var zaposleniLista = await crudZaposlenUseCases.GetAllAsync();

            // Vraćamo podatke koristeći Ok() koji automatski serijalizuje odgovor
            return Ok(zaposleniLista);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Zaposlen_ValidateZaposlenIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> GetZaposleniById(int id)
        {
            var zaposlen = await crudZaposlenUseCases.GetByIdAsync(id);

            if (zaposlen == null)
                return NotFound();

            return Ok(zaposlen);
        }

        [HttpPost]
        [TypeFilter(typeof(Zaposlen_ValidateAddZaposlenFilterAttribute))]
        [Authorize(Policy = "WritePolicy")] 
        public async Task<IActionResult> AddZaposleni([FromBody] Zaposlen zaposlen) // Body/form-data  testiranje u Postman
                                                                        //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Zaposlen objekat.
                                                                        // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            var success = await crudZaposlenUseCases.AddAsync(zaposlen); // Ova metoda će biti implementirana u UseCases i koristiće repository za dodavanje zaposlenog u bazu podataka.

            if (!success)
                return BadRequest("Greška prilikom dodavanja Zaposlenog.");

            return CreatedAtAction(nameof
                (GetZaposleniById), new { id = zaposlen.Id }, zaposlen); // So this is a convention of web API.
                                                                         // When we are working on a create endpoint, we should return a created status code with a location header and the object itself as a Json.

            //      CreatedAtAction vraća HTTP 201 Created status →  znači da je resurs uspešno kreiran.
            //      nameof(GetZaposleniById) → Kaže klijentu da može da koristi GetZaposleniById metodu da preuzme ovog zaposlenog.
            //      new { id = zaposlen.Id } → Prosleđuje ID novo - kreirane zaposlene.
            //      zaposlen → U telo odgovora vraća kreirani objekat.        }
        }


        [HttpPut("{id}")]
        [TypeFilter(typeof(Zaposlen_ValidateZaposlenIdFilterAttribute))]
        [TypeFilter(typeof(Zaposlen_ValidateUpdateZaposlenFilterAttributeOsnovni))]
        [TypeFilter(typeof(Zaposlen_HandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateZaposleni(int id, [FromBody] Zaposlen zaposlen)
        {
            var success = await crudZaposlenUseCases.UpdateAsync(zaposlen);

            if (!success)
                return NotFound();

            return Ok(zaposlen);
        }


        [HttpDelete("{id}")]
        [TypeFilter(typeof(Zaposlen_ValidateZaposlenIdFilterAttribute))]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> DeleteZaposleni(int id)
        {
            var success = await crudZaposlenUseCases.DeleteAsync(id);

            if (!success) // .Result ako je metoda async, a ako nije async onda samo success
            {
                _logger.LogError($"Zaposlen sa ID {id} nije pronađen ili nije mogao biti obrisan."); // Loguje grešku ako Zaposlen nije pronađen ili nije mogao biti obrisan.
                return NotFound();
            }

            return Ok(new { Message = "Zaposlen je uspešno obrisan." }); // Brisanje radi i bez ovoga u zagradi , ali je ovako bolje jer u postmanu dole pokaže osim potvrde 200 i zapis koji je obrisan
        }
    }
}
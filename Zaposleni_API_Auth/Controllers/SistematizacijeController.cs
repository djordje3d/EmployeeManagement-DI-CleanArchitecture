using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Sistematizacije.Interfaces;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SistematizacijeController : ControllerBase
    {
        private readonly ICrudSistematizacijaUseCases crudSistematizacijaUseCases;
        private readonly ILogger<SistematizacijeController> _logger; // Logger za logovanje grešaka i informacija. Ispisuje se u konzolu ili fajl, zavisno od konfiguracije aplikacije.

        public SistematizacijeController(ICrudSistematizacijaUseCases crudSistematizacijaUseCases, ILogger<SistematizacijeController> logger)
        {
            this.crudSistematizacijaUseCases = crudSistematizacijaUseCases;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetSistematizacije()
        {
            var sistematizacijeLista = await crudSistematizacijaUseCases.GetAllAsync();

            return Ok(sistematizacijeLista);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Sistematizacija_ValidateSistematizacijaIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")]
        public IActionResult GetSistematizacijaById(int id)
        {
            var sistematizacija = crudSistematizacijaUseCases.GetByIdAsync(id);

            if (sistematizacija == null)
                return NotFound();

            return Ok(sistematizacija);
        }

        [HttpPost]
        [TypeFilter(typeof(Sistematizacija_ValidateAddSistematizacijaFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> AddSistematizacija([FromBody] Sistematizacija sistematizacija) // Body/form-data  testiranje u Postman
                                                                                                        //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Sistematizacija objekat.
                                                                                                        // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            var success = await crudSistematizacijaUseCases.AddAsync(sistematizacija);

            if (!success)
                return BadRequest("Greška prilikom dodavanja Sistematizacije.");

            return CreatedAtAction(nameof
                (GetSistematizacijaById), new { id = sistematizacija.Id }, sistematizacija); // So this is a convention of web API.
                                                                                             // When we are working on a create endpoint, we should return a created status code with a location header and the object itself as a Json.

            //      CreatedAtAction vraća HTTP 201 Created status →  znači da je resurs uspešno kreiran.
            //      nameof(GetSistematizacijaById) → Kaže klijentu da može da koristi GetSistematizacijaById metodu da preuzme ovu sistematizaciju.
            //      new { id = zaposlen.Id } → Prosleđuje ID novo - kreirane zaposlene.
            //      sistematizacija → U telo odgovora vraća kreirani objekat.        }
        }


        [HttpPut("{id}")]
        [TypeFilter(typeof(Sistematizacija_ValidateSistematizacijaIdFilterAttribute))]
        [TypeFilter(typeof(Sistematizacija_ValidateUpdateSistematizacijaFilterAttribute))]
        [TypeFilter(typeof(Sistematizacija_HandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateSistematizacija(int id, [FromBody] Sistematizacija sistematizacija)
        {
            var success = await crudSistematizacijaUseCases.UpdateAsync(sistematizacija);

            if (!success)
                return NotFound();

            return Ok(sistematizacija);
        }


        [HttpDelete("{id}")]
        [TypeFilter(typeof(Sistematizacija_ValidateSistematizacijaIdFilterAttribute))]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> DeleteSistematizacija(int id)
        {
            // Ne mora da se brine da li je null jer ta provera postoji u Sistematizacija_ValidateSistematizacijaIdFilterAttribute 
            var success = await crudSistematizacijaUseCases.DeleteAsync(id); // Ova metoda vraća Task<bool> koji pokazuje da li je brisanje uspešno ili ne.

            if (!success) // .Result jer je DeleteAsync asinhrona metoda
            {
                _logger.LogError($"Sistematizacija sa ID {id} nije pronađena ili nije mogla biti obrisana."); // Loguje grešku ako sistematizacija nije pronađena ili nije mogla biti obrisana.
                return NotFound();
            }

            return Ok(new { Message = "Sistematizacija uspešno obrisana." }); // // Vraća poruku o uspešnom brisanju sistematizacije.
        }

    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[JwtTokenAuthFilter] // Ovaj filter će se koristiti za autorizaciju
    public class KvalifikacijeController : ControllerBase
    {
        private readonly IAddKvalifikacijaUseCase addKvalifikacijaUseCase;
        private readonly IKvalifikacijaByIdUseCase kvalifikacijaByIdUseCase;
        private readonly IEditKvalifikacijaUseCase editKvalifikacijaUseCase;
        private readonly IDeleteKvalifikacijaUseCase deleteKvalifikacijaUseCase;
        private readonly IListKvalifikacijaUseCase listKvalifikacijaUseCase;
        private readonly ILogger<KvalifikacijeController> _logger; // ILogger se koristi za logovanje informacija, upozorenja i grešaka u aplikaciji. U ovom slučaju, koristi se za logovanje grešaka prilikom brisanja Kvalifikacije.

        public KvalifikacijeController(
            IAddKvalifikacijaUseCase addKvalifikacijaUseCase,
            IKvalifikacijaByIdUseCase kvalifikacijaByIdUseCase,
            IEditKvalifikacijaUseCase editKvalifikacijaUseCase,
            IDeleteKvalifikacijaUseCase deleteKvalifikacijaUseCase,
            IListKvalifikacijaUseCase listKvalifikacijaUseCase,
            ILogger<KvalifikacijeController> logger)
        {
            this.addKvalifikacijaUseCase = addKvalifikacijaUseCase;
            this.kvalifikacijaByIdUseCase = kvalifikacijaByIdUseCase;
            this.editKvalifikacijaUseCase = editKvalifikacijaUseCase;
            this.deleteKvalifikacijaUseCase = deleteKvalifikacijaUseCase;
            this.listKvalifikacijaUseCase = listKvalifikacijaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetKvalifikacije()
        {
            var kvalifikacijeLista = await listKvalifikacijaUseCase.ExecuteAsync();

            // Vraćamo podatke koristeći Ok() koji automatski serijalizuje odgovor
            return Ok(kvalifikacijeLista);
        }


        [HttpGet("{id}")]
        [TypeFilter(typeof(Kvalifikacija_ValidateKvalifikacijaIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetKvalifikacijaById(int id)
        {
            var kvalifikacija = await kvalifikacijaByIdUseCase.ExecuteAsync(id);

            return Ok(kvalifikacija);
        }

        [HttpPost]
        [TypeFilter(typeof(Kvalifikacija_ValidateAddKvalifikacijaFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> AddKvalifikacija([FromBody] Kvalifikacija kvalifikacija) // Body/form-data  testiranje u Postman
                                                                                      //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Kvalifikacija objekat.
                                                                                      // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            var success = await addKvalifikacijaUseCase.ExecuteAsync(kvalifikacija);
            if (!success)
            {
                return BadRequest("Kvalifikacija nije uspešno dodata.");
            }

            return CreatedAtAction(nameof
                (GetKvalifikacijaById), new { id = kvalifikacija.Id }, kvalifikacija);
        }


        [HttpPut("{id}")]
        [TypeFilter(typeof(Kvalifikacija_ValidateKvalifikacijaIdFilterAttribute))]
        [TypeFilter(typeof(Kvalifikacija_ValidateUpdateKvalifikacijaFilterAttribute))]
        [TypeFilter(typeof(Kvalifikacija_HandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateKvalifikacija(int id, [FromBody] Kvalifikacija kvalifikacija)
        {
            var success = await editKvalifikacijaUseCase.ExecuteAsync(kvalifikacija); 
            if (!success)
            {
                return NotFound("Kvalifikacija nije pronađena ili nije uspešno ažurirana.");
            }

            return Ok(kvalifikacija);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Kvalifikacija_ValidateKvalifikacijaIdFilterAttribute))]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> DeleteKvalifikacije(int id)
        {
            var success = await deleteKvalifikacijaUseCase.ExecuteAsync(id); // Ova metoda će biti implementirana u UseCases i koristiće repository za brisanje Kvalifikacije iz baze podataka.

            if (!success) // .Result ako je metoda async, a ako nije async onda samo success
            {
                _logger.LogError($"Kvalifikacija sa ID {id} nije pronađena ili nije mogla biti obrisana."); // Loguje grešku ako Kvalifikacija nije pronađena ili nije mogla biti obrisana.
                return NotFound();
            }

            return Ok(new { Message = "Kvalifikacija uspešno obrisana." }); // Brisanje radi i bez ovoga u zagradi , ali je ovako bolje jer u postmanu dole pokaže osim potvrde 200 i zapis koji je obrisan
        }
    }
}

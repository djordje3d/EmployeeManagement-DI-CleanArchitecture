using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.ClanDomacinstva.Interfaces;


namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[JwtTokenAuthFilter] // Ovaj filter će se koristiti za autorizaciju

    public class ClanoviDomacinstvaController : ControllerBase
    {
        private readonly ICrudClanoviDomacinstvaUseCases crudClanoviDomacinstvaUseCases;
        private readonly ILogger<ClanoviDomacinstvaController> _logger;

        public ClanoviDomacinstvaController(ICrudClanoviDomacinstvaUseCases crudClanoviDomacinstvaUseCases, ILogger<ClanoviDomacinstvaController> logger)
        {
            this.crudClanoviDomacinstvaUseCases = crudClanoviDomacinstvaUseCases;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetMesta()
        {
            var clanoviDomacinstvaLista = await crudClanoviDomacinstvaUseCases.GetAllAsync();

            return Ok(clanoviDomacinstvaLista);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(ClanoviDomacinstva_ValidateClanoviIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetClanoviById(int id)
        {
            var clanDomacinstva = await crudClanoviDomacinstvaUseCases.GetByIdAsync(id);

            if (clanDomacinstva == null)
                return NotFound();

            return Ok(clanDomacinstva);
        }

        [HttpPost]
        [TypeFilter(typeof(ClanoviDomacinstva_ValidateAddClanFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> AddClanDom([FromBody] ClanoviDomacinstva clanDom) // Body/form-data  testiranje u Postman
                                                                               //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Zaposlen objekat.
                                                                               // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            var success = await crudClanoviDomacinstvaUseCases.AddAsync(clanDom);

            if (!success)
                return BadRequest("Greška prilikom dodavanja člana domaćinstva.");

            return CreatedAtAction(nameof
                (GetClanoviById), new { id = clanDom.Id }, clanDom);
        }


        [HttpPut("{id}")]
        [TypeFilter(typeof(ClanoviDomacinstva_ValidateClanoviIdFilterAttribute))]
        [TypeFilter(typeof(ClanDom_ValidateUpdateClanDomFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [TypeFilter(typeof(ClanoviDomacinstvaHandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateClanDom(int id, [FromBody] ClanoviDomacinstva clanDom)
        {
            var success = await crudClanoviDomacinstvaUseCases.UpdateAsync(clanDom);

            if (!success)
                return NotFound();

            return Ok(clanDom);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(ClanoviDomacinstva_ValidateClanoviIdFilterAttribute))]
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> DeleteClanDom(int id)
        {
            var success = await crudClanoviDomacinstvaUseCases.DeleteAsync(id);

            if (!success) // .Result ako je metoda async, a ako nije async onda samo success
            {
                _logger.LogError($"Član domaćinstva sa ID {id} nije pronađen ili nije mogao biti obrisan."); // Loguje grešku ako sistematizacija nije pronađena ili nije mogla biti obrisana.
                return NotFound();
            }

            return Ok(new { Message = "Član domaćinstva uspešno obrisan." }); // Brisanje radi i bez ovoga  u zagradi , ali je ovako bolje jer u postmanu dole pokaže osim potvrde 200 i zapis koji je obrisan
        }
    }
}

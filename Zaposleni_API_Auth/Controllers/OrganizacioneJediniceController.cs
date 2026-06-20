using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica.Interfaces;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    /*[JwtTokenAuthFilter] */// Ovaj filter će se koristiti za autorizaciju

    public class OrganizacioneJediniceController : ControllerBase
    {
        private readonly ICrudOrganizacionaJedinicaUseCases crudOrganizacionaJedinicaUseCases;
        private readonly ILogger<OrganizacioneJediniceController> _logger;

        public OrganizacioneJediniceController(ICrudOrganizacionaJedinicaUseCases crudOrganizacionaJedinicaUseCases, ILogger<OrganizacioneJediniceController> logger)
        {
            this.crudOrganizacionaJedinicaUseCases = crudOrganizacionaJedinicaUseCases;
            _logger = logger;
        }

        [HttpGet]
        //[RequiredClaim("read", "true")]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")

        public async Task<IActionResult> GetOrgJed()
        {
            var orgJedLista = await crudOrganizacionaJedinicaUseCases.GetAllAsync();

            return Ok(orgJedLista);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(OJ_ValidateOJ_IdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> GetOrgJedById(int id)

        {
            //var orgJed1 = HttpContext.Items["orgjed"];
            var orgJed = await crudOrganizacionaJedinicaUseCases.GetByIdAsync(id);

            if (orgJed == null)
                return NotFound();

            return Ok(orgJed);
        }

        [HttpPost]
        [TypeFilter(typeof(OJ_ValidateAdd_OJFilterAttribute))]
        //[Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> AddOrgJed([FromBody] OrganizacioneJedinice orgJed) // Body/form-data  testiranje u Postman
                                                                                //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Zaposlen objekat.
                                                                                // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            var success = await crudOrganizacionaJedinicaUseCases.AddAsync(orgJed);

            if (!success)
                return BadRequest("Greška prilikom dodavanja OJ.");

            return CreatedAtAction(nameof
                (GetOrgJedById), new { id = orgJed.Id }, orgJed);
        }


        [HttpPut("{id}")]
        [TypeFilter(typeof(OJ_ValidateOJ_IdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        //[TypeFilter(typeof(OJ_ValidateUpdateOJFilterAttributeOsnovni))] // Moralo da se isključi
        [TypeFilter(typeof(OJ_ValidateUpdateOJFilterAttribute))] 
        [TypeFilter(typeof(OJ_HandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateOrgJed(int id, [FromBody] OrganizacioneJedinice orgJed)
        {
            var success = await crudOrganizacionaJedinicaUseCases.UpdateAsync(orgJed);

            if (!success)
                return NotFound();

            return Ok(orgJed);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(OJ_ValidateOJ_IdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> DeleteOrgJed(int id)
        {
            var success = await crudOrganizacionaJedinicaUseCases.DeleteAsync(id);

            if (!success) // .Result ako je metoda async, a ako nije async onda samo success
            {
                _logger.LogError($"OJ sa ID {id} nije pronađena ili nije mogla biti obrisana."); // Loguje grešku ako OJ nije pronađena ili nije mogla biti obrisana.
                return NotFound();
            }

            return Ok(new { Message = "OJ uspešno obrisana." }); // Brisanje radi i bez ovoga u zagradi , ali je ovako bolje jer u postmanu dole pokaže osim potvrde 200 i zapis koji je obrisan
        }
    }
}

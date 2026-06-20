using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;

namespace Zaposleni_API_Auth.Controllers
{
    /*[Authorize(Roles = "CanAccessMesta")]*/ // Ova klasa zahteva da korisnik ima ulogu "CanAccessMesta" da bi mogao da pristupi ovim akcijama
                                          // Primena JWT autorizacije - zahteva validan token za pristup
    [ApiController]
    [Route("api/[controller]")]
    public class MestaController : ControllerBase
    {
        private readonly IAddMestoUseCase addMestoUseCase;
        private readonly IMestoByIdUseCase mestoByIdUseCase;
        private readonly IEditMestoUseCase editMestoUseCase;
        private readonly IDeleteMestoUseCase deleteMestoUseCase;
        private readonly IListMestoUseCase listMestoUseCase;
        private readonly ILogger<ClanoviDomacinstvaController> _logger;

        public MestaController(IAddMestoUseCase addMestoUseCase,
            IMestoByIdUseCase mestoByIdUseCase,
            IEditMestoUseCase editMestoUseCase,
            IDeleteMestoUseCase deleteMestoUseCase,
            IListMestoUseCase listMestoUseCase,
            ILogger<ClanoviDomacinstvaController> logger)
        {
            this.addMestoUseCase = addMestoUseCase;
            this.mestoByIdUseCase = mestoByIdUseCase;
            this.editMestoUseCase = editMestoUseCase;
            this.deleteMestoUseCase = deleteMestoUseCase;
            this.listMestoUseCase = listMestoUseCase;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetMesta()
        {
            var mestaLista = await listMestoUseCase.ExecuteAsync();
            return Ok(mestaLista);
        }

        // Endpoint za čitanje jednog mesta po ID-u
        [HttpGet("{id}")]
        [TypeFilter(typeof(Mesto_ValidateMestoIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> GetMestoById(int id)
        {
            var mesto = await mestoByIdUseCase.ExecuteAsync(id);

            return Ok(mesto);
        }

        // Endpoint za dodavanje novog mesta
        [HttpPost]
        [TypeFilter(typeof(Mesto_ValidateAddMestoFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> AddMesto([FromBody] Mesto mesto) // Body/form-data  testiranje u Postman
                                                                                      //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Zaposlen objekat.
                                                                                      // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            // Dodavanje novog mesta
            var success = await addMestoUseCase.ExecuteAsync(mesto);
            
            if (!success)
                return BadRequest("Greška prilikom dodavanja mesta.");

            return CreatedAtAction(nameof
                (GetMestoById), new { id = mesto.Id }, mesto);
        }


        [HttpPut("{id}")]
        [TypeFilter(typeof(Mesto_ValidateMestoIdFilterAttribute))]
        [TypeFilter(typeof(Mesto_ValidateUpdateMestoFilterAttribute))]
        [TypeFilter(typeof(Mesto_HandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateMesto(int id, [FromBody] Mesto mesto)
        {
            var success = await editMestoUseCase.ExecuteAsync(mesto);
            if (!success)
                return NotFound();

            return Ok(mesto);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Mesto_ValidateMestoIdFilterAttribute))]
        [Authorize(Policy = "DeletePolicy")] // Korisnici moraju imati dozvolu za brisanje (claim "delete")
        public async Task<IActionResult> DeleteMesto(int id)
        {
            var success = await deleteMestoUseCase.ExecuteAsync(id); // Ova metoda će biti implementirana u UseCases i koristiće repository za brisanje grupe mesta troškova iz baze podataka.

            if (!success) // .Result ako je metoda async, a ako nije async onda samo success
            {
                _logger.LogError($"Grupa mesta troška sa ID {id} nije pronađena ili nije mogla biti obrisana."); // Loguje grešku ako grupa MT nije pronađena ili nije mogla biti obrisana.
                return NotFound();
            }

            return Ok(new { Message = "Mesto uspešno obrisano." }); // Brisanje radi i bez ovoga  u zagradi , ali je ovako bolje jer u postmanu dole pokaže osim potvrde 200 i zapis koji je obrisan
        }
    }
}

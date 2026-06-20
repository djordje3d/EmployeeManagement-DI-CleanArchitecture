using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
//using Zaposleni_API_Auth_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[JwtTokenAuthFilter] // Ovaj filter će se koristiti za autorizaciju
    public class GrupaMestaTroskovaController : ControllerBase
    {
        private readonly IAddGrupaMestaTroskaUseCase addGrupaMestaTroskaUseCase;
        private readonly IGrupaMestaTroskaByIdUseCase grupaMestaTroskaByIdUseCase;
        private readonly IEditGrupaMestaTroskaUseCase editGrupaMestaTroskaUseCase;
        private readonly IDeleteGrupaMestaTroskaUseCase deleteGrupaMestaTroskaUseCase;
        private readonly IListGrupaMestaTroskaUseCase listGrupaMestaTroskaUseCase;
        private readonly ILogger<ClanoviDomacinstvaController> _logger; // ILogger se koristi za logovanje informacija, upozorenja i grešaka u aplikaciji. U ovom slučaju, koristi se za logovanje grešaka prilikom brisanja grupe mesta troškova.

        public GrupaMestaTroskovaController(
            IAddGrupaMestaTroskaUseCase addGrupaMestaTroskaUseCase,
            IGrupaMestaTroskaByIdUseCase grupaMestaTroskaByIdUseCase,
            IEditGrupaMestaTroskaUseCase editGrupaMestaTroskaUseCase,
            IDeleteGrupaMestaTroskaUseCase deleteGrupaMestaTroskaUseCase,
            IListGrupaMestaTroskaUseCase listGrupaMestaTroskaUseCase,
            ILogger<ClanoviDomacinstvaController> logger)
        {
            this.addGrupaMestaTroskaUseCase = addGrupaMestaTroskaUseCase;
            this.grupaMestaTroskaByIdUseCase = grupaMestaTroskaByIdUseCase;
            this.editGrupaMestaTroskaUseCase = editGrupaMestaTroskaUseCase;
            this.deleteGrupaMestaTroskaUseCase = deleteGrupaMestaTroskaUseCase;
            this.listGrupaMestaTroskaUseCase = listGrupaMestaTroskaUseCase;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "ReadPolicy")] // Korisnici moraju imati dozvolu za čitanje (claim "read")
        public async Task<IActionResult> GetGrMestaTroska()
        {
            var grupaMestatroskaLista = await listGrupaMestaTroskaUseCase.ExecuteAsync();
            return Ok(grupaMestatroskaLista);
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(GrMestaTroska_ValidateGrMestaTroskaIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "ReadPolicy")]
        public async Task<IActionResult> GetGrMestaTroskaById(int id)
        {
            var grupaMT = await grupaMestaTroskaByIdUseCase.ExecuteAsync(id); 

            return Ok(grupaMT);
        }

        [HttpPost]
        [TypeFilter(typeof(GrMestaTroska_ValidateAdd_GrMTFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> AddGrMestaTroska([FromBody] GrupaMestaTroskova grupaMestaTroskova) // Body/form-data  testiranje u Postman
                                                                                                            //[FromBody] → ASP.NET Core deserijalizuje JSON telo u Zaposlen objekat.
                                                                                                            // Ovo je standardni način pravljenja RESTful API endpointa za kreiranje resursa u ASP.NET Core.
        {
            var success = await addGrupaMestaTroskaUseCase.ExecuteAsync(grupaMestaTroskova); // Ova metoda će biti implementirana u UseCases i koristiće repository za dodavanje grupe mesta troškova u bazu podataka.

            if (!success)
                return BadRequest("Greška prilikom dodavanja grupe mesta troška.");

            return CreatedAtAction(nameof
                (GetGrMestaTroskaById), new { id = grupaMestaTroskova.Id }, grupaMestaTroskova);
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(GrMestaTroska_ValidateGrMestaTroskaIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [TypeFilter(typeof(GrMestaTroska_ValidateUpdateGrMTFilterAttribute))]
        [TypeFilter(typeof(GrMestaTroska_HandleUpdateExceptionsFilterAttribute))]
        [Authorize(Policy = "WritePolicy")]
        public async Task<IActionResult> UpdateGrupaMT(int id, [FromBody] GrupaMestaTroskova grupaMestaTroskova)
        {
            var success = await editGrupaMestaTroskaUseCase.ExecuteAsync(grupaMestaTroskova);

            if (!success)
                return NotFound();

            return Ok(grupaMestaTroskova);
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(GrMestaTroska_ValidateGrMestaTroskaIdFilterAttribute))] // Menja se u typeof jer se koristi DI. Kada se INSTANCIRA Akcioni filter on će korisiti aktuelni DI.
        [Authorize(Policy = "DeletePolicy")]
        public async Task<IActionResult> DeleteGrMestaTroska(int id)
        {
            var success = await deleteGrupaMestaTroskaUseCase.ExecuteAsync(id); // Ova metoda će biti implementirana u UseCases i koristiće repository za brisanje grupe mesta troškova iz baze podataka.

            if (!success) // .Result ako je metoda async, a ako nije async onda samo success
            {
                _logger.LogError($"Grupa mesta troška sa ID {id} nije pronađena ili nije mogla biti obrisana."); // Loguje grešku ako grupa MT nije pronađena ili nije mogla biti obrisana.
                return NotFound();
            }

            return Ok(new { Message = "Grupa MT uspešno obrisana." }); // Brisanje radi i bez ovoga u zagradi , ali je ovako bolje jer u postmanu dole pokaže osim potvrde 200 i zapis koji je obrisan
        }
    }
}

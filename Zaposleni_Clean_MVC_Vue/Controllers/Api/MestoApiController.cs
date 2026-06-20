using Microsoft.AspNetCore.Mvc;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;

namespace Zaposleni_Clean_MVC_Vue.Controllers.Api
{
    [Route("api/mesta")]
    [ApiController]
    public class MestoApiController : ControllerBase
    {
        private readonly IListMestoUseCase listMestoUseCase;
        private readonly IAddMestoUseCase addMestoUseCase;
        private readonly IEditMestoUseCase editMestoUseCase;
        private readonly IDeleteMestoUseCase deleteMestoUseCase;
        private readonly IMestoByIdUseCase mestoByIdUse;

        public MestoApiController(
            IListMestoUseCase listMestoUseCase, 
            IAddMestoUseCase addMestoUseCase, 
            IEditMestoUseCase editMestoUseCase, 
            IDeleteMestoUseCase deleteMestoUseCase, 
            IMestoByIdUseCase mestoByIdUse
            )
        {
            this.listMestoUseCase = listMestoUseCase;
            this.addMestoUseCase = addMestoUseCase;
            this.editMestoUseCase = editMestoUseCase;
            this.deleteMestoUseCase = deleteMestoUseCase;
            this.mestoByIdUse = mestoByIdUse;
        }

        [HttpGet]
        public async Task<IActionResult> GetMesta()
        {
            var mesta = await listMestoUseCase.ExecuteAsync();
            return new JsonResult(mesta);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Mesto mesto)
        {
            var success = await addMestoUseCase.ExecuteAsync(mesto);
            return Ok(success);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Mesto mesto)
        {
            if (id != mesto.Id)
                return BadRequest("ID nije validan");

            var success = await editMestoUseCase.ExecuteAsync(mesto);

            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await deleteMestoUseCase.ExecuteAsync(id);

            if (!success)
                return NotFound();

            return Ok();
        }
    }
}

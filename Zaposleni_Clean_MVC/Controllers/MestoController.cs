using Microsoft.AspNetCore.Mvc;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;

namespace Zaposleni_Clean_MVC.Controllers
{
    public class MestoController : Controller
    {
        private readonly IListMestoUseCase listMestoUseCase;
        private readonly IAddMestoUseCase addMestoUseCase;
        private readonly IEditMestoUseCase editMestoUseCase;
        private readonly IDeleteMestoUseCase deleteMestoUseCase;
        private readonly IMestoByIdUseCase mestoByIdUse;

        public MestoController(
            IListMestoUseCase listMestoUseCase, 
            IAddMestoUseCase addMestoUseCase,
            IEditMestoUseCase editMestoUseCase,
            IDeleteMestoUseCase deleteMestoUseCase, IMestoByIdUseCase mestoByIdUse)
        {
            this.listMestoUseCase = listMestoUseCase;
            this.addMestoUseCase = addMestoUseCase;
            this.editMestoUseCase = editMestoUseCase;
            this.deleteMestoUseCase = deleteMestoUseCase;
            this.mestoByIdUse = mestoByIdUse;
        }

        public async Task<IActionResult> Index()
        {
            var mesta = await listMestoUseCase.ExecuteAsync();
            return View(mesta);
        }

        public IActionResult AddMesto()
        {

            return View(new Mesto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMesto(Mesto mesto)
        {
            if (ModelState.IsValid)
            {
                var success = await addMestoUseCase.ExecuteAsync(mesto);

                if (success)
                {
                    TempData["SuccessMessage"] = "Mesto uspešno dodato!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja mesta.");
                }
            }

            // Ako dođe do greške, vraća se ista forma sa trenutnim podacima
            return View(mesto);
        }

        public async Task<IActionResult> UpdateMesto(int mestoId)
        {
            try
            {
                var mesto = await mestoByIdUse.ExecuteAsync(mestoId);

                if (mesto != null)
                {
                    return View(mesto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = $"Greška prilikom Update zaposlenog: {ex.Message}";
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMesto(Mesto mesto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool uspesno = await editMestoUseCase.ExecuteAsync(mesto);

                    if (!uspesno)
                    {
                        TempData["ErrorMessage"] = "Mesto nije pronađeno.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["SuccessMessage"] = "Mesto je uspešno izmenjeno.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom izmene mesta: {ex.Message} ";
                }
            }

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(mesto);
        }

        public async Task<IActionResult> DeleteMesto(int mestoId)
        {
            try
            {
                bool rezultat = await deleteMestoUseCase.ExecuteAsync(mestoId);

                if (rezultat)
                {
                    TempData["SuccessMessage"] = "Mesto je uspešno obrisano.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Greška prilikom brisanja mesta.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

    }

}

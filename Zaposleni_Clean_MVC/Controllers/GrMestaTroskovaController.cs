using Microsoft.AspNetCore.Mvc;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;

namespace Zaposleni_Clean_MVC.Controllers
{
    public class GrMestaTroskovaController : Controller
    {
        private readonly IListGrupaMestaTroskaUseCase listGrupaMestaTroskovaUseCase;
        private readonly IAddGrupaMestaTroskaUseCase addGrupaMestaTroskaUseCase;
        private readonly IEditGrupaMestaTroskaUseCase editGrupaMestaTroskaUseCase;
        private readonly IDeleteGrupaMestaTroskaUseCase deleteGrupaMestaTroskaUseCase;
        private readonly IGrupaMestaTroskaByIdUseCase grupaMestaTroskaByIdUseCase;

        public GrMestaTroskovaController(
            IListGrupaMestaTroskaUseCase listGrupaMestaTroskovaUseCase,
            IAddGrupaMestaTroskaUseCase addGrupaMestaTroskaUseCase,
            IEditGrupaMestaTroskaUseCase editGrupaMestaTroskaUseCase,
            IDeleteGrupaMestaTroskaUseCase deleteGrupaMestaTroskaUseCase,
            IGrupaMestaTroskaByIdUseCase grupaMestaTroskaByIdUseCase)
        {
            this.listGrupaMestaTroskovaUseCase = listGrupaMestaTroskovaUseCase;
            this.addGrupaMestaTroskaUseCase = addGrupaMestaTroskaUseCase;
            this.editGrupaMestaTroskaUseCase = editGrupaMestaTroskaUseCase;
            this.deleteGrupaMestaTroskaUseCase = deleteGrupaMestaTroskaUseCase;
            this.grupaMestaTroskaByIdUseCase = grupaMestaTroskaByIdUseCase;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            try
            {
                var grupeMestaTroskova = await listGrupaMestaTroskovaUseCase.ExecuteAsync();

                // Dinamičko sortiranje
                grupeMestaTroskova = sortOrder switch
                {
                    "id" => grupeMestaTroskova.OrderBy(k => k.Id).ToList(),
                    "id_desc" => grupeMestaTroskova.OrderByDescending(k => k.Id).ToList(),
                    "Grupa" => grupeMestaTroskova.OrderBy(k => k.Grupa).ToList(),
                    "Grupa_desc" => grupeMestaTroskova.OrderByDescending(k => k.Grupa).ToList(),
                    "Naziv" => grupeMestaTroskova.OrderBy(k => k.Naziv).ToList(),
                    "Naziv_desc" => grupeMestaTroskova.OrderByDescending(k => k.Naziv).ToList(),
                    _ => grupeMestaTroskova.OrderBy(k => k.Id).ToList() // default
                };

                ViewData["CurrentSort"] = sortOrder;

                // Prosleđivanje sortirane liste modelu View-a
                return View(grupeMestaTroskova);
            }
            catch (Exception ex)
            {
                // Logovanje greške
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return View(new List<GrupaMestaTroskova>()); // Vraćanje prazne liste u slučaju greške
            }
        }


        public IActionResult AddGrupaMestaTroskova()
        {

            return View(new GrupaMestaTroskova());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGrupaMestaTroskova(GrupaMestaTroskova grupaMestaTroskova)
        {
            if (ModelState.IsValid)
            {
                var success = await addGrupaMestaTroskaUseCase.ExecuteAsync(grupaMestaTroskova);
                if (success)
                {
                    TempData["SuccessMessage"] = "Grupa Mesta Troškova uspešno dodata!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja Grupe Mesta Troškova.");
                }
            }
            // Ako dođe do greške, vraća se ista forma sa trenutnim podacima
            return View(grupaMestaTroskova);
        }

        public async Task<IActionResult> UpdateGrupaMestaTroskova(int grMestaTroskovaId)
        {
            try
            {
                // Pozivanje API-ja za dobijanje kvalifikacije po ID-ju
                //var grMestaTroskovaApi = await webApiExecuter.InvokeGet<GrupaMestaTroskova>($"grupamestatroskova/{grMestaTroskovaId}");
                var grMestaTroskova = await grupaMestaTroskaByIdUseCase.ExecuteAsync(grMestaTroskovaId);

                if (grMestaTroskova == null)
                {
                    TempData["ErrorMessage"] = "GrupaMestaTroskova nije pronađena.";
                    return RedirectToAction(nameof(Index));
                }

                return View(grMestaTroskova);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom Update sistematizacije: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGrupaMestaTroskova(GrupaMestaTroskova grMestaTroskova)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool uspesno = await editGrupaMestaTroskaUseCase.ExecuteAsync(grMestaTroskova);

                    if (!uspesno)
                    {
                        TempData["ErrorMessage"] = "GrupaMestaTroskova nije pronađena.";
                        return RedirectToAction(nameof(Index));
                    }

                    // Ako je ažuriranje uspešno, dodajemo poruku u TempData
                    // koja će biti prikazana na Index stranici

                    TempData["SuccessMessage"] = "Kvalifikacija uspešno ažurirana!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom izmene kvalifikacije: {ex.Message}";
                }
            }

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(grMestaTroskova);
        }

        public async Task<IActionResult> DeleteGrupaMestaTroskova(int grMestaTroskovaId)
        {
            try
            {
                //await webApiExecuter.InvokeDelete($"grupamestatroskova/{grMestaTroskovaId}");
                bool rezultat = await deleteGrupaMestaTroskaUseCase.ExecuteAsync(grMestaTroskovaId);
                // Na osnovu rezultata postavljamo odgovarajuću poruku
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

            // Redirektujemo korisnika nazad na Index stranicu
            return RedirectToAction(nameof(Index));

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;

namespace Zaposleni_Clean_MVC_Vue.Controllers
{
    public class KvalifikacijeController : Controller
    {
        private readonly IListKvalifikacijaUseCase listKvalifikacijaUseCase;
        private readonly IAddKvalifikacijaUseCase addKvalifikacijaUseCase;
        private readonly IEditKvalifikacijaUseCase editKvalifikacijaUseCase;
        private readonly IDeleteKvalifikacijaUseCase deleteKvalifikacijaUseCase;
        private readonly IKvalifikacijaByIdUseCase kvalifikacijaByIdUseCase;

        public KvalifikacijeController(
           IListKvalifikacijaUseCase listKvalifikacijaUseCase,
           IAddKvalifikacijaUseCase addKvalifikacijaUseCase,
           IEditKvalifikacijaUseCase editKvalifikacijaUseCase,
           IDeleteKvalifikacijaUseCase deleteKvalifikacijaUseCase,
           IKvalifikacijaByIdUseCase kvalifikacijaByIdUseCase)
        {
            this.listKvalifikacijaUseCase = listKvalifikacijaUseCase;
            this.addKvalifikacijaUseCase = addKvalifikacijaUseCase;
            this.editKvalifikacijaUseCase = editKvalifikacijaUseCase;
            this.deleteKvalifikacijaUseCase = deleteKvalifikacijaUseCase;
            this.kvalifikacijaByIdUseCase = kvalifikacijaByIdUseCase;
        }
        public async Task<IActionResult> Index(string sortOrder)
        {
            try
            {
                // Pozivanje API-ja za dobijanje liste Kvalifikacija
                //var kvalifikacijeApi = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");
                var kvalifikacije = await listKvalifikacijaUseCase.ExecuteAsync();

                // Dinamičko sortiranje
                kvalifikacije = sortOrder switch
                {
                    "id_desc" => kvalifikacije.OrderByDescending(k => k.Id).ToList(),
                    "LicniStepenKv" => kvalifikacije.OrderBy(k => k.LicniStepenKv).ToList(),
                    "LicniStepenKv_desc" => kvalifikacije.OrderByDescending(k => k.LicniStepenKv).ToList(),
                    "Naziv" => kvalifikacije.OrderBy(k => k.Naziv).ToList(),
                    "Naziv_desc" => kvalifikacije.OrderByDescending(k => k.Naziv).ToList(),
                    _ => kvalifikacije.OrderBy(k => k.Id).ToList() // Podrazumevano sortiranje
                };

                ViewData["CurrentSort"] = sortOrder;

                // Prosleđivanje sortirane liste modelu View-a
                return View(kvalifikacije);
            }
            catch (Exception ex)
            {
                // Logovanje greške
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return View(new List<Kvalifikacija>()); // Vraćanje prazne liste u slučaju greške
            }
        }


        public IActionResult AddKvalifikacija()
        {

            return View(new Kvalifikacija());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddKvalifikacija(Kvalifikacija kvalifikacija)
        {
            if (ModelState.IsValid)
            {
                var success = await addKvalifikacijaUseCase.ExecuteAsync(kvalifikacija);
                if (success)
                {
                    TempData["SuccessMessage"] = "Kvalifikacija uspešno dodata!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja kvalifikacije.");
                }
            }

            // Ako dođe do greške, vraća se ista forma sa trenutnim podacima
            return View(kvalifikacija);
        }

        public async Task<IActionResult> UpdateKvalifikacija(int kvalifikacijaId)
        {
            try
            {
                var kvalifikacija = await kvalifikacijaByIdUseCase.ExecuteAsync(kvalifikacijaId);

                if (kvalifikacija != null)
                {
                    return View(kvalifikacija);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Greška prilikom Update kvalifikacije.";
                return View();
            }

            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateKvalifikacija(Kvalifikacija kvalifikacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool uspesno = await editKvalifikacijaUseCase.ExecuteAsync(kvalifikacija);

                    if (!uspesno)
                    {
                        TempData["ErrorMessage"] = "Kvalifikacija nije pronađena.";
                        return RedirectToAction(nameof(Index));
                    }
                    // Ako je ažuriranje uspešno, dodajemo poruku u TempData
                    // koja će biti prikazana na Index stranici

                    TempData["SuccessMessage"] = "Kvalifikacija uspešno ažurirana!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["ErrorMessage"] = "Greška prilikom izmene kvalifikacije.";
                }
            }

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(kvalifikacija);
        }

        public async Task<IActionResult> DeleteKvalifikacija(int kvalifikacijaId)
        {
            try
            {
                // await webApiExecuter.InvokeDelete($"kvalifikacije/{kvalifikacijaId}");
                // Pozivamo servis za brisanje kvalifikacije
                bool rezultat = await deleteKvalifikacijaUseCase.ExecuteAsync(kvalifikacijaId);

                if (rezultat)
                {
                    TempData["SuccessMessage"] = "Kvalifikacija je uspešno obrisana.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Greška prilikom brisanja kvalifikacije.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }


            // Dodajemo poruku u TempData, koja će biti prikazana na Index stranici
            TempData["SuccessMessage"] = "Kvalifikacija je uspešno obrisana.";

            return RedirectToAction(nameof(Index));

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Sistematizacije;
using Zaposleni_Blazor.UseCases.Zaposleni.Interfaces;

namespace Zaposleni_Clean_MVC_Vue.Controllers
{
    public class ZaposleniController : Controller
    {
        private readonly ICrudZaposlenUseCases crudZaposlenUseCases;

        public ZaposleniController(ICrudZaposlenUseCases crudZaposlenUseCases)
        {
            this.crudZaposlenUseCases = crudZaposlenUseCases;
        }

        public async Task<IActionResult> Index()
        {
            try
            {

                // Smeštanje rezultata poziva u promenljivu
                var zaposleni = await crudZaposlenUseCases.GetAllAsync();

                // Korišćenje promenljive u View metodi
                return View(zaposleni);
            }
            catch (Exception ex)
            {
                // Logovanje greške
                Console.WriteLine($"Error fetching data: {ex.Message}");

                return View(new List<Zaposlen>()); // Vraćanje prazne liste u slučaju greške
            }
        }

        public async Task<IActionResult> AddZaposleni()
        {
            await UcitajMesta();
            await UcitajKvalifikacije();

            // Vraćanje prikaza forme
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddZaposleni(Zaposlen zaposlen)
        {

            if (ModelState.IsValid)
            {
                var success = await crudZaposlenUseCases.AddAsync(zaposlen);

                if (success)
                {
                    TempData["SuccessMessage"] = "Zaposleni je uspešno dodat.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja zaposlenog.");
                }
            }

            await UcitajMesta();
            await UcitajKvalifikacije();

            return View(zaposlen);
        }


        public async Task<IActionResult> UpdateZaposleni(int zaposlenId)
        {

            try
            {
                var zaposlen = await crudZaposlenUseCases.GetByIdAsync(zaposlenId);

                if (zaposlen != null)
                {
                    await UcitajMesta();
                    await UcitajKvalifikacije();

                    return View(zaposlen);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Greška prilikom Update zaposlenog.";
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateZaposleni(Zaposlen zaposlen)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool uspesno = await crudZaposlenUseCases.UpdateAsync(zaposlen);

                    if (!uspesno)
                    {
                        TempData["ErrorMessage"] = "Zaposleni nije pronađen.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["SuccessMessage"] = "Zaposleni je uspešno izmenjen.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "Greška prilikom izmene zaposlenog.";
                }
            }

            // Ako postoji greška u modelu, ponovo učitajte listu mesta i kvalifikacija da bi bile dostupne u formi

            await UcitajMesta();
            await UcitajKvalifikacije();

            return View(zaposlen);
        }

        public async Task<IActionResult> DeleteZaposleni(int zaposlenId)
        {

            // Na osnovu rezultata postavljamo odgovarajuću poruku

            try
            {
                // Pozivamo servis koji sadrži logiku za brisanje
                var rezultat = await crudZaposlenUseCases.DeleteAsync(zaposlenId);

                // Na osnovu rezultata postavljamo odgovarajuću poruku
                if (rezultat)
                {
                    TempData["SuccessMessage"] = "Zaposleni je uspešno obrisan";
                }
                else
                {
                    TempData["ErrorMessage"] = "Greška prilikom brisanja zaposlenog.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            // Redirektujemo korisnika nazad na Index stranicu
            return RedirectToAction(nameof(Index));
        }
        private async Task UcitajKvalifikacije()
        {
            var kvalifikacije = await crudZaposlenUseCases.GetAllKvalifikacijeAsync();
            // Postavljanje liste kvalifikacija u ViewBag kako bi bila dostupna u formi
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");
        }

        private async Task UcitajMesta()
        {
            var mesta = await crudZaposlenUseCases.GetAllMestaAsync();
            // Postavljanje liste mesta u ViewBag kako bi bila dostupna u formi
            ViewBag.Mesta = new SelectList(mesta, "Id", "Naziv");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.ClanDomacinstva;
using Zaposleni_Blazor.UseCases.Sistematizacije.Interfaces;
using Zaposleni_Blazor.UseCases.Zaposleni;

namespace Zaposleni_Clean_MVC.Controllers
{
    public class SistematizacijeController : Controller
    {
        private readonly ICrudSistematizacijaUseCases crudSistematizacijaUseCases;

        public SistematizacijeController(ICrudSistematizacijaUseCases crudSistematizacijaUseCases)
        {
            this.crudSistematizacijaUseCases = crudSistematizacijaUseCases;
        }

        public async Task<IActionResult> Index()
        {
            var sistematizacije = await crudSistematizacijaUseCases.GetAllAsync();

            // Proverite da li je sistematizacija null ili prazna
            if (sistematizacije == null || sistematizacije.Count() == 0)
            {
                // Ako jeste, možete dodati poruku ili preusmeriti na drugu stranicu
                // return RedirectToAction("Error", "Home");
                return View("Error");
            }

            // Korišćenje promenljive u View metodi
            return View(sistematizacije);
        }

        public async Task<IActionResult> AddSistematizacija()
        {
            await UcitajOrgJedinice(); // Učitavanje organizacionih jedinica i ViewBag
            await UcitajKvalifikacije(); // Učitavanje kvalifikacija i ViewBag

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSistematizacija(Sistematizacija sistematizacija)
        {

            if (ModelState.IsValid)
            {
                var success = await crudSistematizacijaUseCases.AddAsync(sistematizacija);
                if (success)
                {
                    TempData["SuccessMessage"] = "Sistematizacija je uspešno dodata.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja sistematizacije.");
                }
            }

            await UcitajOrgJedinice();
            await UcitajKvalifikacije();

            return View(sistematizacija);
        }
        public async Task<IActionResult> UpdateSistematizacija(int sistematizacijaId)
        {
            try
            {
                var sistematizacija = await crudSistematizacijaUseCases.GetByIdAsync(sistematizacijaId);

                if (sistematizacija != null)
                {
                    await UcitajOrgJedinice();
                    await UcitajKvalifikacije();

                    return View(sistematizacija);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom Update sistematizacije {ex.Message}";
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSistematizacija(Sistematizacija sistematizacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool uspesno = await crudSistematizacijaUseCases.UpdateAsync(sistematizacija);

                    if (!uspesno)
                    {
                        TempData["ErrorMessage"] = "Sistematizacija nije pronađena.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["SuccessMessage"] = "Sistematizacija je uspešno izmenjena.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Greška prilikom izmene sistematizacije: {ex.Message}";
                }
            }

            await UcitajOrgJedinice();
            await UcitajKvalifikacije();

            return View(sistematizacija);
        }

        public async Task<IActionResult> DeleteSistematizacija(int sistematizacijaId)
        {
            try
            {
                // Pozivamo servis koji sadrži logiku za brisanje
                var rezultat = await crudSistematizacijaUseCases.DeleteAsync(sistematizacijaId);

                // Na osnovu rezultata postavljamo odgovarajuću poruku
                if (rezultat)
                {
                    TempData["SuccessMessage"] = "Sistematizacija je uspešno obrisana";
                }
                else
                {
                    TempData["ErrorMessage"] = "Greška prilikom brisanja sistematizacije.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            // Redirektujemo korisnika nazad na Index stranicu
            return RedirectToAction(nameof(Index));
        }

        private async Task UcitajOrgJedinice()
        {
            var orgJedinice = await crudSistematizacijaUseCases.GetAllOrganizacioneJediniceAsync();
            ViewBag.OrgJedinice = new SelectList(orgJedinice, "Id", "Naziv");
        }

        private async Task UcitajKvalifikacije()
        {
            var kvalifikacije = await crudSistematizacijaUseCases.GetAllKvalifikacijeAsync();
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");
        }
    }
}

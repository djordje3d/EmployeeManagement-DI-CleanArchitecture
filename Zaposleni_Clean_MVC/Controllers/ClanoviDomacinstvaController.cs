using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.ClanDomacinstva.Interfaces;

namespace Zaposleni_Clean_MVC.Controllers
{
    public class ClanoviDomacinstvaController : Controller
    {
        private readonly ICrudClanoviDomacinstvaUseCases crudClanoviDomacinstvaUseCases;

        public ClanoviDomacinstvaController(ICrudClanoviDomacinstvaUseCases crudClanoviDomacinstvaUseCases)
        {
            this.crudClanoviDomacinstvaUseCases = crudClanoviDomacinstvaUseCases;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var clanoviDomacinstva = await crudClanoviDomacinstvaUseCases.GetAllAsync();

                // Korišćenje promenljive u View metodi
                return View(clanoviDomacinstva);
            }
            catch (Exception ex)
            {
                // Logovanje greške
                Console.WriteLine($"Error fetching data: {ex.Message}");

                return View(new List<ClanoviDomacinstva>()); // Vraćanje prazne liste u slučaju greške
            }
        }

        public async Task<IActionResult> AddClanDomacinstva()
        {
            // Preuzimanje liste mesta sa API-ja (ili baze podataka)
            //var zaposleniApi = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");  // Prilagodite API URL
            await UcitajZaposlene();

            // Vraćanje prikaza forme
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClanDomacinstva(ClanoviDomacinstva clanDom)
        {
            if (ModelState.IsValid)
            {
                var success = await crudClanoviDomacinstvaUseCases.AddAsync(clanDom);

                if (success)
                {
                    TempData["SuccessMessage"] = "Član domaćinstva uspešno dodat!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja Člana domaćinstva.");
                }
            }

            await UcitajZaposlene();

            return View(clanDom);
        }

        public async Task<IActionResult> UpdateClanDomacinstva(int clanId)
        {

            try
            {
                //var clanApi = await webApiExecuter.InvokeGet<ClanoviDomacinstva>($"clanovidomacinstva/{clanId}");
                var clan = await crudClanoviDomacinstvaUseCases.GetByIdAsync(clanId);

                if (clan != null)
                {
                    await UcitajZaposlene();
                    return View(clan);
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
        public async Task<IActionResult> UpdateClanDomacinstva(ClanoviDomacinstva clanDom)
        {
            if (!ModelState.IsValid)
            {
                await UcitajZaposlene();
                return View(clanDom);
            }

            try
            {
                bool uspesno = await crudClanoviDomacinstvaUseCases.UpdateAsync(clanDom);

                if (!uspesno)
                {
                    TempData["ErrorMessage"] = "Član domaćinstva nije pronađen.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Član domaćinstva je uspešno izmenjen.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom izmene člana domaćinstva: {ex.Message}";
                await UcitajZaposlene();
                return View(clanDom);
            }
        }

        public async Task<IActionResult> DeleteClanDomacinstva(int clanId)
        {
            // Pozivamo servis koji sadrži logiku za brisanje
            try
            {
                bool rezultat = await crudClanoviDomacinstvaUseCases.DeleteAsync(clanId);

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

        private async Task UcitajZaposlene()
        {
            var zaposleni = await crudClanoviDomacinstvaUseCases.GetAllZaposleniAsync();
            ViewBag.Zaposleni = new SelectList(zaposleni, "Id", "Ime");
        }
    }
}

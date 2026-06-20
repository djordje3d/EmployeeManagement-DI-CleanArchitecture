using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica.Interfaces;

namespace Zaposleni_Clean_MVC_Vue.Controllers
{
    public class OrgJediniceController : Controller
    {
        private readonly ICrudOrganizacionaJedinicaUseCases crudOrganizacionaJedinicaUseCases;

        public OrgJediniceController(ICrudOrganizacionaJedinicaUseCases crudOrganizacionaJedinicaUseCases)
        {
            this.crudOrganizacionaJedinicaUseCases = crudOrganizacionaJedinicaUseCases;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var orgJedinice = await crudOrganizacionaJedinicaUseCases.GetAllAsync();

                return View(orgJedinice);
            }
            catch (Exception ex)
            {
                // Logovanje greške
                Console.WriteLine($"Error fetching data: {ex.Message}");

                return View(new List<OrganizacioneJedinice>()); // Vraćanje prazne liste u slučaju greške
            }
        }
        public async Task<IActionResult> AddOrgJedinica()
        {
            await UcitajGrupeMestaTroskova();

            // Vraćanje prikaza forme
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrgJedinica(OrganizacioneJedinice orgJed)
        {
            if (ModelState.IsValid)
            {
                var success = await crudOrganizacionaJedinicaUseCases.AddAsync(orgJed);

                if (success)
                {
                    TempData["SuccessMessage"] = "OJ uspešno dodata!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Greška prilikom dodavanja OJ.");
                }
            }

            await UcitajGrupeMestaTroskova();

            // Ako dođe do greške, vraća se ista forma sa trenutnim podacima
            return View(orgJed);
        }

        public async Task<IActionResult> UpdateOrgJedinica(int orgJedId)
        {
            try
            {
                var orgJedinica = await crudOrganizacionaJedinicaUseCases.GetByIdAsync(orgJedId);

                if (orgJedinica != null)
                {
                    await UcitajGrupeMestaTroskova();

                    return View(orgJedinica);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "Greška prilikom Update OJ.";
                return View();
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrgJedinica(OrganizacioneJedinice orgJed)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za ažuriranje mesta
                    //await webApiExecuter.InvokePut($"organizacionejedinice/{orgJed.Id}", orgJed);
                    bool uspesno = await crudOrganizacionaJedinicaUseCases.UpdateAsync(orgJed);

                    if (!uspesno)
                    {
                        TempData["ErrorMessage"] = "OJ nije pronađena.";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["SuccessMessage"] = "OJ je uspešno izmenjena!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["ErrorMessage"] = "Greška prilikom izmene sistematizacije.";
                }
            }

            await UcitajGrupeMestaTroskova();

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(orgJed);
        }

        public async Task<IActionResult> DeleteOrgJedinica(int orgJedId)
        {
            try
            {
                // Pozivamo servis koji sadrži logiku za brisanje
                bool rezultat = await crudOrganizacionaJedinicaUseCases.DeleteAsync(orgJedId);

                // Na osnovu rezultata postavljamo odgovarajuću poruku
                if (rezultat)
                {
                    TempData["SuccessMessage"] = "OJ je uspešno obrisana.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Greška prilikom brisanja OJ.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            // Redirektujemo korisnika nazad na Index stranicu
            return RedirectToAction(nameof(Index));
        }

        private async Task UcitajGrupeMestaTroskova()
        {
            var grupaMestaTroskova = await crudOrganizacionaJedinicaUseCases.GetAllGrupaMestaTroskovaAsync();
            // Postavljanje liste mesta u ViewBag kako bi bila dostupna u formi
            ViewBag.Mesta = new SelectList(grupaMestaTroskova, "Id", "Naziv");
        }
    }
}

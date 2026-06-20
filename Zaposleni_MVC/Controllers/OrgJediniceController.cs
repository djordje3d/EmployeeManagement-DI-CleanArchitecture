using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Clean_MVC_API.UseCases.Interfaces;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "OJ")]

    [TokenAuthenticationFilter] // Ova klasa će automatski proveriti token pre svake akcije umesto da se poziva u svakoj akciji CheckToken()
    public class OrgJediniceController : Controller
    {
        private readonly ICrudOrganizacionaJedinicaUseCases _crudOrganizacionaJedinicaUseCases;

        public OrgJediniceController(ICrudOrganizacionaJedinicaUseCases crudOrganizacionaJedinicaUseCases)
        {
            _crudOrganizacionaJedinicaUseCases = crudOrganizacionaJedinicaUseCases;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                // Pozivanje API-ja za dobijanje liste OJ
                //var orgJedinice1 = await webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
                var orgJedinice = await _crudOrganizacionaJedinicaUseCases.GetAllAsync();


                // Prosleđivanje liste modelu View-a
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
            var grupaMestaTroskova = await _crudOrganizacionaJedinicaUseCases.GetAllGrupaMestaTroskovaAsync();  // Prilagodite API URL


            // Postavljanje liste mesta u ViewBag kako bi bila dostupna u formi
            ViewBag.Mesta = new SelectList(grupaMestaTroskova, "Id", "Naziv");

            // Vraćanje prikaza forme
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddOrgJedinica(OrganizacioneJedinice orgJed)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za dodavanje novog mesta
                    var response = await _crudOrganizacionaJedinicaUseCases.AddAsync(orgJed);


                    if (response != null)
                    {
                        TempData["SuccessMessage"] = "Organizaciona jedinica je uspešno dodata!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Dodavanje organizacione jedinice nije uspelo.");
                    }
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }
            else
            {
                // Provera grešaka u ModelState
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Greška: {error.ErrorMessage}");
                }
            }
            // Ako dođe do greške, vraća se ista forma sa trenutnim podacima

            // Ponovno učitavanje liste mesta troškova u slučaju greške
            var grupaMestaTroskova = await _crudOrganizacionaJedinicaUseCases.GetAllGrupaMestaTroskovaAsync();
            ViewBag.Mesta = new SelectList(grupaMestaTroskova, "Id", "Naziv");

            return View(orgJed);
        }

        public async Task<IActionResult> UpdateOrgJedinica(int orgJedId)
        {
            try
            {
                // Pozivanje API-ja za dobijanje mesta po ID-ju
                var orgJedinica = await _crudOrganizacionaJedinicaUseCases.GetByIdAsync(orgJedId);

                if (orgJedinica != null)
                {
                    // Preuzimanje liste mesta sa API-ja (ili baze podataka)
                    var grupeMestaTroskova = await _crudOrganizacionaJedinicaUseCases.GetAllGrupaMestaTroskovaAsync();

                    // Postavljanje liste mesta u ViewBag kako bi bila dostupna u formi
                    ViewBag.Mesta = new SelectList(grupeMestaTroskova, "Id", "Naziv");
                    return View(orgJedinica);
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Neočekivana greška: " + ex.Message;
                return RedirectToAction(nameof(Index));
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
                    await _crudOrganizacionaJedinicaUseCases.UpdateAsync(orgJed);

                    TempData["SuccessMessage"] = "OJ uspešno ažurirano!";
                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(orgJed);
        }

        public async Task<IActionResult> DeleteOrgJedinica(int orgJedId)
        {
            try
            {
                await _crudOrganizacionaJedinicaUseCases.DeleteAsync(orgJedId);

                // Dodajemo poruku u TempData, koja će biti prikazana na Index stranici
                TempData["SuccessMessage"] = "OJ je uspešno obrisana!";

                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                var organJedinice = await _crudOrganizacionaJedinicaUseCases.GetAllAsync();
                return View(nameof(Index), organJedinice); 
            }
        }

        private void HandleWebApiException(WebApiException ex)
        {
            Console.WriteLine($"API Error: {ex.Message}");

            if (ex.ErrorResponse != null &&
                ex.ErrorResponse.Errors != null &&
                ex.ErrorResponse.Errors.Count > 0)
            {
                foreach (var error in ex.ErrorResponse.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
                }
            }
            else if (ex.ErrorResponse != null)
            {
                ModelState.AddModelError("Error", ex.ErrorResponse.Title);
            }
            else
            {
                ModelState.AddModelError("Error", ex.Message);
            }
        }
    }
}

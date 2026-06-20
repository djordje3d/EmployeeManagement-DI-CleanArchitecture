using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "Sistematizacije")]

    [TokenAuthenticationFilter] // Ova klasa će automatski proveriti token pre svake akcije umesto da se poziva u svakoj akciji CheckToken()
    public class SistematizacijeController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public SistematizacijeController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index()
        {

            // Smeštanje rezultata poziva u promenljivu
            var sistematizacije = await webApiExecuter.InvokeGet<List<Sistematizacija>>("sistematizacije");

            // Korišćenje promenljive u View metodi
            return View(sistematizacije);
        }

        public async Task<IActionResult> AddSistematizacija()
        {
            // Preuzimanje liste orgJed sa API-ja (ili baze podataka)
            var orgJedinice = await webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
            // Preuzimanje liste kvalifikacija sa API-ja (ili baze podataka)
            var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");  // Prilagodite API URL

            // Postavljanje liste orgJed u ViewBag kako bi bila dostupna u formi
            ViewBag.OrgJedinice = new SelectList(orgJedinice, "Id", "Naziv");
            // Postavljanje liste kvalifikacija u ViewBag kako bi bila dostupna u formi
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");
            // Vraćanje prikaza forme
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSistematizacija(Sistematizacija sistematizacija)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await webApiExecuter.InvokePost("sistematizacije", sistematizacija);
                    if (response != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            var orgJedinice = await webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
            ViewBag.OrgJedinice = new SelectList(orgJedinice, "Id", "Naziv"); // Ponovno punjenje liste orgJed u slučaju greške

            var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv"); // Ponovno punjenje liste kvalifikacija u slučaju greške

            return View(sistematizacija);
        }


        public async Task<IActionResult> UpdateSistematizacija(int sistematizacijaId)
        {

            try
            {
                var sistematizacija = await webApiExecuter.InvokeGet<Sistematizacija>($"sistematizacije/{sistematizacijaId}");

                if (sistematizacija != null)
                {
                    // Preuzimanje liste orgJed sa API-ja (ili baze podataka)
                    var orgJedinice = await webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
                    // Preuzimanje liste kvalifikacija sa API-ja (ili baze podataka)
                    var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");  // Prilagodite API URL


                    // Postavljanje liste orgJed u ViewBag kako bi bila dostupna u formi
                    ViewBag.OrgJedinice = new SelectList(orgJedinice, "Id", "Naziv"); // Ponovno punjenje liste orgJed u slučaju greške

                    // Postavljanje liste kvalifikacija u ViewBag kako bi bila dostupna u formi
                    ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");

                    return View(sistematizacija);
                }
            }
            catch (WebApiException ex)
            {
                TempData["ErrorMessage"] = "Došlo je do greške na strani API-ja: " + ex.Message;
                //return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> UpdateSistematizacija(Sistematizacija sistematizacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Updating Sistematizacija with ID: {sistematizacija.Id}");
                    var apiUrl = $"sistematizacije/{sistematizacija.Id}";
                    Console.WriteLine($"Calling API with URL: {apiUrl}");

                    await webApiExecuter.InvokePut($"sistematizacije/{sistematizacija.Id}", sistematizacija);

                    return RedirectToAction(nameof(Index));
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
            // Ako postoji greška u modelu, ponovo učitajte listu orgJed i kvalifikacija da bi bile dostupne u formi

            // Preuzimanje liste orgJed sa API-ja (ili baze podataka)
            var orgJedinice = await webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
            ViewBag.OrgJedinice = new SelectList(orgJedinice, "Id", "Naziv");

            // Preuzimanje liste kvalifikacija sa API-ja (ili baze podataka)
            var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");  // Prilagodite API URL
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");

            return View(sistematizacija);
        }
        public async Task<IActionResult> DeleteSistematizacija(int sistematizacijaId)
        {
            try
            {
                await webApiExecuter.InvokeDelete($"sistematizacije/{sistematizacijaId}");

                TempData["SuccessMessage"] = "Sistematizacija je uspešno obrisana.";
                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                var sistematizacije = await webApiExecuter.InvokeGet<List<Sistematizacija>>("sistematizacije");
                return View(nameof(Index), sistematizacije);
            }
        }
        private void HandleWebApiException(WebApiException ex)
        {
            Console.WriteLine($"API Error: {ex.Message}");

            if (ex.ErrorResponse != null && ex.ErrorResponse.Errors?.Count > 0)
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

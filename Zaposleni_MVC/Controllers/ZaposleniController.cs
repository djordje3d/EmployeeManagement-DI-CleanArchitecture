using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "Zaposleni")]

    [TokenAuthenticationFilter] // Ova klasa će automatski proveriti token pre svake akcije umesto da se poziva u svakoj akciji CheckToken()
    public class ZaposleniController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public ZaposleniController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index()
        {
            //// Provera da li postoji JWT token u sesiji
            //var authCheck = CheckToken();
            //if (authCheck != null) return authCheck;

            try
            {
                // Pozivanje API-ja za dobijanje liste mesta
                var zaposleni = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");
                return View(zaposleni);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return View(new List<Zaposlen>());
            }
        }
        public async Task<IActionResult> AddZaposleni()
        {
            var mesta = await webApiExecuter.InvokeGet<List<Mesto>>("mesta");  // Prilagodite API URL
            var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");  // Prilagodite API URL

            // Postavljanje liste mesta u ViewBag kako bi bila dostupna u formi
            ViewBag.Mesta = new SelectList(mesta, "Id", "Naziv");

            // Postavljanje liste kvalifikacija u ViewBag kako bi bila dostupna u formi
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");

            // Vraćanje prikaza forme
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddZaposleni(Zaposlen zaposlen)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await webApiExecuter.InvokePost("zaposleni", zaposlen);
                    if (response != null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Neočekivana greška: {ex.Message}");
                }

            }

            var mesta = await webApiExecuter.InvokeGet<List<Mesto>>("mesta");
            ViewBag.Mesta = new SelectList(mesta, "Id", "Naziv"); // Ponovno punjenje liste mesta u slučaju greške

            var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("mesta");
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv"); // Ponovno punjenje liste kvalifikacija u slučaju greške

            return View(zaposlen);
        }


        public async Task<IActionResult> UpdateZaposleni(int zaposlenId)
        {

            try
            {
                var zaposlen = await webApiExecuter.InvokeGet<Zaposlen>($"zaposleni/{zaposlenId}");

                if (zaposlen != null)
                {
                    // Preuzimanje liste mesta sa API-ja (ili baze podataka)
                    var mesta = await webApiExecuter.InvokeGet<List<Mesto>>("mesta");
                    // Preuzimanje liste kvalifikacija sa API-ja (ili baze podataka)
                    var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");  // Prilagodite API URL


                    // Postavljanje liste mesta u ViewBag kako bi bila dostupna u formi
                    ViewBag.Mesta = new SelectList(mesta, "Id", "Naziv");

                    // Postavljanje liste kvalifikacija u ViewBag kako bi bila dostupna u formi
                    ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");

                    return View(zaposlen);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Neočekivana greška: {ex.Message}";
                return RedirectToAction(nameof(Index));
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
                    Console.WriteLine($"Updating Zaposlen with ID: {zaposlen.Id}");

                    var apiUrl = $"zaposleni/{zaposlen.Id}";

                    Console.WriteLine($"Calling API with URL: {apiUrl}");

                    await webApiExecuter.InvokePut($"zaposleni/{zaposlen.Id}", zaposlen);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Neočekivana greška: {ex.Message}");
                }
            }

            // Ako postoji greška u modelu, ponovo učitajte listu mesta i kvalifikacija da bi bile dostupne u formi

            // Preuzimanje liste mesta sa API-ja (ili baze podataka)
            var mesta = await webApiExecuter.InvokeGet<List<Mesto>>("mesta"); // Prilagodite API URL
            ViewBag.Mesta = new SelectList(mesta, "Id", "Naziv");

            // Preuzimanje liste kvalifikacija sa API-ja (ili baze podataka)
            var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");  // Prilagodite API URL
            ViewBag.Kvalifikacije = new SelectList(kvalifikacije, "Id", "Naziv");

            return View(zaposlen);
        }

        public async Task<IActionResult> DeleteZaposleni(int zaposlenId)
        {
            try
            {
                await webApiExecuter.InvokeDelete($"zaposleni/{zaposlenId}"); // Pozivanje API-ja za brisanje zaposlenog

                // Dodajemo poruku u TempData, koja će biti prikazana na Index stranici
                TempData["SuccessMessage"] = "Zaposleni je uspešno obrisan.";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Neočekivana greška: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // Ova metoda proverava da li je JWT token prisutan u sesiji
        // NAPOMENA: Ova metoda više nije potrebna jer je provera tokena prebačena u filter TokenAuthenticationFilter !!!
        private IActionResult? CheckToken()
        {
            var token = HttpContext.Session.GetString("JwtToken");

            if (string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Sesija je istekla. Molimo vas da se ponovo prijavite.";
                return RedirectToAction("Login", "Auth", new { returnUrl = Request.Path }); // Preusmeravanje sa trenutnim URL-om
            }

            return null; // Ako je token validan, nastavi dalje
        }


    }
}

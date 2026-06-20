using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;


namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "Članovi")]

    [TokenAuthenticationFilter] // Ova klasa će automatski proveriti token pre svake akcije umesto da se poziva u svakoj akciji CheckToken()
    public class ClanoviDomacinstvaController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public ClanoviDomacinstvaController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index()
        {
            // Smeštanje rezultata poziva u promenljivu
            var clanoviDomacinstva = await webApiExecuter.InvokeGet<List<ClanoviDomacinstva>>("clanovidomacinstva");

            // Korišćenje promenljive u View metodi
            return View(clanoviDomacinstva);
        }

        public async Task<IActionResult> AddClanDomacinstva()
        {
            // Preuzimanje liste mesta sa API-ja (ili baze podataka)
            var zaposleni = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");  // Prilagodite API URL

            // Postavljanje liste zaposlenih u ViewBag kako bi bili dostupni u formi
            ViewBag.Zaposleni = new SelectList(zaposleni, "Id", "Ime");

            // Vraćanje prikaza forme
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddClanDomacinstva(ClanoviDomacinstva clan)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await webApiExecuter.InvokePost("clanovidomacinstva", clan);
                    if (response != null)
                    {
                        TempData["SuccessMessage"] = "Član domaćinstva uspešno dodat!";
                        return RedirectToAction(nameof(Index));
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

            // Preuzimanje liste mesta sa API-ja (ili baze podataka)
            var zaposleni = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");  // Ponovno punjenje liste mesta u slučaju greške

            // Postavljanje liste zaposlenih u ViewBag kako bi bili dostupni u formi
            ViewBag.Zaposleni = new SelectList(zaposleni, "Id", "Ime");

            return View(clan);
        }

        public async Task<IActionResult> UpdateClanDomacinstva(int clanId)
        {

            try
            {
                var clan = await webApiExecuter.InvokeGet<ClanoviDomacinstva>($"clanovidomacinstva/{clanId}");

                if (clan != null)
                {
                    // Preuzimanje liste zaposlenih sa API-ja (ili baze podataka)
                    var zaposleni = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");

                    // Postavljanje liste zaposlenih u ViewBag kako bi bila dostupna u formi
                    ViewBag.Zaposleni = new SelectList(zaposleni, "Id", "Ime");

                    return View(clan);
                }
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }


            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClanDomacinstva(ClanoviDomacinstva clanoviDomacinstva)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Updating Zaposlen with ID: {clanoviDomacinstva.Id}");
                    
                    var apiUrl = $"clanovidomacinstva/{clanoviDomacinstva.Id}";
                    Console.WriteLine($"Calling API with URL: {apiUrl}");

                    await webApiExecuter.InvokePut($"clanovidomacinstva/{clanoviDomacinstva.Id}", clanoviDomacinstva);

                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }

            }

            // Ako postoji greška u modelu, ponovo učitajte listu mesta i kvalifikacija da bi bile dostupne u formi

            // Preuzimanje liste zaposlenih sa API-ja (ili baze podataka)
            var zaposleni = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");

            // Postavljanje liste zaposlenih u ViewBag kako bi bila dostupna u formi
            ViewBag.Zaposleni = new SelectList(zaposleni, "Id", "Ime");

            return View(clanoviDomacinstva);
        }

        public async Task<IActionResult> DeleteClanDomacinstva(int clanId)
        {
            try
            {
                await webApiExecuter.InvokeDelete($"clanovidomacinstva/{clanId}");

                // Dodajemo poruku u TempData, koja će biti prikazana na Index stranici
                TempData["SuccessMessage"] = "Član domaćinstva je uspešno obrisan.";

                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                var zaposleni = await webApiExecuter.InvokeGet<List<Zaposlen>>("zaposleni");
                return View(nameof(Index), zaposleni);
            }

        }
        private void HandleWebApiException(WebApiException ex)
        {
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

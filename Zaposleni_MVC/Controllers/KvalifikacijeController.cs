using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "Kvalifikacije")]

    [TokenAuthenticationFilter] // Ova klasa će automatski proveriti token pre svake akcije umesto da se poziva u svakoj akciji CheckToken()
    public class KvalifikacijeController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public KvalifikacijeController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        // Pozivanje API-ja za dobijanje liste Kvalifikacija
        //        var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");

        //        // Sortiranje liste po Naziv-u
        //        var sortiraneKvalifikacije = kvalifikacije.OrderBy(k => k.LicniStepenKv).ToList();

        //        // Prosleđivanje sortirane liste modelu View-a
        //        return View(sortiraneKvalifikacije);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Logovanje greške
        //        Console.WriteLine($"Error fetching data: {ex.Message}");
        //        return View(new List<Kvalifikacija>()); // Vraćanje prazne liste u slučaju greške
        //    }
        //}

        public async Task<IActionResult> Index(string sortOrder)
        {
            try
            {
                // Pozivanje API-ja za dobijanje liste Kvalifikacija
                var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");

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
        public async Task<IActionResult> AddKvalifikacija(Kvalifikacija kvalifikacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za dodavanje nove Kvalifikacije
                    var response = await webApiExecuter.InvokePost<Kvalifikacija>("kvalifikacije", kvalifikacija);

                    if (response != null)
                    {
                        TempData["SuccessMessage"] = "Kvalifikacija uspešno dodata!";
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
            // Ako dođe do greške, vraća se ista forma sa trenutnim podacima
            return View(kvalifikacija);
        }

        public async Task<IActionResult> UpdateKvalifikacija(int kvalifikacijaId)
        {
            try
            {
                // Pozivanje API-ja za dobijanje kvalifikacije po ID-ju
                var kvalifikacija = await webApiExecuter.InvokeGet<Kvalifikacija>($"kvalifikacije/{kvalifikacijaId}");

                if (kvalifikacija == null)
                {
                    TempData["ErrorMessage"] = "Kvalifikacija nije pronađena.";
                    return RedirectToAction(nameof(Index));
                }

                return View(kvalifikacija);
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateKvalifikacija(Kvalifikacija kvalifikacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za ažuriranje kvalifikacije
                    await webApiExecuter.InvokePut($"kvalifikacije/{kvalifikacija.Id}", kvalifikacija);

                    TempData["SuccessMessage"] = "Kvalifikacija uspešno ažurirana!";
                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(kvalifikacija);
        }

        public async Task<IActionResult> DeleteKvalifikacija(int kvalifikacijaId)
        {
            try
            {
                await webApiExecuter.InvokeDelete($"kvalifikacije/{kvalifikacijaId}");

                // Dodajemo poruku u TempData, koja će biti prikazana na Index stranici
                TempData["SuccessMessage"] = "Kvalifikacija je uspešno obrisana.";

                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                var kvalifikacije = await webApiExecuter.InvokeGet<List<Kvalifikacija>>("kvalifikacije");
                return View(nameof(Index), kvalifikacije);
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

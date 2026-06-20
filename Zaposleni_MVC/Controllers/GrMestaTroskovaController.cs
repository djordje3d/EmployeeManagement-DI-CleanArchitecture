using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "MT")]

    [TokenAuthenticationFilter] // Ova klasa će automatski proveriti token pre svake akcije umesto da se poziva u svakoj akciji CheckToken()
    public class GrMestaTroskovaController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public GrMestaTroskovaController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            try
            {
                // Pozivanje API-ja za dobijanje liste GrupaMestaTroskova
                var grupeMestaTroskova = await webApiExecuter.InvokeGet<List<GrupaMestaTroskova>>("grupamestatroskova");

                // Dinamičko sortiranje
                grupeMestaTroskova = sortOrder switch
                {
                    "id" => grupeMestaTroskova.OrderBy(k => k.Id).ToList(),
                    "id_desc" => grupeMestaTroskova.OrderByDescending(k => k.Id).ToList(),
                    "Grupa" => grupeMestaTroskova.OrderBy(k => k.Grupa).ToList(),
                    "Grupa_desc" => grupeMestaTroskova.OrderByDescending(k => k.Grupa).ToList(),
                    "Naziv" => grupeMestaTroskova.OrderBy(k => k.Naziv).ToList(),
                    "Naziv_desc" => grupeMestaTroskova.OrderByDescending(k => k.Naziv).ToList(),
                    _ => grupeMestaTroskova.OrderBy(k => k.Id).ToList() // default
                };

                ViewData["CurrentSort"] = sortOrder;

                // Prosleđivanje sortirane liste modelu View-a
                return View(grupeMestaTroskova);
            }
            catch (Exception ex)
            {
                // Logovanje greške
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return View(new List<GrupaMestaTroskova>()); // Vraćanje prazne liste u slučaju greške
            }
        }


        public IActionResult AddGrupaMestaTroskova()
        {

            return View(new GrupaMestaTroskova());
        }

        [HttpPost]
        public async Task<IActionResult> AddGrupaMestaTroskova(GrupaMestaTroskova grupaMestaTroskova)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za dodavanje nove Kvalifikacije
                    var response = await webApiExecuter.InvokePost<GrupaMestaTroskova>("grupamestatroskova", grupaMestaTroskova);

                    if (response != null)
                    {
                        TempData["SuccessMessage"] = "Grupa mesta troskova uspešno dodata!";
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
            return View(grupaMestaTroskova);
        }

        public async Task<IActionResult> UpdateGrupaMestaTroskova(int grMestaTroskovaId)
        {
            try
            {
                // Pozivanje API-ja za dobijanje kvalifikacije po ID-ju
                var grMestaTroskova = await webApiExecuter.InvokeGet<GrupaMestaTroskova>($"grupamestatroskova/{grMestaTroskovaId}");

                if (grMestaTroskova == null)
                {
                    TempData["ErrorMessage"] = "GrupaMestaTroskova nije pronađena.";
                    return RedirectToAction(nameof(Index));
                }

                return View(grMestaTroskova);
            }
            catch (WebApiException ex)
            {
                TempData["ErrorMessage"] = "Došlo je do greške na strani API-ja: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Neočekivana greška: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGrupaMestaTroskova(GrupaMestaTroskova grMestaTroskova)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za ažuriranje kvalifikacije
                    await webApiExecuter.InvokePut($"grupamestatroskova/{grMestaTroskova.Id}", grMestaTroskova);

                    TempData["SuccessMessage"] = "GrupaMestaTroskova uspešno ažurirana!";
                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {
                    ModelState.AddModelError(string.Empty, "Došlo je do greške na strani API-ja: " + ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Neočekivana greška: " + ex.Message);
                }
            }

            // Ako dođe do greške, vraća se forma sa trenutnim podacima
            return View(grMestaTroskova);
        }

        public async Task<IActionResult> DeleteGrupaMestaTroskova(int grMestaTroskovaId)
        {
            try
            {
                await webApiExecuter.InvokeDelete($"grupamestatroskova/{grMestaTroskovaId}");

                // Dodajemo poruku u TempData, koja će biti prikazana na Index stranici
                TempData["SuccessMessage"] = "GrupaMestaTroskova je uspešno obrisana.";

                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                var kvalifikacije = await webApiExecuter.InvokeGet<List<GrupaMestaTroskova>>("grupamestatroskova");
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

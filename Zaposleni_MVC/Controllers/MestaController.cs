using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Zaposleni_Clean_MVC_API.Data;
//using Zaposleni_Clean_MVC_API.Models;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    [Authorize(Roles = "Mesta")]
    public class MestaController : Controller
    {
        private readonly IWebApiExecuter webApiExecuter;

        public MestaController(IWebApiExecuter webApiExecuter)
        {
            this.webApiExecuter = webApiExecuter;
        }

        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        // Pozivanje API-ja za dobijanje liste mesta
        //        var mesta = await webApiExecuter.InvokeGet<List<Mesto>>("mesta"); // `SetAuthorizationHeader` se automatski poziva unutar `InvokeGet`
        //        return View(mesta);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Logovanje greške
        //        Console.WriteLine($"Error fetching data: {ex.Message}");
        //        return View(new List<Mesto>()); // Vraćanje prazne liste u slučaju greške
        //    }
        //}
        public async Task<IActionResult> Index()
        {
            // Provera da li postoji JWT token u sesiji
            var authCheck = CheckToken();
            if (authCheck != null) return authCheck;

            try
            {
                // Pozivanje API-ja za dobijanje liste mesta
                var mesta = await webApiExecuter.InvokeGet<List<Mesto>>("mesta");
                return View(mesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data: {ex.Message}");
                return View(new List<Mesto>());
            }
        }

        public IActionResult AddMesto()
        {
            // Provera da li postoji JWT token u sesiji
            var authCheck = CheckToken();
            if (authCheck != null) return authCheck;


            return View(new Mesto());
        }

        [HttpPost]
        public async Task<IActionResult> AddMesto(Mesto mesto)
        {
            // Provera da li postoji JWT token u sesiji
            var authCheck = CheckToken();
            if (authCheck != null) return authCheck;

            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za dodavanje novog mesta
                    var response = await webApiExecuter.InvokePost<Mesto>("mesta", mesto);

                    if (response != null)
                    {
                        TempData["SuccessMessage"] = "Mesto uspešno dodato!";
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

            return View(mesto); // Povratak na formu u slučaju greške
        }

        public async Task<IActionResult> UpdateMesto(int mestoId)
        {
            // Provera da li postoji JWT token u sesiji
            var authCheck = CheckToken();
            if (authCheck != null) return authCheck;

            try
            {
                // Pozivanje API-ja za dobijanje mesta po ID-ju
                var mesto = await webApiExecuter.InvokeGet<Mesto>($"mesta/{mestoId}");

                if (mesto == null)
                {
                    TempData["ErrorMessage"] = "Mesto nije pronađeno.";
                    return RedirectToAction(nameof(Index));
                }

                return View(mesto);
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMesto(Mesto mesto)
        {
            // Provera da li postoji JWT token u sesiji
            var authCheck = CheckToken();
            if (authCheck != null) return authCheck;


            if (ModelState.IsValid)
            {
                try
                {
                    // Pozivanje API-ja za ažuriranje mesta
                    await webApiExecuter.InvokePut($"mesta/{mesto.Id}", mesto);

                    TempData["SuccessMessage"] = "Mesto uspešno ažurirano!";
                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {
                    HandleWebApiException(ex);
                }
            }

            return View(mesto);
        }

        public async Task<IActionResult> DeleteMesto(int mestoId)
        {
            var authCheck = CheckToken(); // Provera da li postoji JWT token u sesiji
            if (authCheck != null) return authCheck;

            try
            {
                // Pozivanje API-ja za brisanje mesta
                await webApiExecuter.InvokeDelete($"mesta/{mestoId}");

                TempData["SuccessMessage"] = "Mesto uspešno obrisano!";
                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                var organJedinice = await webApiExecuter.InvokeGet<List<OrganizacioneJedinice>>("organizacionejedinice");
                return View(nameof(Index), organJedinice);
            }
        }
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

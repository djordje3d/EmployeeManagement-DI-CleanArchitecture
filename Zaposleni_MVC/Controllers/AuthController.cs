using Microsoft.AspNetCore.Mvc;
using Zaposleni_Clean_MVC_API.Models;
using System.Net.Http.Formatting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    public class AuthController : Controller
    {
        //public IActionResult Login(string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    return View();
        //}
        public IActionResult Login(string returnUrl = "/")
        {
            // Ako nema vrednosti za `returnUrl`, postavlja se na osnovnu stranicu
            ViewData["ReturnUrl"] = string.IsNullOrEmpty(returnUrl) || returnUrl == "/" ? null : returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            using (var client = new HttpClient())
            {
                var loginData = new
                {
                    Username = model.Username,
                    Password = model.Password
                };

                var response = await client.PostAsJsonAsync("https://localhost:7221/api/auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsAsync<dynamic>();
                    var token = (string)responseContent.token;

                    HttpContext.Session.SetString("JwtToken", token);

                    // Dohvati korisničke uloge iz API-ja
                    var rolesResponse = await client.GetAsync($"https://localhost:7221/api/auth/roles/{model.Username}");
                    var roles = rolesResponse.IsSuccessStatusCode ? await rolesResponse.Content.ReadAsAsync<List<string>>() : new List<string>();

                    // Kreiraj claim-ove za autentifikaciju
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role)); // Dodaj korisničke uloge u autentifikaciju
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    Console.WriteLine($"Login successful. ReturnUrl: {returnUrl}");
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var client = new HttpClient())
            {
                var registerData = new
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword
                };

                var response = await client.PostAsJsonAsync("https://localhost:7221/api/auth/register", registerData);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Registracija uspešna! Možete se prijaviti.";
                    return RedirectToAction("Login");
                }

                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    // Ako su vraćeni IdentityError objekti kao JSON
                    var errors = JsonConvert.DeserializeObject<IEnumerable<IdentityError>>(errorContent);

                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch
                {
                    // Ako je vraćen običan string (npr. "Passwords do not match")
                    ModelState.AddModelError(string.Empty, errorContent);
                }

                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout() // 
        {
            // Uklanjamo token iz sesije
            HttpContext.Session.Remove("JwtToken");

            // Opcionalno: Brišemo sve podatke iz sesije (ako je potrebno)
            // HttpContext.Session.Clear();

            // Preusmeravamo korisnika na stranicu za prijavu
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}

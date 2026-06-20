using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Filters;
using Zaposleni_Clean_MVC_API.UseCases.Interfaces;
using Zaposleni_Clean_MVC_API.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Dodaj MVC servise sa autentifikacionim filterom
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new TokenAuthenticationFilter()); // Dodavanje filtera za autentifikaciju
});

// Konfiguracija API klijenata za komunikaciju sa servisima
builder.Services.AddHttpClient("ZaposleniApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7221/api/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("AuthorityApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7221/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Konfiguracija autentifikacije i autorizacije
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // Putanja za login
        options.LogoutPath = "/Auth/Logout"; // Putanja za logout
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Putanja za blokiran pristup
        options.SlidingExpiration = true; // Automatsko produzenje sesije
    });


var supportedCultures = new[] { new CultureInfo("sr-RS") }; // Dodavanje podrške za lokalizaciju
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("sr-RS"); //
    options.SupportedCultures = supportedCultures; // Podržane kulture
    options.SupportedUICultures = supportedCultures; // Podržane UI kulture
});

//  Registracija servisa
//builder.Services.AddTransient<IWebApiExecuter, WebApiExecuter>();

builder.Services.AddScoped<ICrudOrganizacionaJedinicaUseCases, CrudOrganizacionaJedinicaUseCases>();
builder.Services.AddScoped<IWebApiExecuter, WebApiExecuter>();


builder.Services.AddHttpContextAccessor(); // Omogucava pristup HTTP kontekstu
builder.Services.AddSession(); //  Aktivacija sesija

var app = builder.Build();

// Konfiguracija lokalizacije
app.UseRequestLocalization();
// Konfiguracija middlewares-a
if (!app.Environment.IsDevelopment()) // Uklanja detaljne greške u produkciji
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Aktivacija autentifikacije i autorizacije (VAzNO: Redosled je bitan!)
app.UseAuthentication(); // Prvo se autentifikuje korisnik
app.UseAuthorization();  // Zatim se primenjuju prava pristupa

app.UseSession(); // Aktivacija sesija

// Mapiranje ruta
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

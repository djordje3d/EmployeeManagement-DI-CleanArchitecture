using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
//using Zaposleni_API_Auth.Data;
using Zaposleni_API_Auth.Services;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica.Interfaces;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica;
using Zaposleni_Blazor.UseCases.Sistematizacije.Interfaces;
using Zaposleni_Blazor.UseCases.Sistematizacije;
using Zaposleni_Blazor.UseCases.PluginInterfaces;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_API_Auth.Controllers;
using Zaposleni_API_Auth.Filters.ActionFilters;
using Zaposleni_API_Auth.Filters.ExceptionFilters;
using Zaposleni_Blazor.UseCases.ClanDomacinstva;
using Zaposleni_Blazor.UseCases.ClanDomacinstva.Interfaces;
using Zaposleni_Blazor.UseCases.Zaposleni.Interfaces;
using Zaposleni_Blazor.UseCases.Zaposleni;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;
using Zaposleni_Blazor.UseCases.Mesta;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;
using Zaposleni_Blazor.CoreBusiness.APICore;
using Zaposleni_Blazor.UseCases.Kvalifikacije;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// JSON serijalizacija – uklanja Reference cikluse i null vrednosti
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.MaxDepth = 64;
})
.AddXmlSerializerFormatters() // Dodaje podršku za XML format (opciono)
.AddControllersAsServices(); // Omoguæava inject-ovanje kontrolera

// EF Core DbContext
builder.Services.AddDbContext<Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ZaposleniConnection"));
});

// Identity konfiguracija
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("WritePolicy", policy => policy.RequireClaim("WriteAccess", "true"));
    options.AddPolicy("DeletePolicy", policy => policy.RequireClaim("DeleteAccess", "true"));
    options.AddPolicy("ReadPolicy", policy => policy.RequireClaim("ReadAccess", "true"));
});

// JWT konfiguracija
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// UseCase i Repository DI
builder.Services.AddScoped<ICrudOrganizacionaJedinicaUseCases, CrudOrganizacionaJedinicaUseCases>();
builder.Services.AddScoped<IOrganizacionaJedinicaRepository, OrgJediniceEFCoreRepository>();

builder.Services.AddScoped<ICrudSistematizacijaUseCases, CrudSistematizacijaUseCases>();
builder.Services.AddScoped<ISistematizacijaRepository, SistematizacijaEFCoreRepository>();

builder.Services.AddScoped<ICrudClanoviDomacinstvaUseCases, CrudClanoviDomacinstvaUseCases>();
builder.Services.AddScoped<IClanDomacinstvaRepository, ClanoviDomacinstvaEFCoreRepository>();

builder.Services.AddScoped<ICrudZaposlenUseCases, CrudZaposlenUseCases>();
builder.Services.AddScoped<IZaposlenRepository, ZaposlenEFCoreRepository>();

builder.Services.AddScoped<IAddGrupaMestaTroskaUseCase, AddGrupaMestaTroskaUseCase>();
builder.Services.AddScoped<IGrupaMestaTroskaByIdUseCase, GrupaMestaTroskaByIdUseCase>();
builder.Services.AddScoped<IEditGrupaMestaTroskaUseCase, EditGrupaMestaTroskaUseCase>();
builder.Services.AddScoped<IDeleteGrupaMestaTroskaUseCase, DeleteGrupaMestaTroskaUseCase>();
builder.Services.AddScoped<IListGrupaMestaTroskaUseCase, ListGrupaMestaTroskaUseCase>();
builder.Services.AddScoped<IGrupaMestaTroskaRepository, GrupaMestaTroskaEFCoreRepository>();

builder.Services.AddScoped<IAddMestoUseCase, AddMestoUseCase>();
builder.Services.AddScoped<IMestoByIdUseCase, MestoByIdUseCase>();
builder.Services.AddScoped<IEditMestoUseCase, EditMestoUseCase>();
builder.Services.AddScoped<IDeleteMestoUseCase, DeleteMestoUseCase>();
builder.Services.AddScoped<IListMestoUseCase, ListMestoUseCase>();
builder.Services.AddScoped<IMestoRepository, MestoEFCoreRepository>();

builder.Services.AddScoped<IAddKvalifikacijaUseCase, AddKvalifikacijaUseCase>();
builder.Services.AddScoped<IKvalifikacijaByIdUseCase, KvalifikacijaByIdUseCase>();
builder.Services.AddScoped<IEditKvalifikacijaUseCase, EditKvalifikacijaUseCase>();
builder.Services.AddScoped<IDeleteKvalifikacijaUseCase, DeleteKvalifikacijaUseCase>();
builder.Services.AddScoped<IListKvalifikacijaUseCase, ListKvalifikacijaUseCase>();
builder.Services.AddScoped<IKvalifikacijaRepository, KvalifikacijaEFCoreRepository>();

// Action i Exception filteri
builder.Services.AddScoped<OJ_ValidateOJ_IdFilterAttribute>();
builder.Services.AddScoped<OJ_ValidateAdd_OJFilterAttribute>();
builder.Services.AddScoped<OJ_ValidateUpdateOJFilterAttributeOsnovni>();
builder.Services.AddScoped<OJ_HandleUpdateExceptionsFilterAttribute>();

builder.Services.AddScoped<Sistematizacija_ValidateSistematizacijaIdFilterAttribute>();
builder.Services.AddScoped<Sistematizacija_ValidateAddSistematizacijaFilterAttribute>();
builder.Services.AddScoped<Sistematizacija_ValidateUpdateSistematizacijaFilterAttribute>();
builder.Services.AddScoped<Sistematizacija_HandleUpdateExceptionsFilterAttribute>();

builder.Services.AddScoped<ClanoviDomacinstva_ValidateClanoviIdFilterAttribute>();
builder.Services.AddScoped<ClanoviDomacinstva_ValidateAddClanFilterAttribute>();
builder.Services.AddScoped<ClanDom_ValidateUpdateClanDomFilterAttribute>();
builder.Services.AddScoped<ClanoviDomacinstvaHandleUpdateExceptionsFilterAttribute>();

builder.Services.AddScoped<Zaposlen_ValidateZaposlenIdFilterAttribute>();
builder.Services.AddScoped<Zaposlen_ValidateAddZaposlenFilterAttribute>();
builder.Services.AddScoped<Zaposlen_ValidateUpdateZaposlenFilterAttributeOsnovni>();
builder.Services.AddScoped<Zaposlen_HandleUpdateExceptionsFilterAttribute>();

builder.Services.AddScoped<GrMestaTroska_ValidateGrMestaTroskaIdFilterAttribute>();
builder.Services.AddScoped<GrMestaTroska_ValidateAdd_GrMTFilterAttribute>();
builder.Services.AddScoped<GrMestaTroska_ValidateUpdateGrMTFilterAttribute>();
builder.Services.AddScoped<GrMestaTroska_HandleUpdateExceptionsFilterAttribute>();

builder.Services.AddScoped<Mesto_ValidateMestoIdFilterAttribute>();
builder.Services.AddScoped<Mesto_ValidateAddMestoFilterAttribute>();
builder.Services.AddScoped<Mesto_ValidateUpdateMestoFilterAttribute>();
builder.Services.AddScoped<Mesto_HandleUpdateExceptionsFilterAttribute>();

builder.Services.AddScoped<Kvalifikacija_ValidateKvalifikacijaIdFilterAttribute>();
builder.Services.AddScoped<Kvalifikacija_ValidateAddKvalifikacijaFilterAttribute>();
builder.Services.AddScoped<Kvalifikacija_ValidateUpdateKvalifikacijaFilterAttribute>();
builder.Services.AddScoped<Kvalifikacija_HandleUpdateExceptionsFilterAttribute>();

// Permission servis
builder.Services.AddScoped<PermissionService>();

// Build aplikaciju
var app = builder.Build();

// Exception middleware (globalni handler)
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            var response = new
            {
                error = exception.Message,
                stackTrace = exception.StackTrace
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

// Test konekcije, kreiranje uloga i test registracije servisa
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
    var authController = scope.ServiceProvider.GetService<AuthController>();

    // Test konekcije sa bazom
    try
    {
        dbContext.Database.CanConnect();
        Console.WriteLine("Uspešna konekcija sa bazom.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Greška u konekciji: {ex.Message}");
    }

    // Kreiranje osnovnih uloga ako ne postoje
    var roles = new[] { "CanAccessZaposleni", "CanAccessMesta" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
            Console.WriteLine($"Kreirana uloga: {role}");
        }
    }

    // Provera UserManager servisa
    Console.WriteLine(userManager != null
        ? "UserManager je uspešno registrovan u DI."
        : "UserManager NIJE registrovan u DI!");

    // Provera AuthController-a
    Console.WriteLine(authController != null
        ? "AuthController se uspešno kreira!"
        : "AuthController NIJE registrovan u DI!");
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

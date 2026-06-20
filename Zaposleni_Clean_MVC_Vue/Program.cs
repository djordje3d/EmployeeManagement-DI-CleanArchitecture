using Zaposleni_Blazor.UseCases.Mesta.Interfaces;
using Zaposleni_Blazor.UseCases.Mesta;
using Zaposleni_Blazor.UseCases.PluginInterfaces;
using Zaposleni.Plugins.EFCoreSqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.UseCases.ClanDomacinstva.Interfaces;
using Zaposleni_Blazor.UseCases.ClanDomacinstva;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces;
using Zaposleni_Blazor.UseCases.GrupeMestaTroskova;
using Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces;
using Zaposleni_Blazor.UseCases.Kvalifikacije;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica.Interfaces;
using Zaposleni_Blazor.UseCases.OrganizacionaJedinica;
using Zaposleni_Blazor.UseCases.Sistematizacije.Interfaces;
using Zaposleni_Blazor.UseCases.Sistematizacije;
using Zaposleni_Blazor.UseCases.Zaposleni.Interfaces;
using Zaposleni_Blazor.UseCases.Zaposleni;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Test the connection string
try
{
    using (var connection = new SqlConnection(connectionString))
    {
        connection.Open();
        Console.WriteLine("Connection successful!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Connection failed: {ex.Message}");
    throw; // Rethrow to stop the application if the connection fails.
}

// Add DbContext with connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IMestoRepository, MestoEFCoreRepository>();
builder.Services.AddTransient<IAddMestoUseCase, AddMestoUseCase>();
builder.Services.AddTransient<IListMestoUseCase, ListMestoUseCase>();
builder.Services.AddTransient<IEditMestoUseCase, EditMestoUseCase>();
builder.Services.AddTransient<IDeleteMestoUseCase, DeleteMestoUseCase>();
builder.Services.AddTransient<IMestoByIdUseCase, MestoByIdUseCase>();

builder.Services.AddScoped<IKvalifikacijaRepository, KvalifikacijaEFCoreRepository>();

builder.Services.AddTransient<IAddKvalifikacijaUseCase, AddKvalifikacijaUseCase>();
builder.Services.AddTransient<IListKvalifikacijaUseCase, ListKvalifikacijaUseCase>();
builder.Services.AddTransient<IEditKvalifikacijaUseCase, EditKvalifikacijaUseCase>();
builder.Services.AddTransient<IDeleteKvalifikacijaUseCase, DeleteKvalifikacijaUseCase>();
builder.Services.AddTransient<IKvalifikacijaByIdUseCase, KvalifikacijaByIdUseCase>();


builder.Services.AddScoped<IGrupaMestaTroskaRepository, GrupaMestaTroskaEFCoreRepository>();

builder.Services.AddTransient<IAddGrupaMestaTroskaUseCase, AddGrupaMestaTroskaUseCase>();
builder.Services.AddTransient<IListGrupaMestaTroskaUseCase, ListGrupaMestaTroskaUseCase>();
builder.Services.AddTransient<IEditGrupaMestaTroskaUseCase, EditGrupaMestaTroskaUseCase>();
builder.Services.AddTransient<IDeleteGrupaMestaTroskaUseCase, DeleteGrupaMestaTroskaUseCase>();
builder.Services.AddTransient<IGrupaMestaTroskaByIdUseCase, GrupaMestaTroskaByIdUseCase>();

builder.Services.AddScoped<IOrganizacionaJedinicaRepository, OrgJediniceEFCoreRepository>();
builder.Services.AddTransient<ICrudOrganizacionaJedinicaUseCases, CrudOrganizacionaJedinicaUseCases>();

builder.Services.AddScoped<ISistematizacijaRepository, SistematizacijaEFCoreRepository>();
builder.Services.AddTransient<ICrudSistematizacijaUseCases, CrudSistematizacijaUseCases>();

builder.Services.AddScoped<IZaposlenRepository, ZaposlenEFCoreRepository>();
builder.Services.AddTransient<ICrudZaposlenUseCases, CrudZaposlenUseCases>();

builder.Services.AddScoped<IClanDomacinstvaRepository, ClanoviDomacinstvaEFCoreRepository>();
builder.Services.AddTransient<ICrudClanoviDomacinstvaUseCases, CrudClanoviDomacinstvaUseCases>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

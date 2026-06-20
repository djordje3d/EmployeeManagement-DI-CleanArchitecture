using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces
{
    public interface IEditKvalifikacijaUseCase
    {
        Task<bool> ExecuteAsync(Kvalifikacija kvalifikacija);
    }
}

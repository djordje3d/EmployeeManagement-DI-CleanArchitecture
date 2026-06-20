using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces
{
    public interface IAddKvalifikacijaUseCase
    {
        Task<bool> ExecuteAsync(Kvalifikacija kvalifikacija);
    }
}

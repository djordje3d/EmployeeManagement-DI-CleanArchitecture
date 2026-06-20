using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces
{
    public interface IListKvalifikacijaUseCase
    {
        Task<IEnumerable<Kvalifikacija>> ExecuteAsync();
    }
}
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces
{
    public interface IKvalifikacijaByIdUseCase
    {
        Task<Kvalifikacija?> ExecuteAsync(int kvalifikacijaId);
    }
}

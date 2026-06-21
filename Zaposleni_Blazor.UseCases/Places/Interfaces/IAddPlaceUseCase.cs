using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Places.Interfaces
{
    public interface IAddPlaceUseCase
    {
        Task<bool> ExecuteAsync(Mesto mesto);
    }
}

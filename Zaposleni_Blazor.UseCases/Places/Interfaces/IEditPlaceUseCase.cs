using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Places.Interfaces
{
    public interface IEditPlaceUseCase
    {
        Task<bool> ExecuteAsync(Mesto mesto);
    }
}

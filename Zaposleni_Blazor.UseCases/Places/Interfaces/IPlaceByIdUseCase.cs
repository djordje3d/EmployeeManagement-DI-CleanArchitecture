using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Places.Interfaces
{
    public interface IPlaceByIdUseCase
    {
        Task<Mesto?> ExecuteAsync(int mestoId);
    }
}

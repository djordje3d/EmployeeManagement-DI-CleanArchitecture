using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Mesta.Interfaces
{
    public interface IEditMestoUseCase
    {
        Task<bool> ExecuteAsync(Mesto mesto);
    }
}

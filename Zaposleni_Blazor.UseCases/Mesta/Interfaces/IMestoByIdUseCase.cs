using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Mesta.Interfaces
{
    public interface IMestoByIdUseCase
    {
        Task<Mesto?> ExecuteAsync(int mestoId);
    }
}

using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces
{
    public interface IListGrupaMestaTroskaUseCase
    {
        Task<IEnumerable<GrupaMestaTroskova>> ExecuteAsync();
    }
}
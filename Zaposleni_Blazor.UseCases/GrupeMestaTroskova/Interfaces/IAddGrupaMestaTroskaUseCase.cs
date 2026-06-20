using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces
{
    public interface IAddGrupaMestaTroskaUseCase
    {
        Task<bool> ExecuteAsync(GrupaMestaTroskova grupaMestaTroskova);
    }
}

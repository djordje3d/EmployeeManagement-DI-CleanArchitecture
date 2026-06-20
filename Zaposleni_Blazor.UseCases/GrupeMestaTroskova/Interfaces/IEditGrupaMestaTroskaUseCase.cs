using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces
{
    public interface IEditGrupaMestaTroskaUseCase
    {
        Task<bool> ExecuteAsync(GrupaMestaTroskova grupaMestaTroskova);
    }
}

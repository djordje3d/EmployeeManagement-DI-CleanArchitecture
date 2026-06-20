using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces
{
    public interface IGrupaMestaTroskaByIdUseCase
    {
        Task<GrupaMestaTroskova?> ExecuteAsync(int grupaMestaTroskaId);
    }
}

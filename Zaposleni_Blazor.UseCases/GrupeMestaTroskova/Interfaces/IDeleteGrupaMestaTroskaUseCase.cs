using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.UseCases.GrupeMestaTroskova.Interfaces
{
    public interface IDeleteGrupaMestaTroskaUseCase
    {
        Task<bool> ExecuteAsync(int id);
    }
}

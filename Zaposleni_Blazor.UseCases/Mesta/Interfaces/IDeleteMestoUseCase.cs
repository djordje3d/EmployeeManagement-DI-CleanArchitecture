using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.UseCases.Mesta.Interfaces
{
    public interface IDeleteMestoUseCase
    {
        Task<bool> ExecuteAsync(int id);
    }
}

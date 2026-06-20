using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaposleni_Blazor.CoreBusiness;

namespace Zaposleni_Blazor.UseCases.Mesta.Interfaces
{
    public interface IListMestoUseCase
    {
        Task<IEnumerable<Mesto>> ExecuteAsync();
    }
}

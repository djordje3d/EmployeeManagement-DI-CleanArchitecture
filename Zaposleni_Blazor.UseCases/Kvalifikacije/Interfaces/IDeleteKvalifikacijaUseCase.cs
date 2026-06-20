using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.UseCases.Kvalifikacije.Interfaces
{
    public interface IDeleteKvalifikacijaUseCase
    {
        Task<bool> ExecuteAsync(int id);
    }
}

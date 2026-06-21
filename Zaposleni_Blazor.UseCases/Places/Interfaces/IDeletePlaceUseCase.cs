using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.UseCases.Places.Interfaces
{
    public interface IDeletePlaceUseCase
    {
        Task<bool> ExecuteAsync(int id);
    }
}

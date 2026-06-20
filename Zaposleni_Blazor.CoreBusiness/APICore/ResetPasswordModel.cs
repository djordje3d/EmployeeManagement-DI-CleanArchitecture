using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class ResetPasswordModel
    {
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
}

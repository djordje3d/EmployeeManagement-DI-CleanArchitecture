using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class AssignRolesDto
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}

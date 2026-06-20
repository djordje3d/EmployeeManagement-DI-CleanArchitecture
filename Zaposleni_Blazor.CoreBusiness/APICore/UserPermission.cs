using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class UserPermission
    {
        public string UserId { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public ApplicationUser User { get; set; }
        public Permission Permission { get; set; }

        // su 100% ispravni i služe da:
        // Permission definiše jednu opciju/pravo(npr. "CanAccessZaposleni", "CanEditCategory", "CanDeleteUser", itd.)
        // UserPermission povezuje koji korisnik ima koje pravo(Many-to-Many relacija između ApplicationUser i Permission)
    }
}

using System.Runtime.Intrinsics.X86;
using System.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Zaposleni_API_Auth.Models
{
    public class UserPermission
    {
        public string UserId { get; set; }
        public int PermissionId { get; set; }
        public ApplicationUser User { get; set; }
        public Permission Permission { get; set; }

        // su 100% ispravni i služe da:
        // Permission definiše jednu opciju/pravo(npr. "CanAccessZaposleni", "CanEditCategory", "CanDeleteUser", itd.)
        // UserPermission povezuje koji korisnik ima koje pravo(Many-to-Many relacija između ApplicationUser i Permission)
    }
}
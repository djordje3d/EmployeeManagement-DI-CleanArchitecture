using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }

        public ICollection<UserPermission>? UserPermissions { get; set; }
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}

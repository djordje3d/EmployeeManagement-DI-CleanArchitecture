using Microsoft.AspNetCore.Identity;

namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class RolePermission
    {
        public int Id { get; set; }

        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}

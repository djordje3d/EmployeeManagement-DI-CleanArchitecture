using Microsoft.AspNetCore.Identity;


namespace Zaposleni_Blazor.CoreBusiness.APICore
{
    public class ApplicationUser : IdentityUser
    {
        // Dodatna polja po potrebi, npr:
        // public string FullName { get; set; }

        // Navigacija
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}

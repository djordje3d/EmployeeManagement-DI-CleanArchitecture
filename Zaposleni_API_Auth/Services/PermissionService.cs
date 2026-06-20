using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;
using Zaposleni.Plugins.EFCoreSqlServer;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.CoreBusiness.APICore;

namespace Zaposleni_API_Auth.Services
{
    public class PermissionService
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public PermissionService(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            _userManager = userManager;
        }

        // 1. Dodeli Permission korisniku
        public async Task AssignPermissionToUserAsync(string userId, int permissionId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Korisnik nije pronađen.");

            var permission = await db.Permissions.FindAsync(permissionId);
            if (permission == null)
                throw new Exception("Permission nije pronađen.");

            if (!db.UserPermissions.Any(up => up.UserId == userId && up.PermissionId == permissionId))
            {
                db.UserPermissions.Add(new UserPermission
                {
                    UserId = userId,
                    PermissionId = permissionId
                });

                await db.SaveChangesAsync();
            }
        }


        // 2. Proveri da li korisnik ima Permission
        public async Task<bool> UserHasPermissionAsync(string userId, string permissionName)
        {
            return await db.UserPermissions
                .Include(up => up.Permission)
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permissionName);
        }

        // 3. Vrati listu Permissions za korisnika
        public async Task<List<string>> GetPermissionsForUserAsync(string userId)
        {
            return await db.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission.Name)
                .ToListAsync();
        }

        public async Task<bool> UserHasPermissionIncludingRolesAsync(string userId, string permissionName)
        {
            // Prvo proveravamo direktno dodeljena prava
            var hasDirectPermission = await db.UserPermissions
                .Include(up => up.Permission)
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permissionName);

            if (hasDirectPermission)
                return true;

            // Dohvata sve uloge korisnika
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count == 0) return false;

            // Proveravamo da li neka od uloga ima pravo preko RolePermission
            var hasRolePermission = await db.RolePermissions
                .Include(rp => rp.Permission)
                .Include(rp => rp.Role)
                .AnyAsync(rp => roles.Contains(rp.Role.Name) && rp.Permission.Name == permissionName);

            return hasRolePermission;
        }

    }
}

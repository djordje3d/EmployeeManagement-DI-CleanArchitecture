using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Data;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.CoreBusiness.APICore;

namespace Zaposleni_API_Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPermissionsApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext _context;

        public UserPermissionsApiController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, Zaposleni.Plugins.EFCoreSqlServer.ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        // Dodela uloge korisniku
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                return BadRequest("Role does not exist");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        //[HttpPost("assign-multiple")]
        //public async Task<IActionResult> AssignMultipleRoles([FromBody] AssignRolesDto model)
        //{
        //    var user = await _userManager.FindByIdAsync(model.UserId);
        //    if (user == null) 
        //        return NotFound("Korisnik nije pronađen.");

        //    var result = await _userManager.AddToRolesAsync(user, model.Roles);

        //    if (!result.Succeeded)
        //        return BadRequest(result.Errors);

        //    return Ok(new { Message = "Uloge uspešno dodeljene korisniku." });
        //}

        [HttpPost("assign-multiple")]
        public async Task<IActionResult> AssignMultipleRoles([FromBody] AssignRolesDto model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null) 
                return NotFound("Korisnik nije pronađen.");

            // Proveravamo da li sve uloge postoje pre nego što ih dodelimo
            foreach (var role in model.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return BadRequest($"Uloga '{role}' ne postoji.");
                }
            }

            var result = await _userManager.AddToRolesAsync(user, model.Roles);
            // Ako dodeljivanje nije uspelo, vraćamo greške
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { Message = "Uloge uspešno dodeljene korisniku." });
        }

        // Dodela specifičnih prava korisniku
        [HttpPost("assign-permission")]
        public async Task<IActionResult> AssignPermissionToUser([FromBody] AssignPermissionModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var userPermission = new UserPermission
            {
                UserId = model.UserId,
                PermissionId = model.PermissionId
            };

            _context.UserPermissions.Add(userPermission);
            await _context.SaveChangesAsync();

            return Ok();
        }



    }
}

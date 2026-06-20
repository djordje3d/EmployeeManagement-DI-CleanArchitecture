using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zaposleni_Blazor.CoreBusiness;

//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness.APICore;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Dodela uloge korisniku
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound($"Korisnik sa ID '{request.UserId}' nije pronađen.");
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                return NotFound($"Uloga '{request.Role}' ne postoji.");
            }

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (result.Succeeded)
            {
                return Ok(new { Success = true, Message = $"Uloga '{request.Role}' je uspešno dodeljena korisniku '{user.UserName}'." });
            }

            return BadRequest(new { Success = false, Message = "Greška prilikom dodele uloge." });
        }

        // Dohvat svih uloga
        [HttpGet("AllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();

            return Ok(roles);
        }

        // Kreiranje nove uloge
        //[HttpPost("CreateRole")]
        //public async Task<IActionResult> CreateRole([FromBody] string roleName)
        //{
        //    if (string.IsNullOrEmpty(roleName))
        //    {
        //        return BadRequest(new { Success = false, Message = "Naziv uloge je obavezan." });
        //    }

        //    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        //    if (result.Succeeded)
        //    {
        //        return Ok(new { Success = true, Message = $"Uloga '{roleName}' je uspešno kreirana." });
        //    }

        //    return BadRequest(new { Success = false, Message = "Greška prilikom kreiranja uloge." });
        //}

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                return BadRequest(new ApiResponseDto
                {
                    Success = false,
                    Message = "Naziv uloge je obavezan."
                });
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(request.Name));
            if (result.Succeeded)
            {
                return Ok(new ApiResponseDto
                {
                    Success = true,
                    Message = $"Uloga '{request.Name}' je uspešno kreirana."
                });
            }

            return BadRequest(new ApiResponseDto
            {
                Success = false,
                Message = "Greška prilikom kreiranja uloge."
            });
        }



        // Ažuriranje postojeće uloge
        [HttpPut("UpdateRole/{roleId}")]
        public async Task<IActionResult> UpdateRole(string roleId, [FromBody] string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { Success = false, Message = $"Uloga sa ID '{roleId}' nije pronađena." });
            }

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { Success = true, Message = $"Uloga '{newRoleName}' je uspešno ažurirana." });
            }

            return BadRequest(new { Success = false, Message = "Greška prilikom ažuriranja uloge." });
        }

        // Brisanje uloge
        [HttpDelete("DeleteRole/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound(new { Success = false, Message = $"Uloga sa ID '{roleId}' nije pronađena." });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { Success = true, Message = $"Uloga '{role.Name}' je uspešno obrisana." });
            }

            return BadRequest(new { Success = false, Message = "Greška prilikom brisanja uloge." });
        }

        // Dohvat svih korisnika

        [HttpGet("AllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.AsNoTracking().ToListAsync(); // AsNoTracking sprečava konkurentne pristupe DbContext-u

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Svaki korisnik sada dohvata uloge zasebno

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList() // Ispravljen tip liste
                });
            }

            return Ok(userDtos);
        }


        // Ažuriranje korisnika
        [HttpPut("UpdateUser/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Success = false, Message = $"Korisnik sa ID '{userId}' nije pronađen." });
            }

            user.Email = request.Email;
            user.UserName = request.UserName;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Success = true, Message = $"Korisnik '{request.UserName}' je uspešno ažuriran." });
            }

            return BadRequest(new { Success = false, Message = "Greška prilikom ažuriranja korisnika." });
        }

        // Brisanje korisnika
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Success = false, Message = $"Korisnik sa ID '{userId}' nije pronađen." });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Success = true, Message = $"Korisnik '{user.UserName}' je uspešno obrisan." });
            }

            return BadRequest(new { Success = false, Message = "Greška prilikom brisanja korisnika." });
        }

        // Dodela više uloga korisniku
        [HttpGet("GetUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Korisnik nije pronađen.");

            var roles = await _userManager.GetRolesAsync(user); // Dohvati sve uloge korisnika
            return Ok(roles);
        }

        [HttpPost("RemoveRoles/{userId}")]
        public async Task<IActionResult> RemoveRoles(string userId, [FromBody] List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Korisnik nije pronađen.");

            await _userManager.RemoveFromRolesAsync(user, roles);
            return Ok(new ApiResponseDto { Success = true, Message = "Uloge uspešno uklonjene." });
        }

        [HttpPost("AssignRoles/{userId}")]
        public async Task<IActionResult> AssignRoles(string userId, [FromBody] List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("Korisnik nije pronađen.");

            await _userManager.AddToRolesAsync(user, roles);
            return Ok(new ApiResponseDto { Success = true, Message = "Uloge uspešno dodeljene." });
        }


    }

}

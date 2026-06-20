using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_API_Auth.Services;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly PermissionService _permissionService;

        public PermissionsController(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignPermission(string userId, int permissionId)
        {
            await _permissionService.AssignPermissionToUserAsync(userId, permissionId);
            return Ok("Permission dodeljen korisniku.");
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckPermission(string userId, string permissionName)
        {
            var hasPermission = await _permissionService.UserHasPermissionAsync(userId, permissionName);
            return Ok(hasPermission);
        }
    }
}

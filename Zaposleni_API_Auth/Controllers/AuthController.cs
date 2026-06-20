using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
//using Zaposleni_API_Auth.Models;
using Zaposleni_Blazor.CoreBusiness.APICore;

namespace Zaposleni_API_Auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager; // Ovaj objekat se koristi za upravljanje korisnicima
//            this.signInManager = signInManager; // Ovaj objekat se koristi za upravljanje prijavom korisnika
            this.configuration = configuration; // Ovaj objekat se koristi za pristup konfiguraciji aplikacije
        }

        // Endpoint za registraciju
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model.Password != model.ConfirmPassword)
                return BadRequest("Passwords do not match");

            if (await userManager.FindByNameAsync(model.Username) != null)
                return BadRequest("Username already exists");

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null || !(await userManager.CheckPasswordAsync(user, model.Password)))
                return Unauthorized("Invalid username or password");

            // Dohvatanje uloga iz baze
            var userRoles = await userManager.GetRolesAsync(user);

            // Osnovni claimovi
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("ReadAccess", "true"),
        new Claim("DeleteAccess", "true"),
        new Claim("WriteAccess", "true")
    };

            // Dodavanje claimova na osnovu uloga iz baze
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // <--- ključno za [Authorize(Roles = "...")]
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken( // kreiranje tokena
                issuer: configuration["Jwt:Issuer"], // izdavač
                audience: configuration["Jwt:Audience"], // publika
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds  // potpisivanje tokena
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expires = token.ValidTo,
                username = user.UserName,
                expiresAt = token.ValidTo.ToString("dd.MM.yyyy HH:mm:ss"),
                roles = userRoles // <-- konkretne uloge korisnika
            });
        }

        [HttpPost("reset-password")]
        [AllowAnonymous] // ili [Authorize(Roles = "Admin")] kasnije ako želiš da zaštitiš
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model) // 
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
                return NotFound("Korisnik nije pronađen.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            return Ok("Lozinka uspešno resetovana.");
        }

        [HttpGet("roles/{username}")] // Endpoint za dobijanje uloga korisnika
        [AllowAnonymous]
        public async Task<IActionResult> GetUserRoles(string username) // Endpoint za dobijanje uloga korisnika
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("Korisnik nije pronađen.");

            var roles = await userManager.GetRolesAsync(user);
            return Ok(roles);
        }

    }
}

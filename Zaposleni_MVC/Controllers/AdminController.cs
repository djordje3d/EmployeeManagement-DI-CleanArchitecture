using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zaposleni_Clean_MVC_API.Data;
using Zaposleni_Clean_MVC_API.Models;

namespace Zaposleni_Clean_MVC_API.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IWebApiExecuter _webApiExecuter;

        public AdminController(IWebApiExecuter webApiExecuter)
        {
            _webApiExecuter = webApiExecuter;
        }

        // Dodela uloge korisniku
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDto assignRoleDto)
        {
            if (string.IsNullOrEmpty(assignRoleDto.UserId) || string.IsNullOrEmpty(assignRoleDto.Role))
            {
                TempData["ErrorMessage"] = "ID korisnika i naziv uloge su obavezni.";
                return RedirectToAction("ManageRoles");
            }

            try
            {
                var response = await _webApiExecuter.InvokePost<AssignRoleDto, ApiResponseDto>("role/AssignRole", assignRoleDto);

                if (response != null && response.Success)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Uloga uspešno dodeljena.";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom dodele uloge.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction("ManageRoles");
        }

        //[HttpPost]
        //public async Task<IActionResult> AssignRolesToUser(string userId, List<string> roles)
        //{
        //    if (string.IsNullOrEmpty(userId) || roles == null || roles.Count == 0)
        //    {
        //        TempData["ErrorMessage"] = "ID korisnika i bar jedna uloga su obavezni.";
        //        return RedirectToAction("ManageUsers");
        //    }

        //    // ❌ Sprečavanje dodele Admin uloge slučajnim korisnicima
        //    if (roles.Contains("Admin") && !User.IsInRole("SuperAdmin"))
        //    {
        //        TempData["ErrorMessage"] = "Nemate dozvolu za dodelu Admin uloge.";
        //        return RedirectToAction("ManageUsers");
        //    }

        //    try
        //    {
        //        var assignRolesDto = new AssignRolesDto { UserId = userId, Roles = roles };
        //        var response = await _webApiExecuter.InvokePost<AssignRolesDto, ApiResponseDto>("UserPermissionsApi/assign-multiple", assignRolesDto);

        //        if (response != null && response.Success)
        //        {
        //            TempData["SuccessMessage"] = response.Message ?? "Uloge uspešno dodeljene.";
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom dodele uloga.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Greška: {ex.Message}";
        //    }

        //    return RedirectToAction("ManageUsers");
        //}

        [HttpPost]
        public async Task<IActionResult> AssignRolesToUser(string userId, List<string> roles)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "ID korisnika je obavezan.";
                return RedirectToAction("ManageUsers");
            }

            try
            {
                // ✅ Dohvati postojeće uloge korisnika preko API-ja
                var existingRoles = await _webApiExecuter.InvokeGet<List<string>>($"role/GetUserRoles/{userId}");

                // ❌ Sprečavanje dodele Admin uloge običnim korisnicima
                if (roles.Contains("Admin") && !User.IsInRole("SuperAdmin"))
                {
                    TempData["ErrorMessage"] = "Nemate dozvolu za dodelu Admin uloge.";
                    return RedirectToAction("ManageUsers");
                }

                // ❌ Korak 1: Ukloni samo one uloge koje su odčekirane
                var rolesToRemove = existingRoles.Except(roles ?? new List<string>()).ToList();
                if (rolesToRemove.Count > 0)
                {
                    await _webApiExecuter.InvokePost<List<string>, ApiResponseDto>($"role/RemoveRoles/{userId}", rolesToRemove);
                }

                // ✅ Korak 2: Dodaj samo nove uloge koje nedostaju
                var rolesToAdd = (roles ?? new List<string>()).Except(existingRoles).ToList();
                if (rolesToAdd.Count > 0)
                {
                    await _webApiExecuter.InvokePost<List<string>, ApiResponseDto>($"role/AssignRoles/{userId}", rolesToAdd);
                }

                TempData["SuccessMessage"] = "Uloge uspešno ažurirane.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction("ManageUsers");
        }

        // Prikaz svih uloga
        public async Task<IActionResult> ManageRoles()
        {
            try
            {
                var roles = await _webApiExecuter.InvokeGet<List<RoleDto>>("role/AllRoles");
                return View(roles);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom dohvatanja uloga: {ex.Message}";
                return RedirectToAction("Error", "Home");
            }
        }

        

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                TempData["ErrorMessage"] = "Naziv uloge je obavezan.";
                return RedirectToAction("ManageRoles");
            }

            try
            {
                // Slanje zahteva API-ju sa anonimnim objektom { roleName }
                var response = await _webApiExecuter.InvokePost<object, ApiResponseDto>("role/CreateRole", new { Name = roleName });

                if (response != null && response.Success)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Uloga uspešno kreirana.";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom kreiranja uloge.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška prilikom kreiranja uloge: {ex.Message}";
            }

            return RedirectToAction("ManageRoles"); // Vrati se na stranicu za upravljanje ulogama
        }


        // Ažuriranje postojeće uloge
        [HttpPost]
        public async Task<IActionResult> UpdateRole(string roleId, string newRoleName)
        {
            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(newRoleName))
            {
                TempData["ErrorMessage"] = "ID uloge i naziv su obavezni.";
                return RedirectToAction("ManageRoles");
            }

            try
            {
                var response = await _webApiExecuter.InvokePut<string, ApiResponseDto>($"role/UpdateRole/{roleId}", newRoleName);

                if (response != null && response.Success)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Uloga uspešno ažurirana.";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom ažuriranja uloge.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction("ManageRoles");
        }

        // Brisanje uloge
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId, string roleName)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                TempData["ErrorMessage"] = "ID uloge je obavezan.";
                return RedirectToAction("ManageRoles");
            }

            try
            {
                var response = await _webApiExecuter.InvokeDelete<ApiResponseDto>($"role/DeleteRole/{roleId}");

                if (response != null && response.Success)
                {
                    TempData["SuccessMessage"] = $"Uloga '{roleName}' je uspešno obrisana.";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom brisanja uloge.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction("ManageRoles");
        }


        // Prikaz svih korisnika
        //public async Task<IActionResult> ManageUsers()
        //{
        //    try
        //    {
        //        var users = await _webApiExecuter.InvokeGet<List<UserDto>>("role/AllUsers");
        //        return View(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = $"Greška prilikom dohvatanja korisnika: {ex.Message}";
        //        return RedirectToAction("Error", "Home");
        //    }
        //}

        public async Task<IActionResult> ManageUsers()
        {
            try
            {
                var users = await _webApiExecuter.InvokeGet<List<UserDto>>("role/AllUsers"); // Dohvati sve korisnike
                var roles = await _webApiExecuter.InvokeGet<List<RoleDto>>("role/AllRoles"); // Dohvati sve uloge

                foreach (var user in users)
                {
                    var userRoles = await _webApiExecuter.InvokeGet<List<string>>($"role/GetUserRoles/{user.Id}");
                    user.Roles = userRoles; // Dodaj uloge koje korisnik već ima
                }

                ViewBag.Roles = roles; // Omogućava dinamički prikaz uloga u checkbox poljima

                return View(users);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
                return RedirectToAction("Error", "Home");
            }
        }


        // Ažuriranje korisnika
        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserDto updateUserDto)
        {
            if (string.IsNullOrEmpty(updateUserDto.UserId))
            {
                TempData["ErrorMessage"] = "ID korisnika je obavezan.";
                return RedirectToAction("ManageUsers");
            }

            try
            {
                var response = await _webApiExecuter.InvokePut<UpdateUserDto, ApiResponseDto>($"role/UpdateUser/{updateUserDto.UserId}", updateUserDto);

                if (response != null && response.Success)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Korisnik uspešno ažuriran.";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom ažuriranja korisnika.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction("ManageUsers");
        }

        // Brisanje korisnika
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "ID korisnika je obavezan.";
                return RedirectToAction("ManageUsers");
            }

            try
            {
                var response = await _webApiExecuter.InvokeDelete<ApiResponseDto>($"role/DeleteUser/{userId}");

                if (response != null && response.Success)
                {
                    TempData["SuccessMessage"] = response.Message ?? "Korisnik uspešno obrisan.";
                }
                else
                {
                    TempData["ErrorMessage"] = response?.Message ?? "Greška prilikom brisanja korisnika.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Greška: {ex.Message}";
            }

            return RedirectToAction("ManageUsers");
        }

        


    }
}

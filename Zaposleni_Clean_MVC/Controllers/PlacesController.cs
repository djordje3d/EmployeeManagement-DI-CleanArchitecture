using Microsoft.AspNetCore.Mvc;
using Zaposleni_Blazor.CoreBusiness;
using Zaposleni_Blazor.UseCases.Mesta.Interfaces;

namespace Zaposleni_Clean_MVC.Controllers
{
    public class PlacesController : Controller
    {
        private readonly IListMestoUseCase listPlacesUseCase;
        private readonly IAddMestoUseCase addPlaceUseCase;
        private readonly IEditMestoUseCase editPlaceUseCase;
        private readonly IDeleteMestoUseCase deletePlaceUseCase;
        private readonly IMestoByIdUseCase placeByIdUseCase;

        public PlacesController(
            IListMestoUseCase listPlacesUseCase,
            IAddMestoUseCase addPlaceUseCase,
            IEditMestoUseCase editPlaceUseCase,
            IDeleteMestoUseCase deletePlaceUseCase,
            IMestoByIdUseCase placeByIdUseCase)
        {
            this.listPlacesUseCase = listPlacesUseCase;
            this.addPlaceUseCase = addPlaceUseCase;
            this.editPlaceUseCase = editPlaceUseCase;
            this.deletePlaceUseCase = deletePlaceUseCase;
            this.placeByIdUseCase = placeByIdUseCase;
        }

        public async Task<IActionResult> Index()
        {
            var places = await listPlacesUseCase.ExecuteAsync();
            return View(places);
        }

        public IActionResult AddPlace()
        {
            return View(new Mesto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPlace(Mesto place)
        {
            if (ModelState.IsValid)
            {
                var success = await addPlaceUseCase.ExecuteAsync(place);

                if (success)
                {
                    TempData["SuccessMessage"] = "Place successfully added.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "An error occurred while adding the place.");
            }

            return View(place);
        }

        public async Task<IActionResult> UpdatePlace(int placeId)
        {
            try
            {
                var place = await placeByIdUseCase.ExecuteAsync(placeId);

                if (place != null)
                {
                    return View(place);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while loading the place: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePlace(Mesto place)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool success = await editPlaceUseCase.ExecuteAsync(place);

                    if (!success)
                    {
                        TempData["ErrorMessage"] = "Place was not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    TempData["SuccessMessage"] = "Place successfully updated.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred while updating the place: {ex.Message}";
                }
            }

            return View(place);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlace(int placeId)
        {
            try
            {
                bool success = await deletePlaceUseCase.ExecuteAsync(placeId);

                TempData[success ? "SuccessMessage" : "ErrorMessage"] = success
                    ? "Place successfully deleted."
                    : "An error occurred while deleting the place.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

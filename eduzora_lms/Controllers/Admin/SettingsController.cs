using Azure.Core;
using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Admin.ViewModels;
using eduzora_lms.Repositories.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eduzora_lms.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("settings-admin")]
    public class SettingsController : Controller
    {
        private readonly ISettingsRepository settingsRepository;

        public SettingsController(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }



        // Edit Sales Commission
        [HttpGet("sales-commission")]
        public async Task<IActionResult> SalesCommission()
        {
            // Set To Active in SideBarAdmin.cshtml file
            ViewBag.CurrentPage = "sales-commission";

            // Display All Settings
            var getcommission = await settingsRepository.GetAsync();

            if (getcommission == null)
            {
                TempData["error_message"] = "Sales Commission Value Not Found";
            }

            var model = new EditSalesCommissionRequest
            {
                SalesCommission = getcommission!.SalesCommission
            };

            // Calculate and pass instructor commission to the view
            ViewBag.AdminCommission = getcommission.SalesCommission;
            ViewBag.InstructorCommission = 100 - getcommission.SalesCommission;

            return View(model);
        }

        [HttpPost("sales-commission")]
        public async Task<IActionResult> SalesCommission(EditSalesCommissionRequest editSalesCommissionRequest)
        {
            // Set To Active in SideBarAdmin.cshtml file
            ViewBag.CurrentPage = "sales-commission";

            if (!ModelState.IsValid)
            {
                var err_msg = string.Join("<br/>", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                                    .Select(e => e.ErrorMessage)
                                    .ToList());

                TempData["error_message"] = err_msg;


                // Pass current values to view for display in calculation table
                ViewBag.AdminCommission = editSalesCommissionRequest.SalesCommission;
                ViewBag.InstructorCommission = 100 - editSalesCommissionRequest.SalesCommission;

                return View("~/Views/Settings/SalesCommission.cshtml", editSalesCommissionRequest);
            }

            var existingSaleCommission = await settingsRepository.GetAsync();

            if (existingSaleCommission == null)
            {
                TempData["error_message"] = "Sales Commission record not found!";
                return RedirectToAction("SalesCommission");
            }

            // update existing Record
            existingSaleCommission.SalesCommission = editSalesCommissionRequest.SalesCommission;
            var updateSetting = await settingsRepository.UpdateAsync(existingSaleCommission);

            if (updateSetting != null)
            {
                TempData["success_message"] = "Sales Commission updated successfully!";
            }
            else
            {
                TempData["error_message"] = "Failed to update Sales Commission.";
            }

            return RedirectToAction("SalesCommission");

        }


    }
}

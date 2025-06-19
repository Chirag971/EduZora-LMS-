using eduzora_lms.Data;
using eduzora_lms.Models.Admin.ViewModels;
using eduzora_lms.Models.Instructor.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eduzora_lms.Controllers.Instructor
{
    [Route("/instructor-dashboard")]
    [Authorize(Roles = "Instructor")]
    public class InstructorDashboardController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public InstructorDashboardController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            /// For Dashboard Active CSS
            ViewBag.CurrentPage = "instructor-dashboard";

            return View("~/Views/InstructorDashboard/Index.cshtml");
        }

        // Instructor Edit-Profile Page UI
        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            /// For Dashboard Active CSS
            ViewBag.CurrentPage = "edit-profile";

            // Get the Current Student
            var user = await userManager.GetUserAsync(User);

            // If User Not Found
            if (user == null)
            {
                TempData["error_message"] = "Error : User Not Found!";
                await signInManager.SignOutAsync();
                return RedirectToPage("/account/login");
            }

            // View Model With Student Data
            var model = new InstructorEditProfileViewModel
            {
                FullName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Country = user.Country,
                State = user.State,
                City = user.City,
                Zipcode = user.Zipcode,
                PhotoPath = user.PhotoPath!
            };

            return View("~/Views/InstructorDashboard/EditProfile.cshtml", model);

        }

        // Instructor Edit-Profile Page Form Submit
        [HttpPost("edit-profile")]
        public async Task<IActionResult> EditProfile(InstructorEditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error_message"] = "Please correct the form errors.";
                var userForDisplay = await userManager.GetUserAsync(User);
                if (userForDisplay != null)
                {
                    model.PhotoPath = userForDisplay.PhotoPath;
                }
                return View("~/Views/InstructorDashboard/EditProfile.cshtml", model);
            }

            // Update UserName
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["error_message"] = "Error: User not found. Please log in again.";
                await signInManager.SignOutAsync();
                return RedirectToPage("/Account/Login");
            }


            // Update UserName As Full Name
            if (user.UserName != model.FullName)
            {
                var setUserNameResult = await userManager.SetUserNameAsync(user, model.FullName);
                if (!setUserNameResult.Succeeded)
                {
                    foreach (var error in setUserNameResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    var err_msg = ModelState.Values
                                 .SelectMany(v => v.Errors)
                                 .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                                 .Select(e => e.ErrorMessage)
                                 .ToList();

                    TempData["error_msg_list"] = err_msg;

                    model.PhotoPath = user.PhotoPath;
                    return View("EditProfile", model);
                }
            }

            // Update User Email
            if (user.Email != model.Email)
            {
                var setEmailResult = await userManager.SetEmailAsync(user, model.Email);

                if (!setEmailResult.Succeeded)
                {
                    foreach (var error in setEmailResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    var err_msg = ModelState.Values
                                 .SelectMany(v => v.Errors)
                                 .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                                 .Select(e => e.ErrorMessage)
                                 .ToList();

                    TempData["error_msg_list"] = err_msg;

                    model.PhotoPath = user.PhotoPath;

                    return View("EditProfile", model);

                }
            }

            // Update User Password
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("Retype Password", "Password And Retype Password Do not Match.");

                    var err_msg = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    TempData["error_msg_list"] = err_msg;

                    model.PhotoPath = user.PhotoPath;

                    return View("EditProfile", model);
                }

                var hasPassword = await userManager.HasPasswordAsync(user);
                if (hasPassword)
                {
                    var removePasswordResult = await userManager.RemovePasswordAsync(user);
                    if (!removePasswordResult.Succeeded)
                    {
                        foreach (var error in removePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        var err_msg = ModelState.Values
                       .SelectMany(v => v.Errors)
                       .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                       .Select(e => e.ErrorMessage)
                       .ToList();

                        TempData["error_msg_list"] = err_msg;

                        model.PhotoPath = user.PhotoPath;

                        return View("EditProfile", model);

                    }
                }

                var addPasswordResult = await userManager.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    foreach (var error in addPasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    var err_msg = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    TempData["error_msg_list"] = err_msg;

                    model.PhotoPath = user.PhotoPath;
                    return View("EditProfile", model);
                }
            }

            // Upload User Photo
            if (model.UserPhoto != null)
            {
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(model.UserPhoto.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!string.IsNullOrEmpty(user.PhotoPath) && user.PhotoPath != "default.png")
                {
                    var oldFilePath = Path.Combine(uploadsFolder, user.PhotoPath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }


                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.UserPhoto.CopyToAsync(fileStream);
                }

                user.PhotoPath = uniqueFileName;
            }

            // Update Phone no
            if (user.PhoneNumber != model.Phone)
            {
                // update phono
                user.PhoneNumber = model.Phone;
            }

            // Update Address
            if (user.Address != model.Address)
            {
                // update Address
                user.Address = model.Address;
            }

            // Update Country
            if (user.Country != model.Country)
            {
                user.Country = model.Country;
            }

            // update state
            if (user.State != model.State)
            {
                user.State = model.State;
            }

            // update city
            if (user.City != model.City)
            {
                user.City = model.City;
            }


            // Update zipCode
            if (user.Zipcode != model.Zipcode)
            {
                user.Zipcode = model.Zipcode;
            }


            // Finally update User
            var finalUpdateResult = await userManager.UpdateAsync(user);
            if (!finalUpdateResult.Succeeded)
            {
                foreach (var err in finalUpdateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                }

                var err_msg = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["error_msg_list"] = err_msg;

                model.PhotoPath = user.PhotoPath;

                return View("EditProfile", model);
            }

            // Refresh SignIn
            await signInManager.RefreshSignInAsync(user);

            // Display Toast
            TempData["success_message"] = "Profile Update Successfully";

            // after that redirection
            return RedirectToAction("EditProfile", "InstructorDashboard");
        }
    }
}

using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using eduzora_lms.Data;
using eduzora_lms.Models.Admin.ViewModels;
using eduzora_lms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace eduzora_lms.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.webHostEnvironment = webHostEnvironment;
        }

        // For Login Page UI
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index","StudentDashboard");
                }
                else if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("Index","InstructorDashboard");
                }
            }
            return View();
        }

        // For Submit Login Form
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var err_msg = ModelState.Values
                .SelectMany(v => v.Errors)
                .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                .Select(e => e.ErrorMessage)
                .ToList();

                TempData["error_msg_list"] = err_msg;

                return View("~/Views/Account/Login.cshtml", model);
            }

            // Find Email Of That User
            var user = await userManager.FindByEmailAsync(model.Email!);

            if (user == null)
            {
                TempData["error_msg_list"] = new List<string> { "Invalid Email Or Password" };

                return View("~/Views/Account/Login.cshtml", model);
            }

             // if e -mail not confirm than display error
            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                TempData["error_message"] = "Your email address has not been confirmed. Please confirm your email before Login";

                return View("~/Views/Account/Login.cshtml", model);
            }

            // Sign-In manager for password verification
            var result = await signInManager.PasswordSignInAsync(user, model.Password!, false, false);

            if (result.Succeeded)
            {
                // chk if User Has Admin Role
                if (user != null && await userManager.IsInRoleAsync(user, "Admin"))
                {
                    // Display Toast Message
                    TempData["success_message"] = "You Are Logged In Successfully";

                    // Return redirect to the dashboard
                    return RedirectToAction("Index", "DashboardAdmin");
                }
                else if (user != null && await userManager.IsInRoleAsync(user, "Student"))
                {
                    // Display Toast Message
                    TempData["success_message"] = "You Are Logged In Successfully";

                    // Return redirect to the Student dashboard
                    return RedirectToAction("Index", "StudentDashboard");
                }
                else if (user != null && await userManager.IsInRoleAsync(user, "Instructor"))
                {

                    // Display Toast Message
                    TempData["success_message"] = "You Are Logged In Successfully";

                    // Return redirect to the Student dashboard
                    return RedirectToAction("Index", "InstructorDashboard");
                }
                else
                {
                    // user is exits but password is correct but role is not admin
                    await signInManager.SignOutAsync();
                    // Error
                    ModelState.AddModelError(string.Empty, "Access Denied : You Are Not Authorized Valid User.");
                    TempData["error_msg_list"] = new List<string> { "Access Denied : You Are Not Authorized Valid User." };
                    return View("~/Views/Account/Login.cshtml", model);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
                TempData["error_msg_list"] = new List<string> { "Invalid Or Wrong Password" };

                return View("~/Views/Account/Login.cshtml", model);
            }

        }


        // For Forgot-Password Page UI
        [HttpGet("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // For Submit ForgotPassword Form
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var err_msg = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["error_msg_list"] = err_msg;

                return View("~/Views/Account/ForgotPassword.cshtml", model);
            }

            // 1. find user by email
            var user = await userManager.FindByEmailAsync(model.Email!);

            // 2. if user variable return the null than [User Does not Exits]
            if (user == null)
            {
                TempData["error_message"] = "User Does Not Exists. Please Check Email Address";
                return View("~/Views/Account/ForgotPassword.cshtml", model);
            }

            // 3. if email not confirm than display error
            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                TempData["error_message"] = "Your email address has not been confirmed. Please confirm your email before resetting your password.";

                return View("~/Views/Account/ForgotPassword.cshtml", model);
            }

            // 4. Generate pwd reset token
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // 5. Encoding Generated Token in URL 
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // 6. Reset-Password Call Back URL
            var callResetPassword = Url.Action("ResetPassword", "Account", new { userId = user.Id, @token = encodedToken });

            // 7. Send the E-Mail With Password Reset Link
            string emailBody = $@"
            <p>Please reset your password by clicking the button below:</p>
            <a href='http://localhost:5271{HtmlEncoder.Default.Encode(callResetPassword!)}'
               style='display: inline-block;
                      padding: 10px 20px;
                      font-size: 16px;
                      color: white;
                      background-color: #007bff; 
                      border-radius: 5px;
                      text-decoration: none;
                      text-align: center;
                      cursor: pointer;'>
                      Reset Password
            </a>
            <p style='margin-top: 20px;'>If the button above does not work, you can copy and paste the following link into your browser:</p>
            <p><a href='http://localhost:5271{HtmlEncoder.Default.Encode(callResetPassword!)}'>
                http://localhost:5271{HtmlEncoder.Default.Encode(callResetPassword!)}</a></p>";

            // Sent Email Implementation Method.
            await emailSender.sendEmailAsync("Harsh_P_@AspDotNetCore.Com", user.Email!, "Reset Password Request", emailBody, true);

            // set password allowed Session
            HttpContext.Session.SetString("PasswordResetAllowed", "true");

            // Diplay Toast
            // 8. Success Message and Redirect
            TempData["success_message"] = "A password reset link has been sent to your email. Please check your inbox.";

            return View("~/Views/Account/ForgotPassword.cshtml", model);
        }


        // For Reset-Password Page UI with userid & token
        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword(string? userId, string? token)
        {
            // in qry string if is it will null than display toast
            if (userId == null || token == null)
            {
                TempData["error_message"] = "Invalid Password Reset Link";
                RedirectToAction("ForgotPassword", "Account");
            }

            var pwd_reset_allowed = HttpContext.Session.GetString("PasswordResetAllowed");
            if (pwd_reset_allowed == "false")
            {
                // After One Time Password Changed Than This Block
                var alreadyUsed = HttpContext.Session.GetString("PasswordResetUsed");
                if (alreadyUsed == "true" || alreadyUsed == null)
                {
                    TempData["error_message"] = "Session is Expired Generate A New Link";
                    return RedirectToAction("ForgotPassword", "Account");
                }
            }

            var chkUserId = await userManager.FindByIdAsync(userId!);

            if (chkUserId == null)
            {
                TempData["error_message"] = "Invalid Password Reset Link. User ID Not Found!";
                RedirectToAction("ForgotPassword", "Account");
            }

            // After the Clicking the link if validation is ok than start session
            HttpContext.Session.SetString("PasswordResetAllowed", "true");


            // Pass the UserID and Token to the ViewModel
            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View("~/Views/Account/ResetPassword.cshtml", model);
        }


        // For Reset-password form submit
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var err_msg = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["error_msg_list"] = err_msg;
                return View("~/Views/Account/ResetPassword.cshtml", model);
            }

            // Find the UserID is user is exits
            var user = await userManager.FindByIdAsync(model.UserId!);

            // if user id not found diplay a toast
            if (user == null)
            {
                TempData["error_message"] = "Invalid Password Reset Request. User Not Found!.";
                return RedirectToAction("Login", "Account");
            }

            // Decode Token
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token!));
            }
            catch (FormatException)
            {
                TempData["error_message"] = "Invalid Password Token Format! Token Didn't Match";
                return RedirectToAction("Login", "Account");
            }

            // Now Reset-Password
            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.Password!);

            if (result.Succeeded)
            {
                // Set Session For GET ResetPassword To True
                HttpContext.Session.SetString("PasswordResetUsed", "true");
                // set sesssion for PasswordResetAllowed to False
                HttpContext.Session.SetString("PasswordResetAllowed", "false");

                // Display Toast
                TempData["success_message"] = "Your Password Has Been Reset Successfully. Please Login With New Password.";

                return RedirectToAction("Login", "Account"); // Redirect to login page
            }
            else
            {
                var err_msg = result.Errors.Select(e => e.Description).ToList();

                TempData["error_msg_list"] = err_msg;

                return View("~/Views/Account/ResetPassword.cshtml", model);
            }

        }

        // For Display Edit Profile Page UI
        [Authorize(Roles = "Admin")]
        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await userManager.GetUserAsync(User);
            // if User Not Found! than this condition will Work!
            if (user == null)
            {
                TempData["error_message"] = "Error : User Not Found!";
                await signInManager.SignOutAsync();
                return RedirectToPage("/account/login");
            }

            // For Display Profile Round Icon
            ViewBag.UserPhotoPath = user.PhotoPath ?? "default.png";

            // 2. Populate the viewModel with user data
            var model = new EditProfileViewModel
            {
                FullName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? user.UserName!,
                Email = user.Email!,
                PhotoPath = user.PhotoPath!
            };


            return View("~/Views/Account/EditProfile.cshtml", model);
        }


        // Edit-Profile Form Page
        [Authorize(Roles = "Admin")]
        [HttpPost("edit-profile")]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error_message"] = "Please correct the form errors.";
                var userForDisplay = await userManager.GetUserAsync(User);
                if (userForDisplay != null)
                {
                    model.PhotoPath = userForDisplay.PhotoPath;
                }
                return View("~/Views/Account/EditProfile.cshtml", model);
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
                if (model.Password != model.RetypePassword)
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

            // Finally User Update
            var finalUpdateResult = await userManager.UpdateAsync(user);
            if (!finalUpdateResult.Succeeded)
            {
                foreach (var error in finalUpdateResult.Errors)
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

            // Referesh Sign In
            await signInManager.RefreshSignInAsync(user);

            // Redirect to The Dashboard Page
            TempData["success_message"] = "Profile updated successfully!";
            // After Update profile redirect to the DashBoard Page.
            return RedirectToAction("Index", "DashboardAdmin");
        }

        // Logout and redirect to the login page
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            TempData["success_message"] = "You have been successfully logged out.";

            return RedirectToAction("Login", "Account");
        }

        // Access Denied Page
        [HttpGet("account/access-denied")]
        public IActionResult AccessDenied() 
        {
            return View("~/Views/Account/AccessDenied.cshtml");
        }


        // Confirm Email Address When Register Student/Instructor
        [HttpGet("account/confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // 1. chk if userId and token is null than display toast
            if (userId == null || token == null)
            {
                TempData["error_message"] = "Invalid email confirmation link ";
                // redirect to the Login
                return RedirectToAction("Login", "Account");
            }

            //2. Find the User by ID 
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["error_message"] = "Unable to Find The User ID! Invalid User Id";
                // redirect to the login
                return RedirectToAction("Login", "Account");
            }

            //3.  Decode Token
            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch (FormatException)
            {
                TempData["error_message"] = "The confirmation link is invalid or corrupted.";
                // redirect to the login page
                return RedirectToAction("Login", "Account");
            }

            // 4. Confirm the user email using usermanager
            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
            { 
                TempData["success_message"] = "Thank You For Confirming Your Email! Please Login Here!";
                // Clear All the Identity Sessions
                HttpContext.Session.Clear();
                // redirect to the login page
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var err_msg = result.Errors.Select(e => e.Description).ToList();

                TempData["error_msg_list"] = err_msg;

                return View("~/Views/Account/Login.cshtml", err_msg);
            }
        }

    }
}

using System.Text;
using System.Text.Encodings.Web;
using eduzora_lms.Data;
using eduzora_lms.Models.Student.ViewModels;
using eduzora_lms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace eduzora_lms.Controllers.Student
{
    [Route("/student")]
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public StudentController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }


        // Registration Form Page UI
        [HttpGet("registration")]
        public IActionResult Registration()
        {
            // If User is Already Logged in and try to SignUp
            if (User.Identity!.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "StudentDashboard");
                }
                else if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("Index", "InstructorDashboard");
                }
            }

            return View("~/Views/Student/Registration.cshtml");
        }


        // Registration Form Page
        [HttpPost("registration")]
        public async Task<IActionResult> Registration(StudentRegistrationViewModel model)
        {
            // Model State not valid
            if (!ModelState.IsValid)
            {
                var err_msg = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["error_msg_list"] = err_msg;

                return View("~/Views/Student/Registration.cshtml", model);
            }

            // 1. chk if User with this email already exists
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An Account With This Email Address Already Exists.");

                TempData["error_message"] = "An Account With This Email Address Already Exists.";

                return View("~/Views/Student/Registration.cshtml", model);
            }

            // Create a new application User Object for getting the ViewModel Data
            var user = new ApplicationUser
            {
                UserName = model.Name,
                Email = model.Email,
                EmailConfirmed = false,
                NormalizedEmail = model.Email.ToUpper(),
                NormalizedUserName = model.Name.ToUpper(),
                LockoutEnabled = false
            };

            // 2. Create the Users In AspNetUsers tbl
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //1.  chk role exists in asp.net roles tbl   
                string rolename = "Student";
                if (!await roleManager.RoleExistsAsync(rolename))
                {
                    //2. if role not found inside the role tbl
                    await roleManager.CreateAsync(new IdentityRole(rolename) { ConcurrencyStamp = user.Id });
                }

                //3. Assign the role to the New User in AspNet UsersRoles Tbl
                var roleAssignResult = await userManager.AddToRoleAsync(user, rolename);

                if (roleAssignResult.Succeeded)
                {
                    // 4. Generate Email Confirm Token
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    // 5. Encoding Generated Token in URL 
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    // 6. Reset-Password Call Back URL
                    var callConfirmEmail = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, @token = encodedToken });

                    // 7. Send the E-Mail With Password Reset Link
                    string emailBody = $@"
                        <p>Dear {user.Email}, Student</p>
                        <p>Thank you for registering at EduzoraLMS. Please confirm your email address by clicking the button below:</p>
                        <div style='text-align: center; margin: 20px 0;'>
                            <a href='http://localhost:5271{HtmlEncoder.Default.Encode(callConfirmEmail!)}'
                               style='display: inline-block;
                                      padding: 10px 20px;
                                      font-size: 16px;
                                      color: white;
                                      background-color: #007bff;
                                      border-radius: 5px;
                                      text-decoration: none;
                                      text-align: center;
                                      cursor: pointer;'>
                                Confirm Your Email
                            </a>
                        </div>
                        <p style='margin-top: 20px;'>If the button above does not work, you can copy and paste the following link into your browser:</p>
                        <p><a href='http://localhost:5271{HtmlEncoder.Default.Encode(callConfirmEmail!)}'</a></p>
                        <p>Thanks,<br>The EduzoraLMS Team</p>";

                    // Sent Email Implementation Method.
                    await emailSender.sendEmailAsync("Harsh_P_@AspDotNetCore.Com", user.Email!, "Confirm Your Email-Address for EduzoraLMS", emailBody, true);


                    // Diplay Toast
                    // 8. Success Message and Redirect
                    TempData["success_message"] = "A Email Confirmation link has been sent to your Email Address. Please check your inbox.";
                    // clear the form 
                    ModelState.Clear();

                    return View("~/Views/Student/Registration.cshtml", new StudentRegistrationViewModel());

                }
            }
            else
            {
                var err_msg = result.Errors.Select(e => e.Description).ToList();

                TempData["error_msg_list"] = err_msg;

                return View("~/Views/Student/Registration.cshtml", model);

            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            if (errors.Any())
            {
                TempData["error_msg_list"] = errors;
            }
            return View("~/Views/Student/Registration.cshtml", model);

        }

    }
}

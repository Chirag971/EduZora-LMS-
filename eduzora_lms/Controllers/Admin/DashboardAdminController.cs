using System.Threading.Tasks;
using eduzora_lms.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace eduzora_lms.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("dashboard-admin")]
    public class DashboardAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardAdminController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Set To Active in SideBarAdmin.cshtml file
            ViewBag.CurrentPage = "dashboard-admin";

            // For Profile Photo get the current User
            var currentuser = await userManager.GetUserAsync(User);
            // Get the img of the current User and sent it to the view
            ViewBag.UserPhotoPath = currentuser?.PhotoPath ?? "default.png";


            return View("~/Views/DashboardAdmin/Index.cshtml");
        }
    }
}

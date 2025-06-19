using eduzora_lms.Data;
using eduzora_lms.Models.Instructor.Domain;
using eduzora_lms.Models.Instructor.ViewModels;
using eduzora_lms.Repositories.Instructor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eduzora_lms.Controllers.Instructor
{
    [Authorize(Roles = "Instructor")]
    [Route("create-course")]
    public class CreateCourseController : Controller
    {
        private readonly ICreateCourseRepository createCourseRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CreateCourseController( ICreateCourseRepository createCourseRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.createCourseRepository = createCourseRepository;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }


        // Add Course UI Page
        [HttpGet("add-course")]
        public async Task<IActionResult> Add()
        {
            // For Active create course CSS
            ViewBag.CurrentPage = "create-course";

            var model = new AddCourseRequestViewModel();
            await PopulateDropdowns(model); 

            return View("~/Views/CreateCourse/Add.cshtml", model);
        }

        [HttpPost("add-course")]
        public async Task<IActionResult> Add(AddCourseRequestViewModel model)
        {

            // For Active create course CSS
            ViewBag.CurrentPage = "create-course";



            // Featured Photo Validation and Featured Banner Validation
            if (model.FeaturedPhotoFile == null)
            {
                ModelState.AddModelError(nameof(model.FeaturedPhotoFile), "Course Featured Photo is required.");
            }

            if (model.FeaturedBannerFile == null)
            {
                ModelState.AddModelError(nameof(model.FeaturedBannerFile), "Course Featured Banner is required.");
            }

            // Featured Video Type Validation
            if (string.IsNullOrWhiteSpace(model.FeaturedVideoType))
            {
                ModelState.AddModelError(nameof(model.FeaturedVideoType), "Course Featured Video Type is required.");
            }
            else
            {
                if (model.FeaturedVideoType == "YouTube")
                {
                    if (string.IsNullOrWhiteSpace(model.FeaturedVideoContentYoutube))
                    {
                        ModelState.AddModelError(nameof(model.FeaturedVideoContentYoutube), "YouTube video URL is required.");
                    }
                }
                else if (model.FeaturedVideoType == "Vimeo")
                {
                    if (string.IsNullOrWhiteSpace(model.FeaturedVideoContentVimeo))
                    {
                        ModelState.AddModelError(nameof(model.FeaturedVideoContentVimeo), "Vimeo video URL is required.");
                    }
                }
                else if (model.FeaturedVideoType == "mp4")
                {
                    if (model.FeaturedVideoContentMp4File == null)
                    {
                        ModelState.AddModelError(nameof(model.FeaturedVideoContentMp4File), "MP4 video file is required.");
                    }
                }
            }

            if (!ModelState.IsValid)
            {

                TempData["error_message"] = "All Fields Are Required.";

                await PopulateDropdowns(model); // Repopulate dropdowns

                return View("~/Views/CreateCourse/Add.cshtml", model);
            }

            // Create Course Logic
            var current_instructor = await userManager.GetUserAsync(User);
            if (current_instructor == null)
            {
                TempData["error_message"] = "Instructor not logged in or Not Found.";
                await PopulateDropdowns(model); // Repopulate dropdowns
                return View("~/Views/CreateCourse/Add.cshtml", model);
            }

            // if instructor Found Than Get the Id
            string instructorId = current_instructor.Id;

            string featuredPhotoPath = null!;
            if (model.FeaturedPhotoFile != null)
            {
                featuredPhotoPath = await SaveFile(model.FeaturedPhotoFile, "courses/photos");
            }

            string featuredBannerPath = null!;
            if (model.FeaturedBannerFile != null)
            {
                featuredBannerPath = await SaveFile(model.FeaturedBannerFile, "courses/banners");
            }

            string featuredVideoContent = null!;
            if (model.FeaturedVideoType == "youtube")
            {
                featuredVideoContent = model.FeaturedVideoContentYoutube!;
            }
            else if (model.FeaturedVideoType == "Vimeo")
            {
                featuredVideoContent = model.FeaturedVideoContentVimeo!;
            }
            else if (model.FeaturedVideoType == "mp4" && model.FeaturedVideoContentMp4File != null)
            {
                featuredVideoContent = await SaveFile(model.FeaturedVideoContentMp4File, "courses/videos");
            }

            // Domain Model
            var course = new Course()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Slug = model.Slug,
                Description = model.Description,
                Price = (int) model.Price!,
                PriceOld = model.PriceOld!,
                Category_id = (Guid) model.Category_id!,
                Level_id = (Guid) model.Level_id!,
                Language_id = (Guid) model.Language_id!,
                InstructorId = instructorId,
                FeaturedPhoto = featuredPhotoPath!,
                FeaturedBanner = featuredBannerPath!,
                FeaturedVideoType = model.FeaturedVideoType!,
                FeaturedVideoContent = featuredVideoContent!,
                Status = "Pending",
                UpdatedAt = DateTime.Now,
                TotalStudent = 0,
                TotalRating = 0,
                TotalRatingScore = 0,
                AverageRating = 0,
                TotalVideoHours = 0,
                TotalVideos = 0,
                TotalResources = 0
            };

            // Add Course
            var addCourse = await createCourseRepository.AddAsync(course);

            if (addCourse != null)
            {
                TempData["success_message"] = "Course created successfully and is pending approval!";
                return RedirectToAction("Add", "CreateCourse");
            }
            else
            {
                TempData["error_message"] = "Failed to Create Course.";
                await PopulateDropdowns(model); // Repopulate dropdowns
                return View("~/Views/CreateCourse/Add.cshtml", model);
            }

        }

        // Populate DropDowns
        private async Task PopulateDropdowns(AddCourseRequestViewModel model)
        {
            model.Categories = (await createCourseRepository.GetAllCategoriesAsync())
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName });

            model.Levels = (await createCourseRepository.GetAllLevelsAsync())
             .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.LevelName});

            model.Languages = (await createCourseRepository.GetAllLanguagesAsync())
             .Select(l => new SelectListItem { Value = l.Id.ToString(), Text =  l.LanguageName});
        }

        // SaveFile Helper Method
        private async Task<string> SaveFile(IFormFile file, string subfolder)
        {
            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads", subfolder);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine("/uploads", subfolder, uniqueFileName).Replace("\\","/");
        }

    }
}

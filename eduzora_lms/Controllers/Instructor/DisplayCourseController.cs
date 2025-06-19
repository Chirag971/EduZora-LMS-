
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
    [Route("display-course")]
    public class DisplayCourseController : Controller
    {
        private readonly IDisplayCourseRepository displayCourseRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public DisplayCourseController(IDisplayCourseRepository displayCourseRepository, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.displayCourseRepository = displayCourseRepository;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
        }


        // Display All Instructors Courses
        [HttpGet("instructor-courses")]
        public async Task<IActionResult> Index()
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            // Get Current Instructor
            var instructor = await userManager.GetUserAsync(User);

            // if current User Not Found
            if (instructor == null)
            {
                return Unauthorized();
            }

            // Pass Id to the param of Current instructor
            var courses = await displayCourseRepository.GetCoursesByInstructorAsync(instructor.Id);

            var courses_viewmodel = courses.Select((c , index) => new InstructorCoursesDisplayViewModel 
            {
                CourseId = c.Id,
                Title = c.Title,
                FeaturedPhoto = c.FeaturedPhoto,
                Price = c.Price,
                Status = c.Status
            }).ToList();

            return View(courses_viewmodel);
        }


        // edit basic instructor Courses
        [HttpGet("instructor-course-edit-basic/{id}")]
        public async Task<IActionResult> EditCourseBasic(Guid id)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            var course = await displayCourseRepository.GetCourseById(id);

            if (course != null)
            {
                var editCourseBasicViewModel = new EditCourseBasicRequestViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    Slug = course.Slug,
                    Description = course.Description,
                    Price = course.Price,
                    PriceOld = course.PriceOld,
                    Category_id = course.Category_id,
                    Level_id = course.Level_id,
                    Language_id = course.Language_id
                };

                await PopulateDropdowns(editCourseBasicViewModel);
                return View(editCourseBasicViewModel);
            }

            return View(null);
        }

        // edit basic instructor Courses Form
        [HttpPost("instructor-course-edit-basic/{id}")]
        public async Task<IActionResult> EditCourseBasic(EditCourseBasicRequestViewModel editCourseBasicRequest)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(editCourseBasicRequest);
                return View(editCourseBasicRequest);
            }

            // Edit Domain Model
            var editcoursebasic = new Course
            { 
                Id = editCourseBasicRequest.Id,
                Title = editCourseBasicRequest.Title,
                Slug = editCourseBasicRequest.Slug,
                Description = editCourseBasicRequest.Description,
                Price = (int) editCourseBasicRequest.Price!,
                PriceOld = editCourseBasicRequest.PriceOld!,
                Category_id = (Guid) editCourseBasicRequest.Category_id!,
                Level_id = (Guid) editCourseBasicRequest.Level_id!,
                Language_id = (Guid) editCourseBasicRequest.Language_id!
            };

            var updatedBasicCourseInfo = await displayCourseRepository.UpdateBasicCourseInfoAsync(editcoursebasic);

            if (updatedBasicCourseInfo != null)
            {
                // display toast
                TempData["success_message"] = "Course Basic Info Updated successfully!";
            }
            else
            {
                // display toast
                TempData["error_message"] = "Course Basic Info Can't Update !";
            }
            return RedirectToAction("EditCourseBasic", new { id = editCourseBasicRequest.Id });

        }


        // Edit course Featured Photo
        [HttpGet("instructor-course-edit-featured-photo/{id}")]
        public async Task<IActionResult> EditCourseFeaturedPhoto(Guid id)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            var course = await displayCourseRepository.GetCourseById(id);

            if (course != null)
            {
                var editCourseFeaturedPhoto = new EditCourseFeaturedPhotoRequestViewModel
                { 
                    id = course.Id,
                    CurrentFeaturedPhoto = course.FeaturedPhoto
                };

                return View(editCourseFeaturedPhoto);
            }
            
            return View(null);
        }

        // edit Course Featured Photo Form Submit

        [HttpPost("instructor-course-edit-featured-photo/{id}")]
        public async Task<IActionResult> EditCourseFeaturedPhoto(EditCourseFeaturedPhotoRequestViewModel editCourseFeaturedPhoto)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            if (!ModelState.IsValid)
            {
                return View(editCourseFeaturedPhoto);
            }


            var course = await displayCourseRepository.GetCourseById(editCourseFeaturedPhoto.id);
            if (course == null)
            {
                TempData["error_message"] = "Course Id Not Found";
                return RedirectToAction("EditCourseBasic", new { id = editCourseFeaturedPhoto.id});
            }

            string newPhotoPath = course.FeaturedPhoto;

            if (editCourseFeaturedPhoto.FeaturedPhotoFile != null)
            {
                // Delete Old Photo and insert new photo
                if (!string.IsNullOrWhiteSpace(course.FeaturedPhoto))
                {
                    var oldPath = Path.Combine(webHostEnvironment.WebRootPath, course.FeaturedPhoto.TrimStart('/').Replace("/",Path.DirectorySeparatorChar.ToString()));

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                }
            }

            newPhotoPath = await SaveFile(editCourseFeaturedPhoto.FeaturedPhotoFile!, "courses/photos");

            // Update Domain Model
            course.FeaturedPhoto = newPhotoPath;
            course.UpdatedAt = DateTime.Now;

            var updatedFeaturedPhoto = await displayCourseRepository.UpdateCourseFeaturedPhoto(course);

            if (updatedFeaturedPhoto != null)
            {
                TempData["success_message"] = "Course Featured Photo updated successfully!";
            }
            else
            {
                TempData["error_message"] = "Failed to update Course featured photo!";
            }   

            return RedirectToAction("EditCourseFeaturedPhoto", new { id = editCourseFeaturedPhoto.id });
        }


        // edit Course Featured Banner Photo

        [HttpGet("instructor-course-edit-featured-banner/{id}")]
        public async Task<IActionResult> EditCourseFeaturedBanner(Guid id)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            var course = await displayCourseRepository.GetCourseById(id);

            if (course != null)
            {
                var editFeaturedBanner = new EditCourseFeaturedBannerRequestViewModel
                { 
                    id = course.Id,
                    CurrentFeaturedBanner = course.FeaturedBanner
                };

                return View(editFeaturedBanner);
            }

            return View(null);

        }


        // edit course featured Banner Form Submit
        [HttpPost("instructor-course-edit-featured-banner/{id}")]
        public async Task<IActionResult> EditCourseFeaturedBanner(EditCourseFeaturedBannerRequestViewModel editCourseFeaturedBanner)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            if (!ModelState.IsValid)
            {
                return View(editCourseFeaturedBanner);
            }

            var course = await displayCourseRepository.GetCourseById(editCourseFeaturedBanner.id);

            if (course == null)
            {

                TempData["error_message"] = "Course Id Not Found";
                return RedirectToAction("EditCourseBasic", new { id = editCourseFeaturedBanner.id });
            }

            string newPhotoPath = course.FeaturedBanner;

            if (editCourseFeaturedBanner.FeaturedBannerFile != null)
            {
                // Delete Old Photo and insert new photo
                if (!string.IsNullOrWhiteSpace(course.FeaturedBanner))
                {
                    var oldPath = Path.Combine(webHostEnvironment.WebRootPath, course.FeaturedBanner.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }

                }
            }

            newPhotoPath = await SaveFile(editCourseFeaturedBanner.FeaturedBannerFile!, "courses/banners");

            // Update Domain Model
            course.FeaturedBanner = newPhotoPath;
            course.UpdatedAt = DateTime.Now;

            var updatedFeaturedBanner = await displayCourseRepository.UpdateCourseFeaturedBanner(course);

            if (updatedFeaturedBanner != null)
            {
                TempData["success_message"] = "Course Featured Banner updated successfully!";
            }
            else
            {
                TempData["error_message"] = "Failed to update Course featured Banner !";
            }

            return RedirectToAction("EditCourseFeaturedBanner", new { id = editCourseFeaturedBanner.id });
        }



        [HttpGet("instructor-course-edit-featured-video/{id}")]
        public async Task<IActionResult> EditCourseFeaturedVideo(Guid id)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            var course = await displayCourseRepository.GetCourseById(id);

            if (course != null)
            {
                var editFeaturedVideo = new EditCourseFeaturedVideoRequestViewModel
                {
                    Id = course.Id,
                    CurrentFeaturedVideoType = course.FeaturedVideoType,
                    CurrentFeaturedVideoContent = course.FeaturedVideoContent,
                };

                return View(editFeaturedVideo);
            }

            return View(null);
        }


        // edit Course Featured Video
        [HttpPost("instructor-course-edit-featured-video/{id}")]
        public async Task<IActionResult> EditCourseFeaturedVideo(EditCourseFeaturedVideoRequestViewModel editCourseFeaturedVideo)
        {
            // For Active Sidebar Instructor Css
            ViewBag.CurrentPage = "display-course";

            if (!ModelState.IsValid)
            {
                return View(editCourseFeaturedVideo);
            }

            // Get Course by its ID
            var course = await displayCourseRepository.GetCourseById(editCourseFeaturedVideo.Id);

            if (course == null)
            {
                NotFound();
            }

            string videoContent = editCourseFeaturedVideo.CurrentFeaturedVideoContent ?? "";
            string videoType = editCourseFeaturedVideo.CurrentFeaturedVideoType ?? "";

            if (editCourseFeaturedVideo.FeaturedVideoType == "youtube" && !string.IsNullOrWhiteSpace(editCourseFeaturedVideo.FeaturedVideoContentYoutube))
            {
                videoContent = editCourseFeaturedVideo.FeaturedVideoContentYoutube;
                videoType = "youtube";
            }
            else if (editCourseFeaturedVideo.FeaturedVideoType == "vimeo" && !string.IsNullOrWhiteSpace(editCourseFeaturedVideo.FeaturedVideoContentVimeo))
            {
                videoContent = editCourseFeaturedVideo.FeaturedVideoContentVimeo;
                videoType = "vimeo";
            }
            else if (editCourseFeaturedVideo.FeaturedVideoType == "mp4" && editCourseFeaturedVideo.FeaturedVideoContentMP4File != null)
            {
                // Delete old Files if Exits
                if (!string.IsNullOrWhiteSpace(course.FeaturedVideoContent) && course.FeaturedVideoType == "mp4")
                {
                    var oldPath = Path.Combine(webHostEnvironment.WebRootPath, course.FeaturedVideoContent.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }


                // Save new file
                videoContent = await SaveFile(editCourseFeaturedVideo.FeaturedVideoContentMP4File, "courses/videos");
                videoType = "mp4";
            }

            course!.FeaturedVideoType = videoType;
            course.FeaturedVideoContent = videoContent;
            course.UpdatedAt = DateTime.Now;


            var updated = await displayCourseRepository.UpdateCourseFeaturedVideo(course);

            if (updated != null)
            {
                TempData["success_message"] = "Course Featured video updated!";
            }
            else
            {
                TempData["error_message"] = "Course Featured Video Update failed!";
            }

            return RedirectToAction("EditCourseFeaturedVideo", new { id = editCourseFeaturedVideo.Id });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteInstructorCourse(Guid id)
        {
            var instructor = await userManager.GetUserAsync(User);
            if (instructor == null)
            {
                return Unauthorized();
            }

            var deletedCourse = await displayCourseRepository.DeleteCourse(id, instructor.Id);

            if (deletedCourse != null)
            {
                TempData["success_message"] = $"Course '{deletedCourse.Title}' deleted!";
                return RedirectToAction("Index", "DisplayCourse");
            }
            else
            {
                TempData["error_message"] = "Course Not Found Or Not Authorized";
            }

            return RedirectToAction("Index", "DisplayCourse");
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

            return Path.Combine("/uploads", subfolder, uniqueFileName).Replace("\\", "/");
        }


        // Populate Dropdown
        private async Task PopulateDropdowns(EditCourseBasicRequestViewModel model)
        {
            model.Categories = (await displayCourseRepository.GetAllCategoriesAsync())
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.CategoryName });

            model.Levels = (await displayCourseRepository.GetAllLevelsAsync())
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.LevelName });

            model.Languages = (await displayCourseRepository.GetAllLanguagesAsync())
                .Select(l => new SelectListItem { Value = l.Id.ToString(), Text = l.LanguageName });
        }
    }
}

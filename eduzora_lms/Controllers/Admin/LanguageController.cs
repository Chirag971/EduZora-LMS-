using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Admin.ViewModels;
using eduzora_lms.Repositories.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("language-admin")]
    public class LanguageController : Controller
    {
        private readonly ILanguageRepository languageRepository;
        private readonly ApplicationDbContext dbContext;

        public LanguageController(ILanguageRepository languageRepository,ApplicationDbContext dbContext)
        {
            this.languageRepository = languageRepository;
            this.dbContext = dbContext;
        }


        [HttpGet("Display")]
        public async Task<IActionResult> Index()
        {
            // Set To Active in SideBarAdmin.cshtml file
            ViewBag.CurrentPage = "language-display";

            // Display All Languages
            var language = await languageRepository.GetAllAsync();

            return View(language);
        }


        // Add Language UI
        [HttpGet("add")]
        public IActionResult Add()
        { 
            ViewBag.CurrentPage = "language-add";

            return View();
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddLanguageRequest addLanguageRequest)
        { 
            ViewBag.CurrentPage = "language-add";

            if (!ModelState.IsValid)
            {
                var err_msg = string.Join("<br/>", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Where(e => !string.IsNullOrWhiteSpace(e.ErrorMessage))
                                    .Select(e => e.ErrorMessage)
                                    .ToList());

                TempData["error_message"] = err_msg;

                return View();
            }

            // chk if Languages already exits
            var existingLanguage = await dbContext.Languages.AnyAsync(l => l.LanguageName.Equals(addLanguageRequest.LanguageName));

            if (existingLanguage)
            {
                ModelState.AddModelError("LanguageName", "A Language with this name already exists. Please choose a different Language.");

                // Display toast
                TempData["error_msg_list"] = new List<string> { "A Language with this name already exists. Please choose a different Language" };

                return View();
            }

            // create obj
            var language = new Language()
            {
                LanguageName = addLanguageRequest.LanguageName
            };

            // repository method
            await languageRepository.AddAsync(language);

            // display toast
            TempData["success_message"] = "Language created successfully!";

            // redirect 
            return RedirectToAction("Index");
        }


        // Language Edit get Id
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            // for hover css effect
            ViewBag.CurrentPage = "language-edit";

            var language = await languageRepository.GetAsync(id);

            if (language != null)
            {
                var editlanguagereq = new EditLanguageRequest()
                {
                    Id = language.Id,
                    LanguageName = language.LanguageName
                };

                return View(editlanguagereq);
            }

            return View(null);
        }


        // For Edit Language Form Submit
        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditLanguageRequest editLanguageRequest)
        {
            // edit domain model
            var language = new Language 
            { 
                Id = editLanguageRequest.Id,
                LanguageName = editLanguageRequest.LanguageName
            };

            var updateLanguage = await languageRepository.UpdateAsync(language);

            if (updateLanguage != null)
            {
                // Display Toast
                TempData["success_message"] = "Language Updated successfully!";
            }
            else
            {
                // display toast
                TempData["error_message"] = "Language Can't Update !";
            }
            return RedirectToAction("Edit", new { id = editLanguageRequest.Id });
        }


        // Delete Language By ID
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(EditLanguageRequest editLanguageRequest)
        {
            var deletedLanguage = await languageRepository.DeleteAsync(editLanguageRequest.Id);

            if (deletedLanguage != null)
            {
                // Show Toast
                TempData["success_message"] = "Language Deleted successfully!";
                return RedirectToAction("Index", "Language");
            }

            TempData["error_message"] = "Language Can't Delete Invalid ID";
            // Show An Error 
            return RedirectToAction("Index", "Language");

        }
    }
}

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
    [Route("level-admin")]
    public class LevelController : Controller
    {
        private readonly ILevelRepository levelRepository;
        private readonly ApplicationDbContext dbContext;

        public LevelController(ILevelRepository levelRepository, ApplicationDbContext dbContext)  
        {
            this.levelRepository = levelRepository;
            this.dbContext = dbContext;
        }

        [HttpGet("display")]
        public async Task<IActionResult> Index()
        {
            // Set To Active in SideBarAdmin.cshtml file
            ViewBag.CurrentPage = "level-display";

            // Display All Levels
            var level = await levelRepository.GetAllAsync();

            return View(level);
        }


        // Add Level UI
        [HttpGet("add")]
        public IActionResult Add()
        {
            ViewBag.CurrentPage = "level-add";

            return View();
        }


        // Add Levels
        [HttpPost("add")]
        public async Task<IActionResult> Add(AddLevelRequest addLevelRequest)
        { 
            ViewBag.CurrentPage = "level-add";

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

            // chk if levels already exits
            var existingLevel = await dbContext.Levels.AnyAsync(l => l.LevelName.Equals(addLevelRequest.LevelName));

            if (existingLevel)
            {
                ModelState.AddModelError("LevelName", "A Level with this name already exists. Please choose a different name.");

                // Display toast
                TempData["error_msg_list"] = new List<string> { "A Level with this name already exists. Please choose a different name" };

                return View();
            }


            // create obj
            var level = new Level()
            { 
                LevelName = addLevelRequest.LevelName
            };

            // repository method
            await levelRepository.AddAsync(level);

            // display toast
            TempData["success_message"] = "Level created successfully!";

            // redirect 
            return RedirectToAction("Index");
        }


        // Level Edit Get Id
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            // for hover css effect
            ViewBag.CurrentPage = "level-edit";

            var level = await levelRepository.GetAsync(id);

            if (level != null)
            {
                var editlevelreq = new EditLevelRequest() { 
                    Id = level.Id,
                    LevelName = level.LevelName
                };

                return View(editlevelreq);
            }

            return View(null);
        }


        // For Level Form Submit
        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditLevelRequest editLevelRequest)
        {
            // edit domain model
            var level = new Level
            { 
                Id = editLevelRequest.Id,
                LevelName = editLevelRequest.LevelName
            };

            var updatelevel = await levelRepository.UpdateAsync(level);


            if (updatelevel != null)
            {
                // display toast
                TempData["success_message"] = "Level Updated successfully!";
            }
            else
            {
                // display toast
                TempData["error_message"] = "Level Can't Update !";
            }
            return RedirectToAction("Edit", new { id = editLevelRequest.Id });
        }


        // Delete Level By ID
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(EditLevelRequest editLevelRequest)
        {
            var deletedLevel = await levelRepository.DeleteAsync(editLevelRequest.Id);

            if (deletedLevel != null)
            {
                // Show Toast
                TempData["success_message"] = "Level Deleted successfully!";
                return RedirectToAction("Index", "Level");
            }

            TempData["error_message"] = "Level Can't Delete Invalid ID!";
            // show an error Notification
            return RedirectToAction("Index", "Level");
        }

    }
}

using System.Threading.Tasks;
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
    [Route("category-admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ICategoryRepository categoryRepository, ApplicationDbContext dbContext)
        {
            this.categoryRepository = categoryRepository;
            this.dbContext = dbContext;
        }



        [HttpGet("display")]
        public async Task<IActionResult> Index()
        {
            // Set To Active in SideBarAdmin.cshtml file
            ViewBag.CurrentPage = "category-display";

            // Display All Category
            var category = await categoryRepository.GetAllAsync();

            return View(category);
        }


        // Add Category UI
        [HttpGet("add")]
        public IActionResult Add()
        {
            ViewBag.CurrentPage = "category-add";

            return View();
        }

        // Category Add Form 
        [HttpPost("add")]
        public async Task<IActionResult> Add(AddCategoryRequest addCategoryRequest)
        {
            ViewBag.CurrentPage = "category-add";

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

            // chk first is category is already exits
            var existingCategory = await dbContext.Categories.AnyAsync(c => c.CategoryName.Equals(addCategoryRequest.CategoryName));

            if (existingCategory)
            {
                ModelState.AddModelError("CategoryName", "A category with this name already exists. Please choose a different name.");

                // Display toast
                TempData["error_msg_list"] = new List<string> { "A category with this name already exists. Please choose a different name." };

                return View();
            }

            // create obj
            var category = new Category()
            {
                CategoryName = addCategoryRequest.CategoryName
            };

            // Repository Method
            await categoryRepository.AddAsync(category);

            // display toast
            TempData["success_message"] = "Category created successfully!";

            // redirect 
            return RedirectToAction("Index");
        }


        // Category Edit Get Id
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            // for hover css effect
            ViewBag.CurrentPage = "category-edit";


            var category = await categoryRepository.GetAsync(id);

            if (category != null)
            {
                var editCategoryReq = new EditCategoryRequest()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                };

                return View(editCategoryReq);
            }

            return View(null);
        }

        // For Category Form Submit
        [HttpPost("edit")]
        public async Task<IActionResult> Edit(EditCategoryRequest editCategoryRequest)
        {
            // Edit Domain Model
            var category = new Category
            {
                Id = editCategoryRequest.Id,
                CategoryName = editCategoryRequest.CategoryName
            };

            var updatecategory = await categoryRepository.UpdateAsync(category);

            if (updatecategory != null)
            {
                // display toast
                TempData["success_message"] = "Category Updated successfully!";
            }
            else
            {
                // display toast
                TempData["error_message"] = "Category Can't Update !";
            }
            return RedirectToAction("Edit", new { id = editCategoryRequest.Id });

        }


        // Delete Category Id
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(EditCategoryRequest editCategoryRequest)
        {
            var deletedCategory = await categoryRepository.DeleteAsync(editCategoryRequest.Id);

            if (deletedCategory != null)
            {
                // Show Toast
                TempData["success_message"] = "Category Deleted successfully!";
                return RedirectToAction("Index", "Category");
            }

            TempData["error_message"] = "Category Can't Delete Invalid ID!";
            // show an error Notification
            return RedirectToAction("Index","Category");
        }
    }
}

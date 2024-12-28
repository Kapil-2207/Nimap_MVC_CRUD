using Microsoft.AspNetCore.Mvc;
using MVC_CRUD.Data;
using System.Linq;

namespace MVC_CRUD
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        // GET: Categories
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var categories = _service.GetAllCategories(page, pageSize);
            ViewBag.TotalPages = (int)Math.Ceiling((double)_service.GetCategoryCount() / pageSize);
            ViewBag.CurrentPage = page;
            return View(categories);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View(new Category()); 
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (_service.AddCategory(category) == "Duplicate category entry is not allowed.")
            {
                ModelState.AddModelError("CategoryName", "Duplicate category entry is not allowed.");
                return View(category); 
            }

            TempData["Message"] = "Category added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var category = _service.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            var result = _service.UpdateCategory(category);
            TempData["Message"] = result;
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete
        public IActionResult Delete(int id)
        {
            var category = _service.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteCategory(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

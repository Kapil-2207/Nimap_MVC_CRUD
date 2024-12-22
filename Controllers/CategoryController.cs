
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Data;
using MVC_CRUD.Models;
using System.Linq;

namespace MVC_CRUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Category/Create

        [HttpGet]
        public IActionResult Create()
        {
            // Populate categories for the dropdown
            ViewData["CategoryId"] = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Add the product to the database
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index)); // Redirect to the product index page after successful creation
                }
                catch (Exception ex)
                {
                    // Log the error and return with validation error
                    ModelState.AddModelError("", "An error occurred while creating the product: " + ex.Message);
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            // Re-populate categories if validation fails
            ViewData["CategoryId"] = _context.Categories.ToList();
            return View(product); // Return the product view with validation errors
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound(); // If product doesn't exist, return 404
            }

            // Populate categories for the dropdown
            ViewData["CategoryId"] = _context.Categories.ToList();
            return View(product); // Return the product view for editing
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound(); // If the id doesn't match, return 404
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the product in the database
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index)); // Redirect to the product index page after successful update
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Handle potential concurrency issues
                    if (!_context.Products.Any(p => p.ProductId == id))
                    {
                        return NotFound(); // If the product doesn't exist, return 404
                    }
                    else
                    {
                        throw; // Rethrow exception if the concurrency issue is not resolved
                    }
                }
                catch (Exception ex)
                {
                    // Log the error and return with validation error
                    ModelState.AddModelError("", "An error occurred while updating the product: " + ex.Message);
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            // Re-populate categories if validation fails
            ViewData["CategoryId"] = _context.Categories.ToList();
            return View(product); // Return the product view with validation errors
        }


        // GET: Category/Delete/5
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound(); // If the category doesn't exist, return a 404 error
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound(); // If the category doesn't exist, return a 404 error
            }

            _context.Categories.Remove(category); // Remove the category from the database
            _context.SaveChanges(); // Save the changes
            return RedirectToAction(nameof(Index)); // Redirect to the category list page after successful deletion
        }
    }
}

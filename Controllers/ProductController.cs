
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Data;
using MVC_CRUD.Models;
using System.Linq;

namespace MVC_CRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();  // Eagerly load the related Category
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = _context.Categories.ToList(); // Populate categories for the dropdown
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product); // Add the product to the database
                _context.SaveChanges(); // Save the changes
                return RedirectToAction(nameof(Index)); // Redirect to the product list page
            }
            ViewData["CategoryId"] = _context.Categories.ToList(); // Re-populate categories if validation fails
            return View(product); // If validation fails, return the same view with errors
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = _context.Categories.ToList(); // Populate categories for the dropdown
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Update(product); // Update the product in the database
                    _context.SaveChanges(); // Save the changes
                    return RedirectToAction(nameof(Index)); // Redirect to the product list page
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(p => p.ProductId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["CategoryId"] = _context.Categories.ToList(); // Re-populate categories if validation fails
            return View(product);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_CRUD.Data;

namespace MVC_CRUD
{
    public class ProductController : Controller
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        // GET: Product/Index
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var products = _service.GetAllProducts(page, pageSize);
            ViewBag.TotalPages = (int)Math.Ceiling((double)_service.GetProductCount() / pageSize);
            ViewBag.CurrentPage = page;

            ViewBag.Message = TempData["Message"];
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _service.GetCategories();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            var result = _service.AddProduct(product);
            TempData["Message"] = result; 
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = _service.GetCategories();
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var result = _service.UpdateProduct(product);
            TempData["Message"] = result; 
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _service.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        // Cancel button action
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}

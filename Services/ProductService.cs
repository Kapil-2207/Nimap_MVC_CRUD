using System.Collections.Generic;
using System.Linq;
using MVC_CRUD.Data;

namespace MVC_CRUD
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts(int page, int pageSize)
        {
            return _context.Products
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .Select(p => new Product
                           {
                               ProductId = p.ProductId,
                               ProductName = p.ProductName,
                               CategoryId = p.CategoryId,
                               Category = p.Category
                           })
                           .ToList();
        }

        public int GetProductCount()
        {
            return _context.Products.Count();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        public string AddProduct(Product product)
        {
            if (_context.Products.Any(p => p.ProductName == product.ProductName))
            {
                return "Duplicate product entry is not allowed.";
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return "Product added successfully.";
        }

        public string UpdateProduct(Product product)
        {
            if (_context.Products.Any(p => p.ProductName == product.ProductName && p.ProductId != product.ProductId))
            {
                return "Duplicate product entry is not allowed.";
            }

            _context.Products.Update(product);
            _context.SaveChanges();
            return "Product updated successfully.";
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }
    }
}

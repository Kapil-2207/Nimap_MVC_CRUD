using System.Collections.Generic;
using System.Linq;
using MVC_CRUD.Data;

namespace MVC_CRUD
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAllCategories(int page, int pageSize)
        {
            return _context.Categories
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToList();
        }

        public int GetCategoryCount()
        {
            return _context.Categories.Count();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.Find(id);
        }

        public string AddCategory(Category category)
        {
            if (_context.Categories.Any(c => c.CategoryName == category.CategoryName))
            {
                return "Duplicate category entry is not allowed.";
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return "Category added successfully.";
        }

        public string UpdateCategory(Category category)
        {
            if (_context.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryId != category.CategoryId))
            {
                return "Duplicate category entry is not allowed.";
            }

            _context.Categories.Update(category);
            _context.SaveChanges();
            return "Category updated successfully.";
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}

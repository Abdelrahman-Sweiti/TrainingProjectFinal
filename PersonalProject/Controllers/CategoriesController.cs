using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalProject.Data;
using PersonalProject.Models;
using PersonalProject.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace PersonalProject.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategory _Category;
        private readonly IProductsCategory _productsCategory;


        public CategoriesController(ApplicationDbContext context, ICategory Category, IProductsCategory productsCategory)
        {
            _context = context;
            _Category = Category;
            _productsCategory = productsCategory;
        }

        // GET: Categories
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var list1 = await _Category.GetCategories();

            return View(list1);
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Category category, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await _Category.GetFile(file, category);
                await _Category.Create(category);

                return RedirectToAction("Index", "Categories");
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,Category category,IFormFile? file)
        {
            if (file != null)
            {
                await _Category.GetFile(file, category);

            }

            await _Category.UpdateCategoryAsync(id,category);
            return RedirectToAction("Index","Categories");
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.categories'  is null.");
            }
            var category = await _context.categories.FindAsync(id);
            if (category != null)
            {
                _context.categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<IActionResult> AddProductToCategories(int CategoryId)
        {
            var category = await _Category.GetCategoryById(CategoryId);

            if (category == null)
            {
                return NotFound(); // Handle the case where the category is not found.
            }

            var categoryProduct = new ProductsCategory()
            {
                CategoryId = CategoryId
            };

            ViewData["CategoryName"] = category.Name; // Pass the category name to the view using ViewData.

            return View(categoryProduct);
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductToCategories(ProductsCategory categoryProduct, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                await _Category.GetFile(file, categoryProduct.product);

                await _Category.AddProductToCategories(categoryProduct.CategoryId, categoryProduct.product);

                return RedirectToAction("Index", "Main");
            }
            else
            {
                return RedirectToAction("ViewAllProducts", "Products");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductsForCategory(int CategoryId)
        {
            var category = await _Category.GetCategoryById(CategoryId);
            var products = await _productsCategory.GetAllProductsForCategory(CategoryId);

            if (category == null)
            {
                return NotFound();
            }
            ViewData["CategoryName"] = category.Name;

            return View(products);
        }




    }
}

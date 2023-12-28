using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PersonalProject.Data;
using PersonalProject.Models;
using PersonalProject.Models.Services;
using Microsoft.AspNetCore.Identity;
using PersonalProject.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PersonalProject.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUser _User;
        private readonly ICart _Cart;
        private readonly UserManager<ApplicationUser> _userManager;
        public CartsController(ApplicationDbContext context, IUser User, ICart cart, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _User = User;
            _Cart = cart;
            _userManager = userManager;
        }


        // GET: Carts
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _User.GetUser(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login page if the user is not authenticated
            }

            var productsInCart = await _Cart.GetProductsInCartAsync(user.Id);

            return View(productsInCart);
        }

        //public int GetProductsCount(string Username)
        //{ 
        //int products = _Cart.GetCartItemCountAsync(Username);
        //    return products;
        //}

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            // Retrieve the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                // Handle the case where user ID is not available
                return RedirectToAction("Index", "Carts");
            }


            // Call the service to remove the product from the cart
            var success = await _Cart.RemoveProductFromCartAsync(productId, userId);

            if (success)
            {
                return RedirectToAction("Index", "Carts"); // Product successfully removed, redirect to cart
            }
            else
            {
                return RedirectToAction("Index", "Carts"); // Redirect to cart (or handle error accordingly)
            }
        }




        public async Task<IActionResult> GetCartItemCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                var cartItemCount = await _Cart.GetCartItemCountAsync(User.Identity.Name);
                return Json(new { cartItemCount });
            }

            // Handle the case where the user is not authenticated (return 0 or an error).
            return Json(new { cartItemCount = 0 });
        }


        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TotalPrice,Count")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TotalPrice,Count")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.carts == null)
            {
                return NotFound();
            }

            var cart = await _context.carts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.carts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.carts'  is null.");
            }
            var cart = await _context.carts.FindAsync(id);
            if (cart != null)
            {
                _context.carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return (_context.carts?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        // CartService.cs


    }
}
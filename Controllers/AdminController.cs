using GiftLab.Data;
using GiftLab.Data.Entities;
using GiftLab.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftLab.Controllers
{
    public class AdminController : Controller
    {
        private readonly GiftLabDbContext _context;

        public AdminController(GiftLabDbContext context)
        {
            _context = context;
        }

        [Route("admin")]
        [Route("admin/index")]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        [Route("admin/dashboard")]
        public IActionResult Dashboard()
        {
            ViewData["Title"] = "Dashboard - Admin";
            return View();
        }

        [Route("admin/analytics")]
        public IActionResult Analytics()
        {
            ViewData["Title"] = "Analytics - Admin";
            return View();
        }

        [Route("admin/users")]
        public IActionResult Users()
        {
            ViewData["Title"] = "Users - Admin";
            return View();
        }

        [Route("admin/products")]
        public IActionResult Products()
        {
            var products = _context.Products
                .Include(p => p.Cat)
                .Select(p => new AdminProductViewModel
                {
                    Id = p.ProductID,
                    Name = p.ProductName,
                    Category = p.Cat != null ? p.Cat.Catname : "",
                    Price = p.Price ?? 0,
                    Stock = p.UnitsInStock ?? 0,
                    Active = p.Active,
                    Description = p.Description,
                    ShortDesc = p.ShortDesc,
                    ImagePath = p.Thumb != null
                        ? p.Thumb.Replace("~", "")
                        : "/images/no-image.png"

                })
                .ToList();

            var vm = new AdminProductIndexViewModel
            {
                Products = products,
                TotalProducts = products.Count,
                ActiveProducts = products.Count(p => p.Active),
                OutOfStockProducts = products.Count(p => p.Stock <= 0),
                TotalCategories = _context.Categories.Count()
            };

            ViewData["Title"] = "Products - Admin";
            return View(vm);
        }

        [HttpPost]
        [Route("admin/products/create")]
        public async Task<IActionResult> CreateProduct(AdminCreateProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("D? li?u không h?p l?");

            string? imagePath = null;

            if (model.Image != null && model.Image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(stream);
                }

                imagePath = "/images/" + fileName;
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                ShortDesc = model.ShortDesc,
                Description = model.Description,
                Price = model.Price,
                UnitsInStock = model.UnitsInStock,
                CatID = model.CatID,
                Active = model.Active,
                Thumb = imagePath,
                BestSellers = false,
                HomeFLag = false,
                DateCreated = DateTime.Now
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }


        [Route("admin/orders")]
        public IActionResult Orders()
        {
            ViewData["Title"] = "Orders - Admin";
            return View();
        }

        [Route("admin/settings")]
        public IActionResult Settings()
        {
            ViewData["Title"] = "Settings - Admin";
            return View();
        }

        [HttpPost]
        [Route("admin/products/edit")]
        public async Task<IActionResult> EditProduct(AdminEditProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _context.Products.FindAsync(model.ProductID);
            if (product == null)
                return NotFound("Không t?m th?y s?n ph?m");

            // c?p nh?t d? li?u
            product.ProductName = model.ProductName;
            product.ShortDesc = model.ShortDesc;
            product.Description = model.Description;
            product.Price = model.Price;
            product.UnitsInStock = model.UnitsInStock;
            product.CatID = model.CatID;
            product.Active = model.Active;
            product.DateModified = DateTime.Now;

            // x? l? ?nh m?i
            if (model.Image != null && model.Image.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(model.Image.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using var stream = new FileStream(savePath, FileMode.Create);
                await model.Image.CopyToAsync(stream);

                product.Thumb = "/images/" + fileName;
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        [HttpPost]
        [Route("admin/products/delete")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Không t?m th?y s?n ph?m");

            // Xóa ?nh trong wwwroot/images
            if (!string.IsNullOrEmpty(product.Thumb))
            {
                var imagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    product.Thumb.TrimStart('/')
                );

                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }


    }
}


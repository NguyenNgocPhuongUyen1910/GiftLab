using GiftLab.Data;
using GiftLab.Helpers;
using GiftLab.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftLab.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private const string CART_KEY = "CART";
        private readonly GiftLabDbContext _db;

        public CartController(GiftLabDbContext db) => _db = db;

        [HttpGet("")]
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpGet("Add")]
        public IActionResult Add() => RedirectToAction(nameof(Index));

        [HttpPost("Add")]
        public IActionResult Add(int productId, int quantity = 1, int? variantId = null)
        {
            if (quantity < 1) quantity = 1;

            var product = _db.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product == null) return NotFound();

            // Base info từ Product
            var displayName = product.ProductName;
            var imagePath = product.Thumb ?? "";
            var unitPrice = product.Price ?? 0;

            // Nếu có variant (AttributesPrice)
            if (!variantId.HasValue || variantId.Value <= 0)
            {
                variantId = null;
            }

            if (variantId.HasValue)
            {
                var ap = _db.AttributesPrices
                    .Include(x => x.Attribute)
                    .FirstOrDefault(x => x.AttributesPriceID == variantId.Value
                                      && x.ProductID == productId
                                      && x.Active);

                if (ap == null) return NotFound();

                unitPrice = ap.Price ?? unitPrice;

                var attrName = ap.Attribute?.Name;
                if (!string.IsNullOrWhiteSpace(attrName))
                    displayName = $"{product.ProductName} - {attrName}";
            }

            var cart = GetCart();

            var existing = cart.Items.FirstOrDefault(x =>
                x.ProductId == productId && x.VariantId == variantId);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = productId,
                    VariantId = variantId,
                    Name = displayName,
                    ImagePath = imagePath,
                    UnitPrice = unitPrice,
                    Quantity = quantity
                });
            }

            SaveCart(cart);

            return RedirectToAction(nameof(Index)); // ✅ về /Cart để thấy ngay
        }
        [HttpPost("Remove")]
        public IActionResult Remove(int productId, int? variantId = null)
        {
            var cart = GetCart();

            var item = cart.Items.FirstOrDefault(x =>
                x.ProductId == productId && x.VariantId == variantId);

            if (item != null)
                cart.Items.Remove(item);

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Update")]
        public IActionResult Update(int productId, int? variantId = null, int quantity = 1)
        {
            var cart = GetCart();

            var item = cart.Items.FirstOrDefault(x =>
                x.ProductId == productId && x.VariantId == variantId);

            if (item == null)
                return RedirectToAction(nameof(Index));

            // Nếu quantity <= 0 thì xoá luôn
            if (quantity <= 0)
            {
                cart.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }


        private CartViewModel GetCart()
            => HttpContext.Session.GetObject<CartViewModel>(CART_KEY) ?? new CartViewModel();

        private void SaveCart(CartViewModel cart)
            => HttpContext.Session.SetObject(CART_KEY, cart);
    }
}

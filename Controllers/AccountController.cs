using Microsoft.AspNetCore.Mvc;
using GiftLab.Models; 

namespace GiftLab.Controllers
{
    public class AccountController : Controller
    {
        // 1. Trang Đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Xử lý Logic Đăng nhập thực tế
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // 2. Trang Đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Xử lý Logic Đăng ký thực tế
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        // 3. Trang Quên mật khẩu
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(); // Trả về View trống
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Xử lý logic gửi email đặt lại mật khẩu thực tế

                ViewBag.Message = "Nếu địa chỉ email tồn tại, chúng tôi đã gửi hướng dẫn đặt lại mật khẩu. Vui lòng kiểm tra email của bạn.";
                return View(model);
            }
            return View(model);
        }

        // 4. Trang Đơn hàng của tôi
        [Route("account/orders")]
        public IActionResult Orders()
        {
            ViewData["Title"] = "Đơn hàng của tôi";
            return View();
        }

        // 5. Chi tiết đơn hàng
        [Route("account/orders/{id}")]
        public IActionResult OrderDetails(int id)
        {
            ViewData["Title"] = "Chi tiết đơn hàng #" + id;
            ViewData["OrderId"] = id;
            return View();
        }
    }
}
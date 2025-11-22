using DemoWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DemoWeb.Controllers
{
    public class AccountController : Controller
    {
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;

		public AccountController()
		{
		}
		public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
			UserManager = userManager;
			SignInManager = signInManager;
		}
		public ApplicationSignInManager SignInManager
		{
			get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
			private set { _signInManager = value; }
		}
		public ApplicationUserManager UserManager
		{
			get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
			private set { _userManager = value; }
		}
		// GET: Login
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}
        // GET: /Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // POST: Login
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            // Kiểm tra ModelState
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("ModelState INVALID!");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"  Error in {state.Key}: {error.ErrorMessage}");
                    }
                }

                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View(model);
            }
            // Tìm user theo email
            var user = await UserManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Email hoặc mật khẩu không đúng!");
				return View(model);
			}

			// Đăng nhập
			var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, false, shouldLockout: false);

			if (result == SignInStatus.Success)
			{
				var roles = await UserManager.GetRolesAsync(user.Id);
				string userRole = roles.FirstOrDefault() ?? "Customer";

				// Lưu Session
				Session["UserId"] = user.Id;
				Session["Username"] = user.UserName;
				Session["FullName"] = user.FullName ?? user.UserName;
				Session["Email"] = user.Email;
				Session["UserRole"] = userRole;

                TempData["LoginMessage"] = "Đăng nhập thành công! Xin chào " + (user.FullName ?? user.UserName);
                // Chuyển hướng theo role
                if (roles.Contains("Admin"))
				{
					return RedirectToAction("Index", "Admin");
				}

				if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}

				return RedirectToAction("Index", "TrangChu");
			}

			ModelState.AddModelError("", "Email hoặc mật khẩu không đúng!");
			return View(model);
			
		}

		// GET: Register
		public ActionResult Register()
        {
            // Mặc định chỉ có Customer
            var roles = new List<string> { "Customer" };

            // Nếu user hiện tại là admin, mới thêm Admin vào list
            if (User.IsInRole("Admin"))
            {
                roles.Add("Admin");
            }
            ViewBag.Roles = new SelectList(roles, "Customer");
			return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string Role)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Nếu user chọn Admin nhưng người đăng ký không phải admin → từ chối
            if (Role == "Admin" && !User.IsInRole("Admin"))
            {
                ModelState.AddModelError("", "Bạn không có quyền tạo tài khoản Admin!");
                // Chỉ giữ Customer trong dropdown cho user bình thường
                ViewBag.Roles = new SelectList(new[] { "Customer" }, Role);
                return View(model);
            }

            // Kiểm tra email đã tồn tại
            var existingUser = await UserManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng!");
                ViewBag.Roles = User.IsInRole("Admin")
                    ? new SelectList(new[] { "Customer", "Admin" }, Role)
                    : new SelectList(new[] { "Customer" }, Role);
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true,
                CreatedAt = DateTime.Now
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AppDbContext()));

                if (!await roleManager.RoleExistsAsync(Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(Role));
                }

                await UserManager.AddToRoleAsync(user.Id, Role);
                TempData["AccountMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            ViewBag.Roles = User.IsInRole("Admin")
                ? new SelectList(new[] { "Customer", "Admin" }, Role)
                : new SelectList(new[] { "Customer" }, Role);

            return View(model);
        }

       
        // Đăng xuất
        public ActionResult Logout()
        {
			AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			Session.Clear();
			Session.Abandon();
            TempData["AccountMessage"] = "Đăng xuất thành công!";
            return RedirectToAction("Login", "Account");
        }
		private IAuthenticationManager AuthenticationManager
		{
			get { return HttpContext.GetOwinContext().Authentication; }
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_userManager != null)
				{
					_userManager.Dispose();
					_userManager = null;
				}

				if (_signInManager != null)
				{
					_signInManager.Dispose();
					_signInManager = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}

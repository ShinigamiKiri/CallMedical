using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CallMedical.Models;
using BCrypt.Net;

namespace CallMedical.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public JsonResult Register(RegisterRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.email) || string.IsNullOrEmpty(model.password))
            {
                return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                if (db.Users.Any(u => u.email == model.email))
                {
                    return Json(new { success = false, message = "Email này đã được sử dụng." });
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);

                var newUser = new UsersModel
                {
                    email = model.email,
                    password_hash = hashedPassword,
                    role = "patient",
                    status = "active",
                    create_at = DateTime.Now,
                    updated_at = DateTime.Now
                };

                db.Users.Add(newUser);
                db.SaveChanges();

                var newProfile = new PatientProfile
                {
                    user_id = newUser.id,
                    full_name = model.fullName ?? "Người dùng mới"
                };

                db.PatientProfiles.Add(newProfile);
                db.SaveChanges();

                // Set Session
                Session["UserId"] = newUser.id;
                Session["UserName"] = newProfile.full_name;
                Session["UserRole"] = newUser.role;

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                if (ex.InnerException?.InnerException != null) 
                    errorMsg += " | Detail: " + ex.InnerException.InnerException.Message;

                return Json(new { success = false, message = "Lỗi Backend: " + errorMsg });
            }
        }

        // POST: Account/Login
        [HttpPost]
        public JsonResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.password_hash))
            {
                var profile = db.PatientProfiles.FirstOrDefault(p => p.user_id == user.id);
                string name = profile != null ? profile.full_name : user.email;

                Session["UserId"] = user.id;
                Session["UserName"] = name;
                Session["UserRole"] = user.role;

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            }
            return Json(new { success = false, message = "Email hoặc mật khẩu không chính xác." });
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class RegisterRequest
    {
        public string email { get; set; }
        public string password { get; set; }
        public string fullName { get; set; }
    }
}

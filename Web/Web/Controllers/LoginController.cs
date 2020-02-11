using Web.Models.Login;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Web.Models;
using System.Linq;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        private LibraryContext _context;
        public LoginController(LibraryContext contex)
        {
            _context = contex;
        }
        public IActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User userModel)
        {
            User u;
            try
            {
                   u = _context.Users.Single(x => (x.UserName == userModel.UserName && x.Password == userModel.Password));
            }
            catch
            {
                return RedirectToAction("Index", "Login", new { message = "Wrong username or password" });
            }
            return await IsSuccessfulAsync(u.Id, userModel.UserName);
        }
        private async Task<RedirectToActionResult> IsSuccessfulAsync(int id,string username)
        {

            var databaseClaim = new List<Claim>() {
                new Claim(ClaimTypes.UserData,id.ToString()),
                new Claim(ClaimTypes.Name,username)
            };
            var dbIdentity = new ClaimsIdentity(databaseClaim, "Database");
            var UserPrincipal = new ClaimsPrincipal(new[] { dbIdentity });
            await HttpContext.SignInAsync(UserPrincipal);
            return RedirectToAction("Index", "Catalog");
        }
        public IActionResult Register(string error = "")
        {
            ViewBag.Error = error;
            return View();
        }
        public async Task<IActionResult> AddUser(RegisterModel register)
        {
            if(register.UserName == null || register.Password == null || register.ReEnterPassword == null)
            {
                return RedirectToAction("Register", "Login", new { error = "Registration Cannot be Completed while there are empty fields left" });
            }
            if (register.Password == register.ReEnterPassword)
            {
                User u = new User();
                u.UserName = register.UserName;
                u.Password = register.Password;
                User user;
                try
                {
                    _context.Users.Add(u);
                    _context.SaveChanges();
                    user = _context.Users.Single(x => (x.UserName == register.UserName && x.Password == register.Password));
                }
                catch
                {
                    return RedirectToAction("Register", "Login", new { error = "username already exists" });
                }     
                 return await IsSuccessfulAsync(user.Id, register.UserName);
            }
            return RedirectToAction("Register", "Login", new { error = "the passwords u entered don't match" });
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}

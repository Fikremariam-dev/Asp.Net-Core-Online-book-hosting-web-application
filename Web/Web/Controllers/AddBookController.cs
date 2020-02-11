using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Web.Models;
using Web.Models.AddBook;

namespace Web.Controllers
{
    public class AddBookController : Controller
    {
        private LibraryContext _context;
        public AddBookController(LibraryContext contex)
        {
            _context = contex;
        }
        [Authorize]
        public IActionResult Index(string m = "")
        {
            string user = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if(user == "Admin")
            {
                ViewBag.m = m;
                return View();
            }
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public  IActionResult Add(BookModel bm)
        {
            Book b = new Book();
            if(bm.Title != null || bm.Year != 0 || bm.ImagePath != null || bm.Tags != null || bm.Author != null || bm.PhoneNo != null)
            {
                b.Title = bm.Title;
                b.Year = bm.Year;
                try
                {
                    b.Image = System.IO.File.ReadAllBytes(bm.ImagePath);
                }
                catch
                {
                    return RedirectToAction("Index", "AddBook", new { m = "Image doesn't exist" });
                }
                b.Tags = bm.Tags;
                b.Author = bm.Author;
                b.PhoneNo = bm.PhoneNo;
                if((_context.Books.Where(x=>(x.Title == b.Title && x.Year == b.Year && x.Author == b.Author)).Count()) > 0)
                {
                    return RedirectToAction("Index", "AddBook", new { m = "Book already exists!" });
                }
                try
                {
                    _context.Add(b);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "AddBook", new { m = "successfully added" });
                }
                catch
                {
                    return RedirectToAction("Index", "AddBook", new { m = "Error" });
                }
            }
            return RedirectToAction("Index","AddBook",new { m = "Empty Fileds" });
        }
    }
}

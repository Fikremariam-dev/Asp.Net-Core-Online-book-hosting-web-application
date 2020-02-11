using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Web.Models;
using System.Linq;


namespace Web.Controllers
{
    public class CatalogController : Controller
    {
        private LibraryContext _context; // Initializing Dataase
        public CatalogController(LibraryContext contex)
        {
            _context = contex;
        }
        public IActionResult Index(string searchString = null)
        {
            IEnumerable<Book> books;
            if (searchString != null)
            {
                books = _context.Books.Where(x => (x.Title.Contains(searchString) || x.Author.Contains(searchString)));
            }
            else
                books = _context.Books.ToList();
            ViewBag.ss = searchString;
            return View(books);
        }
        public IActionResult getImage(int id)
        {
            Book b = _context.Books.Single(x => (x.Id == id));
            byte[] x = b.Image;
            return File(x, "image/jpg");
        }
        public IActionResult Detail(int id)
        {
            Book b = new Book();
            b = _context.Books.SingleOrDefault(x => (x.Id == id));
            IEnumerable<Rating> r;
            r = _context.Ratings.Where(x => (x.BookID == id));
            int count = r.Count();
            if (count == 0)
                ViewBag.Rating = 0;
            else
            {
                int sum = 0;
                foreach(Rating rating in r)
                {
                    sum = sum + rating.value;
                }
                ViewBag.Rating = sum / count;
            }
            ViewBag.NumberOfRatings = count;
            return View(b);
        }
        public RedirectToActionResult Delete(int id)
        {
            Book b = new Book();
            b = _context.Books.SingleOrDefault(x => (x.Id == id));
            _context.Remove(b);
            _context.SaveChanges();
            return RedirectToAction("Index", "Catalog");
        }
    }
}

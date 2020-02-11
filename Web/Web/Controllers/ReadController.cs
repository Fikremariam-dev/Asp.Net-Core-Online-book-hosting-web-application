using Web.Models.Read;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System;
using Web.Models;
using System.Linq;

namespace Web.Controllers
{
    public class ReadController : Controller
    {
        BookServiceClient bb = new BookServiceClient();
        
        private LibraryContext _context;
        public ReadController(LibraryContext contex)
        {
            _context = contex;
        }

        [Authorize]
        public async Task<IActionResult> Index(int id, int c = 1)
        {
            Read r = new Read();
            r.ID = id;
            r.Chapters = await bb.NumberOfChaptersAsync(id);
            int _userId = Int32.Parse(HttpContext.User.FindFirstValue(ClaimTypes.UserData));
            r.ReaderId = _userId;
            if (c > r.Chapters || c < 1)
                c = 1;
            r.CurrentChapterNumber = c;
            byte[] ChapterByteArray = await bb.getChapterAsync(id, r.CurrentChapterNumber);
            if (ChapterByteArray != null)
                r.CurrentChapter = System.Text.Encoding.UTF8.GetString(ChapterByteArray);
            else
                r.CurrentChapter = "Chapters are not available at the moment";
            Rating myRating = _context.Ratings.SingleOrDefault(x => (x.BookID == id && x.userId == _userId));
            if(myRating != null)
            r.userRating = myRating.value;
            return View(r);
        }
        public  IActionResult getImage(int id)
        {
            Book b = _context.Books.Single(x => (x.Id == id));
            byte[] x = b.Image;
            return File(x, "image/jpg");
        }


        [HttpPost]
        public void Rate(string UserID,string BookID,string value)
        {
            Rating r = _context.Ratings.SingleOrDefault(x => (x.BookID == Int32.Parse(BookID) && x.userId == Int32.Parse(UserID)));
            if(r == null)
            {
                r = new Rating();
                r.BookID = Int32.Parse(BookID);
                r.value = Int32.Parse(value);
                r.userId = Int32.Parse(UserID);
                _context.Ratings.Add(r);
                _context.SaveChanges();
            }
            else
            {
                r.value = Int32.Parse(value);
                _context.Update(r);
                _context.SaveChanges();
            }

        }

    }
}

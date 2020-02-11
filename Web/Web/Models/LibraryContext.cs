using Microsoft.EntityFrameworkCore;

namespace Web.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options) { }
        public DbSet<Rating> Ratings { set; get; }
        public DbSet<Book> Books { set; get; }
        public DbSet<User> Users { set; get; }
    }
}

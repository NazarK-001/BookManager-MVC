using Microsoft.EntityFrameworkCore;
using BookManager.Models;

namespace BookManager.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }  // needed to tell which database we are using (SQLite, SQL Server, etc.)
        public DbSet<BookModel> Books { get; set; } // each member of collection is a row in the table  

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=BookManagerApp.db");
        }
    }
}

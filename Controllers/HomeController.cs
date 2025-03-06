using System.Diagnostics;
using BookManager.Models;
using Microsoft.AspNetCore.Mvc;
using BookManager.Data;

namespace BookManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookContext _context;

        public HomeController(ILogger<HomeController> logger, BookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        // POST: Upload CSV
        [HttpPost]
        public IActionResult Index(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["Message"] = "Please select a valid CSV file.";
                return RedirectToAction("Index");
            }

            using (var stream = csvFile.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                try
                {
                    var records = csv.GetRecords<BookModel>().ToList();
                    foreach (var record in records)
                    {
                        if (!_context.Books.Any(u => u.Id == record.Id))
                        {
                            _context.Books.Add(record);
                        }
                        else
                        {
                            Console.WriteLine($"Duplicate record detected for Id: {record.Id}");
                        }
                    }
                    _context.SaveChanges();
                    TempData["Message"] = "CSV uploaded successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = $"Error processing file: {ex.Message}";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, string field, string value)
        {
            var book = _context.Books.FirstOrDefault(u => u.Id == id);
            if (book == null) return NotFound();

            if (field == "Title") book.Title = value;
            if (field == "Author") book.Author = value;
            if (field == "Genre") book.Genre = value;

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(u => u.Id == id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            _context.SaveChanges();
            return Ok();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



    }
}

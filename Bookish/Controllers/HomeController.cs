using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;

namespace Bookish.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpGet]
    public async Task<ActionResult> BookQuery(BookSelection selection)
    {
        string bookId = selection.Id;
        using (var context = new EFCore())
        {
            var books = context.Books.Where(x => x.Id == bookId).ToList();
            foreach (var book in books)
            {
                Console.WriteLine($"Here is your book with the Id {bookId}");
                Console.WriteLine(book.Id);
            }
        }
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
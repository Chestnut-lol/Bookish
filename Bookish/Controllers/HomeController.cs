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

    public IActionResult AddBook()
    {
        return View();
    }
    public IActionResult MemberSearch()
    {
        return View();
    }
    public async Task<ActionResult> MemberQuery(Member member)
    {
        string memberId = member.MemberId;
        
        using (var context = new EFCore())
        {
            member = context.Members.Find(memberId);
            if (member != null)
            {
                return View(member);
            }
        }
        Member resultMember = new Member();
        return View(resultMember);
    }


    [HttpGet]
    public async Task<ActionResult> BookQuery(BookSelection selection)
    {
        Book book = new Book();
        if (selection.Id != null)
        {
            using (var context = new EFCore())
            {
                book = context.Books.Where(x => x.Id == selection.Id).ToList()[0];
                
            }
            
        }
        return View(book);
    }
   
    /*public async Task<ActionResult> CheckoutBook(CheckoutSelection selection)
    {
        Book book = new Book();
        if (selection.BookCopyId != null)
        {
            using (var context = new EFCore())
            {
                book = context.BookCopy.Where(x => x.Id == selection.BookCopyId).ToList()[0];
                
            }
            
        }
        return View(book);
    }*/

    /*private BookInfo SearchBookByAuthor(string author)
    {
        using (var context = new EFCore())
        {
            var books = context.Books.Where(x => x.Author == author).ToList();
            foreach (var book in books)
            {
                Console.WriteLine($"Here is your book with the author {author}");
                Console.WriteLine($"Book name: {book.Title}");
                Console.WriteLine($"Author: {book.Author}");
                Console.WriteLine($"Number of available copies: {book.NumOfCopies}");
            }
        }
    }

    private BookInfo SearchBookById(string bookId)
    {
        using (var context = new EFCore())
        {
            var book = context.Books.Where(x => x.Id == bookId).ToList()[0];
            return new BookInfo(book.Id, book.Title, book.Author, book.NumOfCopies, book.NumOfAvailableCopies);
        }
    }*/


    [HttpGet]
    public async Task<ActionResult> BookInput(BookInput input)
    {
        string bookId = input.Id;
        using (var context = new EFCore())
        {
            var book = context.Books.Find(bookId);
            if (book != null)
            {
                book.NumOfCopies += 1;
                Console.WriteLine("Book found! Added another copy to the db.");
            }
            else

            { 
                book = new Book()
                {
                    Id = input.Id,
                    Title = input.Title,
                    Author = input.Author,
                    NumOfCopies = 1,
                };
                context.Books.Add(book);
            }
            context.SaveChanges();
            
            
            
            
                Console.WriteLine($"Here is your book with the Id {input.Id}");
                Console.WriteLine($"Book name: {book.Title}");
                Console.WriteLine($"Author: {book.Author}");
                Console.WriteLine($"Number of available copies: {book.NumOfCopies}");
                
        }
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
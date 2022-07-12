using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EFCore _dbContext;
    

    public HomeController(ILogger<HomeController> logger, EFCore dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
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
    public IActionResult CheckoutSelection()
    {
        return View();
    }
    public async Task<ActionResult> MemberQuery(Member member)
    {
        string memberId = member.MemberId;
        
        
        member = _dbContext.Members
            .Where(x => x.MemberId == memberId)
            .Include(m=>m.Books)
            .Include("Books.Book")
            .ToList()[0];
        if (member != null)
        {
            return View(member);
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
            
            book = _dbContext.Books
                .Where(x => x.Id == selection.Id)
                .Include(b => b.Copies)
                .Include("Copies.Member")
                .ToList()[0];
        }
        return View(book);
    }
   
    public async Task<ActionResult> CheckoutBook(CheckoutSelection selection)
    {
        BookCopy bookCopy = new BookCopy();
        if (VerifyMemberId(selection.MemberId))
        {
            if (selection.BookCopyId != null)
            {
                bookCopy = _dbContext.BookCopies
                    .Where(b=>b.Id == selection.BookCopyId)
                    .Include("Member")
                    .Include("Book")
                    .ToList()[0];
                bookCopy.Member = _dbContext.Members.Find(selection.MemberId);
                bookCopy.Book.NumOfAvailableCopies -= 1;
                _dbContext.SaveChanges();
            }
        }

        Console.WriteLine("Done");
        return View("Index");
    }

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
    }*/
    private bool VerifyMemberId(string memberId)
    {
        return (_dbContext.Members.Find(memberId) != null);
    }
    
    /*private BookInfo SearchBookById(string bookId)
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
                book.NumOfAvailableCopies += 1;
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
                    NumOfAvailableCopies = 1,
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
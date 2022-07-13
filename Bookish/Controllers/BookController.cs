using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookish.Controllers;

public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly EFCore _dbContext;
    

    public BookController(ILogger<BookController> logger, EFCore dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    

    public IActionResult AddBook()
    {
        return View();
    }
    
    public IActionResult CheckoutSelection()
    {
        return View();
    }
    
    
    
    
    

    
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
            book.Searches += 1;
            _dbContext.SaveChanges();
        }
        return View(book);
    }
   
    public async Task<ActionResult> CheckoutBook(CheckoutSelection selection)
    {
        BookCopy bookCopy = new BookCopy();
        if (VerifyMemberId(selection.MemberId))
        {
            if (selection.BookCopyId != null && (_dbContext.BookCopies.Find(selection.BookCopyId) != null))
            {
                bookCopy = _dbContext.BookCopies
                    .Where(b=>b.Id == selection.BookCopyId)
                    .Include("Member")
                    .Include("Book")
                    .ToList()[0];
                bookCopy.Member = _dbContext.Members.Find(selection.MemberId);
                bookCopy.Book.NumOfAvailableCopies -= 1;
                _dbContext.SaveChanges();
                return View("Index");
            }
            else
            {
                return View("ErrorMsg",new ErrorMsgModel("Invalid Book ID"));
            }
            
        }
        else
        {
            return View("ErrorMsg",new ErrorMsgModel("Invalid Member ID"));
        }

        
        
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
    
    
    public async Task<ActionResult> Catalogue()
    {
        //string memberId = member.MemberId;

        var AllBooksList = new ListOfBooks();
        AllBooksList.AllBooks = _dbContext.Books.ToList().OrderBy(x => x.Searches).ToList();
        AllBooksList.AllBooks = Enumerable.Reverse(AllBooksList.AllBooks).ToList();
        
        if (AllBooksList != null)
        {
            return View(AllBooksList);
        }
        
        return View(AllBooksList);
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
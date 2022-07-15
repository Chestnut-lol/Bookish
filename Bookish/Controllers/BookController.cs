using System.Diagnostics;
using System;
using System.Globalization;
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

    
    [HttpGet]
    public IActionResult AddBook()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult CheckoutSelection()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult EditBook(string bookId)
    {
        using (var context = new EFCore())
        {
            var book = context.Books.Find(bookId);
            return View(book);
        }
    }
    
    
    [HttpGet]
    public IActionResult DeleteBook(string bookId)
    {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == bookId);
            return View(book);
    }
    
    [HttpGet]
    public IActionResult GetBook(BookSelection selection)
    {
        Book book = new Book();
        if (selection.Id != null)
        {
            book = _dbContext.Books.SingleOrDefault(x => x.Id == selection.Id);
            book.Searches += 1;
            _dbContext.SaveChanges();
        }
        return View(book);
    }
   
    /*public async Task<ActionResult> CheckoutBook(CheckoutSelection selection)
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
    public IActionResult AddBookInput(BookInput input)
    {
        string bookId = input.Id;
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == bookId);
            if (book != null)
            {
                book.NumOfCopies += 1;
                book.NumOfAvailableCopies += 1;
            }
            else
            { 
                book = new Book()
                {
                    Id = (_dbContext.Books.Count() + 1).ToString(),
                    Title = input.Title,
                    Author = input.Author,
                    NumOfCopies = 1,
                    NumOfAvailableCopies = 1,
                };
                _dbContext.Books.Add(book);
            }
            _dbContext.SaveChanges();
        return View();
    }

    [HttpPost] 
    public IActionResult BookSuccessfullyEdited(Book input)
    {
        string bookId = input.Id;
        var book = _dbContext.Books.SingleOrDefault(x => x.Id == bookId);

        if (book == null) return View("BookNotEdited");
            
        if (input.Title != null) book.Title = input.Title;
        if (input.Author != null) book.Author = input.Author;
        _dbContext.SaveChanges();
        return View();
    }
    
    [HttpPost]
    public IActionResult BookSuccessfullyDeleted(Book input)
    {
        string bookId = input.Id;
        var book = _dbContext.Books.SingleOrDefault(x => x.Id == bookId);
        if (book == null) return View("BookNotDeleted");
        
        var bookCopies = _dbContext.BookCopies.Where(b=>b.Book.Id == input.Id).Include("Member").Include("Book").ToList();

        foreach (BookCopy copy in bookCopies)
        {
            _dbContext.BookCopies.Remove(copy);
        }
        
        _dbContext.Books.Remove(book);
        _dbContext.SaveChanges();
        return View();
    }

    [HttpGet]
    public IActionResult Catalogue()
    {
        //string memberId = member.MemberId;

        var AllBooksList = new ListOfBooksForCatalogue();
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
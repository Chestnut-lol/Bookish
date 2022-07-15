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
    
    //This action wont be used in this version
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
        using (var context = new EFCore())
        {
            var book = context.Books.Find(bookId);
            return View(book);
        }
    }
    
    [HttpGet]
    public IActionResult CopiesOfBook(string bookId)
    {
        var book = _dbContext.Books.Include(b => b.Copies).Include("Copies.Member").SingleOrDefault(b => b.Id == bookId);
        return View(book);

    }
    
    [HttpGet]
    public IActionResult ToCheckout(string bookCopyId)
    {
        var bookCopy = _dbContext.BookCopies.Include(b => b.Book).Include(b => b.Member).SingleOrDefault(b => b.Id == bookCopyId);
        return View(bookCopy);
    }
    
    
    [HttpPost]
    public IActionResult CheckoutCopy(BookCopy checkedOutCopy)
    {
        var bookCopy = _dbContext.BookCopies.Include(b => b.Book).Include(b => b.Member).SingleOrDefault(x => x.Id == checkedOutCopy.Id);
        var member = _dbContext.Members.SingleOrDefault(x => x.MemberId == checkedOutCopy.Member.MemberId);
        //book.NumOfAvailableCopies -= 1;
        bookCopy.Member = member;
        bookCopy.DueDate = DateTime.Now.AddDays(14);
        _dbContext.SaveChanges();
        
        var book = _dbContext.Books.Include(b => b.Copies).Include("Copies.Member").SingleOrDefault(b => b.Id == bookCopy.Book.Id);
        return View("CopiesOfBook", book);
    }
    
    [HttpGet]
    public IActionResult CheckInCopy(string bookCopyId)
    {
        var bookCopy = _dbContext.BookCopies.Include(b => b.Book).Include(b => b.Member).SingleOrDefault(x => x.Id == bookCopyId);
        //book.NumOfAvailableCopies -= 1;
        bookCopy.Member = null;
        _dbContext.SaveChanges();
        
        var book = _dbContext.Books.Include(b => b.Copies).Include("Copies.Member").SingleOrDefault(b => b.Id == bookCopy.Book.Id);
        return View("CopiesOfBook", book);
    }
    
    [HttpGet]
    public IActionResult CheckoutBook(BookCopy memberId)
    {
        using (var context = new EFCore())
        {
            var bookCopy = _dbContext.BookCopies.Where(x => x.Id == memberId.Id).ToList()[0];
            //var book = _dbContext.Books.Where(x => x.Id == bookCopy.Book.Id).ToList()[0];
            var member = _dbContext.Members.Where(x => x.MemberId == memberId.Member.MemberId).ToList()[0];
            //book.NumOfAvailableCopies -= 1;
            bookCopy.Member = member;
            bookCopy.DueDate = DateTime.Now.AddDays(14);
            _dbContext.SaveChanges();

            
            var AllBooksList = new ListOfBooks();
            AllBooksList.AllBooks = _dbContext.Books.ToList().OrderBy(x => x.Searches).ToList();
            AllBooksList.AllBooks = Enumerable.Reverse(AllBooksList.AllBooks).ToList();


            return View("Catalogue", AllBooksList);
        }
    }
    
    [HttpGet]
    public IActionResult AddCopy(Book numberOfCopies)
    {
        int newCopies = numberOfCopies.NumOfCopies;
        using (var context = new EFCore())
        {
            var book = _dbContext.Books.Where(x => x.Id == numberOfCopies.Id).Include(b => b.Copies).Include("Copies.Member").ToList()[0];
            for (int i = numberOfCopies.Copies.Count; i < numberOfCopies.Copies.Count + newCopies; i++)
            {
                BookCopy newCopy = new BookCopy();
                newCopy.Id =  GetTimestamp(DateTime.Now) + i;
                newCopy.Book = book;
                //context.BookCopies.Add(newCopy);
                book.Copies.Add(newCopy);
                book.NumOfAvailableCopies += 1;
            }
            _dbContext.SaveChanges();
            //context.SaveChanges();
            return View("CopiesOfBook", book);
        }
    }
    
    
    
    
    private static String GetTimestamp(DateTime value)
    {
        return value.ToString("yyMMddHHmmssff");
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
                    Id = GetTimestamp(DateTime.Now),
                    Title = input.Title,
                    Author = input.Author,
                    NumOfCopies = 1,
                    NumOfAvailableCopies = 1,
                };
                context.Books.Add(book);
            }
            context.SaveChanges();
        }
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> BookEdited(Book input)
    {
        string bookId = input.Id;
        using (var context = new EFCore())
        {
            var book = context.Books.Find(bookId);
            if (book != null)
            {
                if (input.Title != null) book.Title = input.Title;
                if (input.Author != null) book.Author = input.Author;
                context.SaveChanges();
                return View();
            }

            return View("BookNotEdited");
        }
    }
    
    [HttpGet]
    public async Task<ActionResult> BookDeleted(Book input)
    {
        string bookId = input.Id;
        using (var context = new EFCore())
        {
            var book = context.Books.Find(bookId);
            var bookCopies = _dbContext.BookCopies.Where(b=>b.Book.Id == input.Id).Include("Member").Include("Book").ToList();
            
            if (book != null)
            {
                foreach (BookCopy copy in bookCopies)
                {
                    context.BookCopies.Remove(copy);
                }
                context.Books.Remove(book);
                context.SaveChanges();
                return View();
            }

            return View("BookNotEdited");
        }
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
    
    /*
    public async Task<ActionResult> EditBook(Book book)
    {
        //string memberId = member.MemberId;

        //var AllBooksList = new ListOfBooks();
        //AllBooksList.AllBooks = _dbContext.Books.ToList().OrderBy(x => x.Searches).ToList();
        //AllBooksList.AllBooks = Enumerable.Reverse(AllBooksList.AllBooks).ToList();
        
        //if (AllBooksList != null)
        {
            return View();
        }
        
        return View();
    }
    */

    [HttpPost]
    public async Task<ActionResult> CheckAction(string btnString)
    {
        if (btnString == "edit")
        {
            return View("EditBook");
        }
        else if (btnString == "delete")
        {
            return View("DeleteBook");
        }

        return View("Catalogue");
    }
    
    [HttpGet]
    public IActionResult DeleteCopy(string bookCopyId, string bookId)
    {
        var copy = _dbContext.BookCopies.SingleOrDefault(b => b.Id == bookCopyId);

        if (copy == null) return View("BookNotEdited");
            
       _dbContext.BookCopies.Remove(copy);
       _dbContext.SaveChanges();
       
       var book = _dbContext.Books.Include(b => b.Copies).Include("Copies.Member").SingleOrDefault(b => b.Id == bookId);

       return View("CopiesOfBook", book);
    }
    


    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
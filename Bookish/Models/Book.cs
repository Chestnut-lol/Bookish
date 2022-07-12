namespace Bookish.Models;

public class Book
{ 
    public string Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int NumOfCopies { get; set; }
    public int NumOfAvailableCopies { get; set; }
    
    public virtual List<BookCopy> Copies { get; set; }
    //public BookCopy BookCopy { get; set; }
}
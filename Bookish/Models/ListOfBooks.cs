namespace Bookish.Models;

public class ListOfBooks
{
    public int num { get; set; }
    public virtual List<Book> AllBooks { get; set; }
}
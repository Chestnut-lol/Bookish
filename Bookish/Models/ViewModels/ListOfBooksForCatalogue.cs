namespace Bookish.Models;

public class ListOfBooksForCatalogue
{
    public int num { get; set; }
    public virtual List<Book> AllBooks { get; set; }
}
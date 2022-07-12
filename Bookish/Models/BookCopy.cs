namespace Bookish.Models;

public class BookCopy
{
    public string Id { get; set; }
    //public string BookId { get; set; }
    public virtual Book Book { get; set;  }
    public virtual Member Member { set; get; }
}
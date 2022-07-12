namespace Bookish.Models;

public class Member
{
    public string MemberId { get; set; }
    public ICollection<Book> Books { get; set; }
}
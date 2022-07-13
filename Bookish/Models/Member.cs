namespace Bookish.Models;

public class Member
{
    public string MemberId { get; set; }
    public virtual List<BookCopy> Books { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
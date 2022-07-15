namespace Bookish.Models;

public class MemberQueryByNameOrEmailModel
{
    public List<Member> ListOfMembers { get; set; }

    public MemberQueryByNameOrEmailModel(List<Member> listOfMembers)
    {
        ListOfMembers = listOfMembers;
    }
}
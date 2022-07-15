namespace Bookish.Models;

public class MemberCatalogueModel
{
    public List<Member> ListOfMembers { get; set; }

    public MemberCatalogueModel(List<Member> listOfMembers)
    {
        ListOfMembers = listOfMembers;
    }
}
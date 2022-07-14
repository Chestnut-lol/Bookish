using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;

namespace Bookish.Controllers;

public class MemberController : Controller
{
    private readonly ILogger<MemberController> _logger;
    private readonly EFCore _dbContext;
    

    public MemberController(ILogger<MemberController> logger, EFCore dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult MemberSearch()
    {
        return View();
    }
    
    
    public IActionResult AddMember()
    {
        return View();
    }
    
    public IActionResult MemberCatalogue()
    {
        return View(_dbContext.Members.ToList());
    }
    
    
    
    public IActionResult MemberQuery(Member member)
    {
        if (member.MemberId != null)
        {
            return SearchMemberById(member.MemberId);
        }
        else if (member.Name != null)
        {
            return SearchMemberByName(member.Name);
        }
        else if (member.Email != null)
        {
            return SearchMemberByEmail(member.Email);
        }
        else
        {
            return View("ErrorMsg", new ErrorMsgModel("You did not input anything! :("));
        }
    }

    private IActionResult SearchMemberById(string memberId)
    {
        var memberModel = _dbContext.Members.SingleOrDefault(m => m.MemberId == memberId);
        if (memberModel == null)
        {
            return View("ErrorMsg", new ErrorMsgModel("Member not found."));
        }

        var resultMember = _dbContext.Members
            .Where(x => x.MemberId == memberId)
            .Include(m=>m.Books)
            .Include("Books.Book")
            .ToList()[0];
        return View("MemberQuery", resultMember);
    }
    
    private IActionResult SearchMemberByName(string name)
    {
        var members = _dbContext.Members.Where(m => m.Name == name);
        
        return View("MemberQueryByNameOrEmail", members);
    }
    
    private IActionResult SearchMemberByEmail(string email)
    {
        var members = _dbContext.Members.Where(m => m.Email == email);
        
        return View("MemberQueryByNameOrEmail", members);
    }

    public IActionResult EditMember(string memberId)
    {
        var member = _dbContext.Members.SingleOrDefault(m => m.MemberId == memberId);
        return View(member);
    }
    
    public IActionResult ConfirmDeleteMember(string memberId)
    {
        var member = _dbContext.Members.Where(m=>m.MemberId == memberId)
            .Include("Books")
            .ToList()[0];
        _dbContext.SaveChanges();
        if (member.Books.Count != 0)
        {
            return View("ErrorMsg", new ErrorMsgModel("Unable to delete the member as lendings are not empty."));
        }
        return View("ConfirmDeleteMember",member);
    }
    
    public IActionResult DeleteMember(Member member)
    {
        _dbContext.Members.Remove(member);
        _dbContext.SaveChanges();
        return View("ErrorMsg", new ErrorMsgModel("Member deleted."));
    }
    
    public IActionResult MemberChange(Member inputMember)
    {
        // var member = _dbContext.Members.Find(memberId);
        var member = _dbContext.Members
            .Where(x => x.MemberId == inputMember.MemberId)
            .Include(m=>m.Books)
            .ThenInclude(b=>b.Book)
            .ToList()[0];
        member.Name = inputMember.Name;
        member.Email = inputMember.Email;
        _dbContext.SaveChanges();
        return View("MemberQuery", member );
    }
    
    private bool VerifyMemberId(string memberId)
    {
        return (_dbContext.Members.Find(memberId) != null);
    }
    
    
    public IActionResult MemberInput(Member member)
    {
        string memberId = GetTimestamp(DateTime.Now);
        member.MemberId = memberId;
        _dbContext.Members.Add(member);
        _dbContext.SaveChanges();
        return View("ErrorMsg", new ErrorMsgModel($"Member added. Your member ID is {member.MemberId}."));
    }
    public static String GetTimestamp(DateTime value)
    {
        return value.ToString("yyMMddHHmmssff");
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
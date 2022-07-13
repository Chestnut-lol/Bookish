using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bookish.Models;
using Microsoft.EntityFrameworkCore;

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
    
    
    public async Task<ActionResult> MemberQuery(Member member)
    {
        if (_dbContext.Members.Find(member.MemberId) == null)
        {
            return View("ErrorMsg", new ErrorMsgModel("Member not found."));
        }

        var resultMember = _dbContext.Members
            .Where(x => x.MemberId == member.MemberId)
            .Include(m=>m.Books)
            .Include("Books.Book")
            .ToList()[0];
        return View(resultMember);
    }

    private bool VerifyMemberId(string memberId)
    {
        return (_dbContext.Members.Find(memberId) != null);
    }
    
    
    public async Task<ActionResult> MemberInput(Member member)
    {
        string memberId = (int.Parse(_dbContext.Members
            .OrderBy(m=>m.MemberId)
            .Last().MemberId) + 1).ToString();
        member.MemberId = memberId;
        _dbContext.Members.Add(member);
        _dbContext.SaveChanges();
        return View("ErrorMsg", new ErrorMsgModel($"Member added. Your member ID is {memberId}."));
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
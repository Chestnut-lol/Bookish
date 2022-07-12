using Bookish.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Bookish;

public class EFCore : DbContext
{
    //public DbSet<BookInfo> Books { get; set; }
    public DbSet<Member> Members { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
        optionsBuilder.UseSqlServer(@"Server=localhost;Database=myDataBase;Trusted_Connection=True;");
    }
}

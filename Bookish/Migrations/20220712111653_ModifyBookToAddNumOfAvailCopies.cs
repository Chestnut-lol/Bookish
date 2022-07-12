using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookish.Migrations
{
    public partial class ModifyBookToAddNumOfAvailCopies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumOfAvailableCopies",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumOfAvailableCopies",
                table: "Books");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookish.Migrations
{
    public partial class ModifyBooks4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopy_Books_bookId",
                table: "BookCopy");

            migrationBuilder.RenameColumn(
                name: "bookId",
                table: "BookCopy",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopy_bookId",
                table: "BookCopy",
                newName: "IX_BookCopy_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopy_Books_BookId",
                table: "BookCopy",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopy_Books_BookId",
                table: "BookCopy");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookCopy",
                newName: "bookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopy_BookId",
                table: "BookCopy",
                newName: "IX_BookCopy_bookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopy_Books_bookId",
                table: "BookCopy",
                column: "bookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

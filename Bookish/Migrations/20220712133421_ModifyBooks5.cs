using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookish.Migrations
{
    public partial class ModifyBooks5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopy_Books_BookId",
                table: "BookCopy");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCopy_Members_MemberId",
                table: "BookCopy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCopy",
                table: "BookCopy");

            migrationBuilder.RenameTable(
                name: "BookCopy",
                newName: "BookCopies");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopy_MemberId",
                table: "BookCopies",
                newName: "IX_BookCopies_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopy_BookId",
                table: "BookCopies",
                newName: "IX_BookCopies_BookId");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "BookCopies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCopies",
                table: "BookCopies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Books_BookId",
                table: "BookCopies",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Members_MemberId",
                table: "BookCopies",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Books_BookId",
                table: "BookCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Members_MemberId",
                table: "BookCopies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookCopies",
                table: "BookCopies");

            migrationBuilder.RenameTable(
                name: "BookCopies",
                newName: "BookCopy");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopies_MemberId",
                table: "BookCopy",
                newName: "IX_BookCopy_MemberId");

            migrationBuilder.RenameIndex(
                name: "IX_BookCopies_BookId",
                table: "BookCopy",
                newName: "IX_BookCopy_BookId");

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "BookCopy",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookCopy",
                table: "BookCopy",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopy_Books_BookId",
                table: "BookCopy",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopy_Members_MemberId",
                table: "BookCopy",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

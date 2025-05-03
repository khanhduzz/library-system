using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBContextForBorrowBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_Book_UserId",
                table: "BorrowedBook");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_User_BookId",
                table: "BorrowedBook");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBook_Book_BookId",
                table: "BorrowedBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBook_User_UserId",
                table: "BorrowedBook",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_Book_BookId",
                table: "BorrowedBook");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_User_UserId",
                table: "BorrowedBook");

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBook_Book_UserId",
                table: "BorrowedBook",
                column: "UserId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBook_User_BookId",
                table: "BorrowedBook",
                column: "BookId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

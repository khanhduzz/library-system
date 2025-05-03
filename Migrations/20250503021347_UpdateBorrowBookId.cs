using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBorrowBookId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowedBook",
                table: "BorrowedBook");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BorrowedBook",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowedBook",
                table: "BorrowedBook",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedBook_UserId",
                table: "BorrowedBook",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BorrowedBook",
                table: "BorrowedBook");

            migrationBuilder.DropIndex(
                name: "IX_BorrowedBook_UserId",
                table: "BorrowedBook");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BorrowedBook");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BorrowedBook",
                table: "BorrowedBook",
                columns: new[] { "UserId", "BookId" });
        }
    }
}

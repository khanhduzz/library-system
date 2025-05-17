using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookInventoryBorrowedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_Book_BookId",
                table: "BorrowedBook");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_User_UserId",
                table: "BorrowedBook");

            migrationBuilder.DropColumn(
                name: "BorrowCount",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "BorrowedDate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "BorrowerId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "TotalCopies",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BorrowedBook",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowedBook_BookId",
                table: "BorrowedBook",
                newName: "IX_BorrowedBook_InventoryId");

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCopies",
                table: "Inventory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBook_Inventory_InventoryId",
                table: "BorrowedBook",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BorrowedBook_User_UserId",
                table: "BorrowedBook",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_Inventory_InventoryId",
                table: "BorrowedBook");

            migrationBuilder.DropForeignKey(
                name: "FK_BorrowedBook_User_UserId",
                table: "BorrowedBook");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "TotalCopies",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "BorrowedBook",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BorrowedBook_InventoryId",
                table: "BorrowedBook",
                newName: "IX_BorrowedBook_BookId");

            migrationBuilder.AddColumn<int>(
                name: "BorrowCount",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowedDate",
                table: "Book",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BorrowerId",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Condition",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Book",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalCopies",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}

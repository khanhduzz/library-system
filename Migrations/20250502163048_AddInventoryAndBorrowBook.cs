using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryAndBorrowBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "Book",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TotalCopies",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BorrowedBook",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowedBook", x => new { x.UserId, x.BookId });
                    table.ForeignKey(
                        name: "FK_BorrowedBook_Book_UserId",
                        column: x => x.UserId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BorrowedBook_User_BookId",
                        column: x => x.BookId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AvailableCopies = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BorrowedBook_BookId",
                table: "BorrowedBook",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_BookId",
                table: "Inventory",
                column: "BookId");

            migrationBuilder.InsertData(
            table: "Book",
            columns: new[] { "Id", "Title", "Description", "AuthorId", "ISBN", "PublishDate", "Image" },
            values: new object[,]
            {
                { 1, "The Da Vinci Code", "A symbologist uncovers a Vatican secret society's clues through Leonardo da Vinci's art.", 1, "9780307474278", new DateTime(2003, 3, 18), null },
                { 2, "Angels & Demons", "Robert Langdon investigates the Illuminati and a plot against the Vatican.", 1, "9780743493468", new DateTime(2000, 5, 1), null },
                { 3, "The Silence of the Lambs", "FBI trainee Clarice Starling consults Hannibal Lecter to catch a serial killer.", 2, "9780312924584", new DateTime(1988, 8, 29), null },
                { 4, "Red Dragon", "An FBI profiler seeks the help of Hannibal Lecter to catch a brutal murderer.", 2, "9780425228227", new DateTime(1981, 10, 1), null },
                { 5, "Murder on the Orient Express", "Detective Hercule Poirot solves a murder aboard a luxury train.", 3, "9780062693662", new DateTime(1934, 1, 1), null },
                { 6, "And Then There Were None", "Ten strangers are invited to an island and begin dying one by one.", 3, "9780062073488", new DateTime(1939, 11, 6), null },
                { 7, "Harry Potter and the Sorcerer's Stone", "A boy discovers he is a wizard and attends Hogwarts School of Witchcraft and Wizardry.", 4, "9780590353427", new DateTime(1997, 6, 26), null },
                { 8, "Harry Potter and the Chamber of Secrets", "Harry returns for a second year at Hogwarts to discover a dark secret.", 4, "9780439064873", new DateTime(1998, 7, 2), null },
                { 9, "The Shining", "A haunted hotel drives a man to madness while his son sees ghosts.", 5, "9780307743657", new DateTime(1977, 1, 28), null },
                { 10, "It", "A group of friends battles an evil entity in the form of a clown.", 5, "9781501142970", new DateTime(1986, 9, 15), null },
                { 11, "A Game of Thrones", "Noble families vie for control of the Iron Throne in a fantasy world.", 6, "9780553573404", new DateTime(1996, 8, 6), null },
                { 12, "A Clash of Kings", "Power struggles intensify as rival claimants to the throne rise.", 6, "9780553579901", new DateTime(1998, 11, 16), null },
                { 13, "The Fellowship of the Ring", "A hobbit begins a perilous quest to destroy a powerful ring.", 7, "9780618640157", new DateTime(1954, 7, 29), null },
                { 14, "The Two Towers", "The fellowship is broken, and Middle-earth faces greater threats.", 7, "9780618640188", new DateTime(1954, 11, 11), null },
                { 15, "Kafka on the Shore", "Two characters’ surreal journeys intertwine in a metaphysical world.", 8, "9781400079278", new DateTime(2002, 9, 12), null },
                { 16, "Norwegian Wood", "A young man reflects on love and loss in 1960s Tokyo.", 8, "9780375704024", new DateTime(1987, 9, 4), null },
                { 17, "Pride and Prejudice", "A spirited young woman navigates manners, morality, and marriage.", 9, "9780141439518", new DateTime(1813, 1, 28), null },
                { 18, "Sense and Sensibility", "Two sisters balance heart and reason in the pursuit of love.", 9, "9780141439662", new DateTime(1811, 10, 30), null },
                { 19, "The Old Man and the Sea", "An aging fisherman battles a giant marlin far out in the Gulf Stream.", 10, "9780684801223", new DateTime(1952, 9, 1), null },
                { 20, "A Farewell to Arms", "A love story set during World War I between an American soldier and a nurse.", 10, "9780684801469", new DateTime(1929, 12, 22), null }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i <= 20; i++)
            {
                migrationBuilder.DeleteData(
                    table: "Book",
                    keyColumn: "Id",
                    keyValue: i);
            }

            migrationBuilder.DropTable(
                name: "BorrowedBook");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropColumn(
                name: "TotalCopies",
                table: "Book");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishDate",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}

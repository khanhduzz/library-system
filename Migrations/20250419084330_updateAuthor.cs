using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibrarySystem.Migrations
{
    /// <inheritdoc />
    public partial class updateAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
            table: "Author",
            columns: new[] { "Id", "Name", "Description" },
            values: new object[,]
            {
                { 1, "Dan Brown", "American author best known for thriller novels like 'The Da Vinci Code'." },
                { 2, "Thomas Harris", "American writer known for the Hannibal Lecter series, including 'The Silence of the Lambs'." },
                { 3, "Agatha Christie", "British writer famous for her detective novels featuring Hercule Poirot and Miss Marple." },
                { 4, "J.K. Rowling", "British author who created the 'Harry Potter' fantasy series." },
                { 5, "Stephen King", "American author known for his horror, supernatural fiction, and suspense novels." },
                { 6, "George R.R. Martin", "American novelist and screenwriter known for 'A Song of Ice and Fire' series." },
                { 7, "J.R.R. Tolkien", "English writer and philologist best known for 'The Lord of the Rings' trilogy." },
                { 8, "Haruki Murakami", "Japanese author whose work blends magical realism with themes of loneliness and surrealism." },
                { 9, "Jane Austen", "English novelist known for romantic fiction set among the British landed gentry." },
                { 10, "Ernest Hemingway", "American novelist and short-story writer famous for his economical and understated style." }
            });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            for (int id = 1; id <= 10; id++)
            {
                migrationBuilder.DeleteData(
                    table: "Author",
                    keyColumn: "Id",
                    keyValue: id
                );
            }

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

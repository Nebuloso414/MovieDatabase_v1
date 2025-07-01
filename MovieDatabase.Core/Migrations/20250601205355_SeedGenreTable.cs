using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieDatabase.Core.Migrations
{
    /// <inheritdoc />
    public partial class SeedGenreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Movies that will have you at the edge of your seat!", "Action" },
                    { 2, "Movies that will make you cry of laughter", "Comedy" },
                    { 3, "Movies that will make you cry...but not of laughter", "Drama" },
                    { 4, "Movies that will make you turn on all the lights", "Horror" },
                    { 5, "Movies that will challenge your imagination", "Sci-Fi" },
                    { 6, "Movies with incredible stories and characters", "Fantasy" },
                    { 7, "Movies that will educate you", "Documentary" },
                    { 8, "Movies that will keep you on the edge of your seat!", "Thriller" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}

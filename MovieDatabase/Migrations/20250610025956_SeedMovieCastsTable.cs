using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieDatabase.Migrations
{
    /// <inheritdoc />
    public partial class SeedMovieCastsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MovieCasts",
                columns: new[] { "MovieId", "PersonId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 1, 2, 1 },
                    { 1, 13, 2 },
                    { 2, 5, 1 },
                    { 2, 6, 1 },
                    { 2, 11, 2 },
                    { 2, 12, 2 },
                    { 3, 8, 1 },
                    { 3, 9, 1 },
                    { 3, 10, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 1, 1, 1 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 1, 2, 1 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 1, 13, 2 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 2, 5, 1 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 2, 6, 1 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 2, 11, 2 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 2, 12, 2 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 3, 8, 1 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 3, 9, 1 });

            migrationBuilder.DeleteData(
                table: "MovieCasts",
                keyColumns: new[] { "MovieId", "PersonId", "RoleId" },
                keyValues: new object[] { 3, 10, 2 });
        }
    }
}

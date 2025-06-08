using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieDatabase.Migrations
{
    /// <inheritdoc />
    public partial class SeedPeopleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(1974, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leonardo", "DiCaprio" },
                    { 2, new DateTime(1981, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "Joseph", "Gordon-Levitt" },
                    { 3, new DateTime(1988, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emma", "Stone" },
                    { 4, new DateTime(1981, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Natalie", "Portman" },
                    { 5, new DateTime(1964, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Keanu", "Reeves" },
                    { 6, new DateTime(1961, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laurence", "Fishburne" },
                    { 7, new DateTime(1964, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Keanu", "Reeves" },
                    { 8, new DateTime(1958, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tim", "Robbins" },
                    { 9, new DateTime(1937, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Morgan", "Freeman" },
                    { 10, new DateTime(1959, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frank", "Darabont" },
                    { 11, new DateTime(1965, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lana", "Wachowski" },
                    { 12, new DateTime(1967, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lilly", "Wachowski" },
                    { 13, new DateTime(1970, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christopher", "Nolan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Lab2.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Publisher",
                columns: new[] { "ID", "PublisherName" },
                values: new object[,]
                {
                    { 1, "Humanitas" },
                    { 2, "Nemira" },
                    { 3, "Arthur" }
                });

            migrationBuilder.InsertData(
                table: "Book",
                columns: new[] { "ID", "Author", "Price", "PublisherID", "PublishingDate", "Title" },
                values: new object[,]
                {
                    { 1, "Marin Preda", 39.90m, 1, new DateTime(2020, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Moromeții" },
                    { 2, "Frank Herbert", 59.90m, 2, new DateTime(2019, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dune" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Book",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Publisher",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Publisher",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publisher",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUD_App.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Username" },
                values: new object[] { new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ramesh" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Username" },
                values: new object[] { new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Manoj" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Username" },
                values: new object[] { new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "alice" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Username" },
                values: new object[] { new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "bob" });
        }
    }
}

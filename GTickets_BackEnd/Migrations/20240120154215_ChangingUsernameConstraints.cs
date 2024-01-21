using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GTickets_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ChangingUsernameConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "629fcb18-0727-428e-9109-fba7de24dc99", "1", "Admin", "Admin" },
                    { "f6dfce08-6f44-4c98-944a-bc55335d5933", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "629fcb18-0727-428e-9109-fba7de24dc99");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6dfce08-6f44-4c98-944a-bc55335d5933");
        }
    }
}

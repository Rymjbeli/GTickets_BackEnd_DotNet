using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GTickets_BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "200b0749-c34d-446c-8a28-4580004233df");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a0890839-756a-4b80-9daf-32623d50fca4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2c6efb11-1fd8-4f39-ae0d-4226ba0e0dbc", "1", "Admin", "Admin" },
                    { "944fc8e0-23d7-4d60-bfdc-0445828a0ec0", "2", "User", "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c6efb11-1fd8-4f39-ae0d-4226ba0e0dbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "944fc8e0-23d7-4d60-bfdc-0445828a0ec0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "200b0749-c34d-446c-8a28-4580004233df", "2", "User", "User" },
                    { "a0890839-756a-4b80-9daf-32623d50fca4", "1", "Admin", "Admin" }
                });
        }
    }
}

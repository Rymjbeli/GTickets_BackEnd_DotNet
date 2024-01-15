using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GTickets_BackEnd.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_CustomUser_UserId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_CustomUser_UserId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_CustomUser_UserId",
                table: "Replies",
                column: "UserId",
                principalTable: "CustomUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_CustomUser_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "CustomUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_CustomUser_UserId",
                table: "Replies");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_CustomUser_UserId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_CustomUser_UserId",
                table: "Replies",
                column: "UserId",
                principalTable: "CustomUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_CustomUser_UserId",
                table: "Tickets",
                column: "UserId",
                principalTable: "CustomUser",
                principalColumn: "Id");
        }
    }
}

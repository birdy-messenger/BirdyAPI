using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class Dialogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChatNumber",
                table: "ChatUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DialogUsers",
                columns: table => new
                {
                    DialogID = table.Column<Guid>(nullable: false),
                    FirstUserID = table.Column<int>(nullable: false),
                    SecondUserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogUsers", x => x.DialogID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DialogUsers");

            migrationBuilder.DropColumn(
                name: "ChatNumber",
                table: "ChatUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class IdGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");
            migrationBuilder.AddColumn<Guid>(
                name: "Token",
                table: "Users",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Token",
                table: "Users",
                nullable: false,
                oldClrType: typeof(Guid));
        }
    }
}

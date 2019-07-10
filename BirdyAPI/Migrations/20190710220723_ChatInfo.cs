using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class ChatInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chat");

            migrationBuilder.CreateTable(
                name: "ChatInfo",
                columns: table => new
                {
                    ChatID = table.Column<Guid>(nullable: false),
                    ChatName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatInfo", x => x.ChatID);
                });

            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    ChatID = table.Column<Guid>(nullable: false),
                    UserInChatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => new { x.ChatID, x.UserInChatID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatInfo");

            migrationBuilder.DropTable(
                name: "ChatUsers");

            migrationBuilder.CreateTable(
                name: "Chat",
                columns: table => new
                {
                    ChatID = table.Column<Guid>(nullable: false),
                    UserinChatID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat", x => new { x.ChatID, x.UserinChatID });
                });
        }
    }
}

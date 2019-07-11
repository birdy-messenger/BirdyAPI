using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    ChatID = table.Column<Guid>(nullable: false),
                    MessageID = table.Column<Guid>(nullable: false),
                    AuthorID = table.Column<int>(nullable: false),
                    SendDate = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}

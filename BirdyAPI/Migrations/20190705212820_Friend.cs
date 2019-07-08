using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class Friend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    FirstUserID = table.Column<int>(nullable: false),
                    SecondUserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.FirstUserID, x.SecondUserID });
                });
        }
    }
}

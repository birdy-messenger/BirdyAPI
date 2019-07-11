using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class renameAndTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniqueTag",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueTag",
                table: "Users");
        }
    }
}

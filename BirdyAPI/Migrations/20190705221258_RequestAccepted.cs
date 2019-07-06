using Microsoft.EntityFrameworkCore.Migrations;

namespace BirdyAPI.Migrations
{
    public partial class RequestAccepted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequestAccepted",
                table: "UserFriend",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestAccepted",
                table: "UserFriend");
        }
    }
}

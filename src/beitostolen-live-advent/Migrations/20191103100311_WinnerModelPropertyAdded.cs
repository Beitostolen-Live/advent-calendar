using Microsoft.EntityFrameworkCore.Migrations;

namespace beitostolen_live_api.Migrations
{
    public partial class WinnerModelPropertyAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnlyWithCorrectAnswer",
                table: "Winners",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnlyWithCorrectAnswer",
                table: "Winners");
        }
    }
}

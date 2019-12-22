using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class AddedIsStartedPropertyInTOurnament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Tournaments");

            migrationBuilder.AddColumn<bool>(
                name: "IsStarted",
                table: "Tournaments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStarted",
                table: "Tournaments");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

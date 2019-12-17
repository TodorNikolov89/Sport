using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class addedPlacePropertyInTournament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "Tournaments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Place",
                table: "Tournaments");
        }
    }
}

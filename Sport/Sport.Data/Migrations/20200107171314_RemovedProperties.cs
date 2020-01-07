using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class RemovedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPlayerGames",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "FirstPlayerPoints",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "FirstPlayerSets",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SecondPlayerGames",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SecondPlayerPoints",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SecondPlayerSets",
                table: "Results");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerGames",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerPoints",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerSets",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerGames",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerPoints",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerSets",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

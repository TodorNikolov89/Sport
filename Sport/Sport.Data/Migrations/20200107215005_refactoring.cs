using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirsPlayerSets",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "SecondPlayerSets",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "FirstPlayerTieBreakPoints",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "IsTieBreak",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SecondPlayerTieBreakPoints",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "FirsPlayerGames",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SeconPlayerGames",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "FirsPlayerTieBreakPoints",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerGames",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTieBreak",
                table: "Sets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerGames",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerTieBreakPoints",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerSets",
                table: "Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerSets",
                table: "Results",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirsPlayerPoints",
                table: "Games",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerPoints",
                table: "Games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirsPlayerTieBreakPoints",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "FirstPlayerGames",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "IsTieBreak",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "SecondPlayerGames",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "SecondPlayerTieBreakPoints",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "FirstPlayerSets",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SecondPlayerSets",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "FirsPlayerPoints",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "SecondPlayerPoints",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "FirsPlayerSets",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerSets",
                table: "Sets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerTieBreakPoints",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTieBreak",
                table: "Results",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerTieBreakPoints",
                table: "Results",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirsPlayerGames",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeconPlayerGames",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

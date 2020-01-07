using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class newMigratin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPlayerGames",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FirstPlayerPoints",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FirstPlayerSets",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FirstPlayerTieBreakPoints",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "IsTieBreak",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondPlayerGames",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondPlayerPoints",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondPlayerSets",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondPlayerTieBreakPoints",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Result",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstPlayerPoints = table.Column<string>(nullable: true),
                    SecondPlayerPoints = table.Column<string>(nullable: true),
                    FirstPlayerGames = table.Column<int>(nullable: false),
                    SecondPlayerGames = table.Column<int>(nullable: false),
                    FirstPlayerSets = table.Column<int>(nullable: false),
                    SecondPlayerSets = table.Column<int>(nullable: false),
                    FirstPlayerTieBreakPoints = table.Column<int>(nullable: false),
                    SecondPlayerTieBreakPoints = table.Column<int>(nullable: false),
                    IsTieBreak = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Result", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ResultId",
                table: "Matches",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Result_ResultId",
                table: "Matches",
                column: "ResultId",
                principalTable: "Result",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Result_ResultId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Result");

            migrationBuilder.DropIndex(
                name: "IX_Matches_ResultId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerGames",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstPlayerPoints",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerSets",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerTieBreakPoints",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTieBreak",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerGames",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SecondPlayerPoints",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerSets",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerTieBreakPoints",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

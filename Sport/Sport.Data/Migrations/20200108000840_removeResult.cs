using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class removeResult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Results_MatchResultId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Results_ResultId",
                table: "Sets");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Sets_ResultId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchResultId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "MatchResultId",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Sets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerSets",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondPlayerSets",
                table: "Matches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sets_MatchId",
                table: "Sets",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Matches_MatchId",
                table: "Sets",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_Matches_MatchId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_MatchId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "FirstPlayerSets",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondPlayerSets",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Sets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MatchResultId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstPlayerSets = table.Column<int>(type: "int", nullable: false),
                    MatchId = table.Column<int>(type: "int", nullable: false),
                    SecondPlayerSets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sets_ResultId",
                table: "Sets",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_MatchResultId",
                table: "Matches",
                column: "MatchResultId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Results_MatchResultId",
                table: "Matches",
                column: "MatchResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_Results_ResultId",
                table: "Sets",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

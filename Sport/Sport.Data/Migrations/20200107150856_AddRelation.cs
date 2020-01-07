using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class AddRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Matches_MatchId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_MatchId",
                table: "Results");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Results_MatchResultId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_MatchResultId",
                table: "Matches");

            migrationBuilder.CreateIndex(
                name: "IX_Results_MatchId",
                table: "Results",
                column: "MatchId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Matches_MatchId",
                table: "Results",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

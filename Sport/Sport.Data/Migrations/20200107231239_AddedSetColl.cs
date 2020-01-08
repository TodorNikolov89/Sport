using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class AddedSetColl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "Sets",
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
        }
    }
}

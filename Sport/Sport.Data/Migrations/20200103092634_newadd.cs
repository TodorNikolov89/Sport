using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class newadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstPlayerId = table.Column<string>(nullable: true),
                    FirstUserId = table.Column<string>(nullable: true),
                    SecondPlayerId = table.Column<string>(nullable: true),
                    SecondUserId = table.Column<string>(nullable: true),
                    UmpireId = table.Column<string>(nullable: true),
                    TournamentId = table.Column<int>(nullable: false),
                    FirstPlayerPoints = table.Column<string>(nullable: true),
                    SecondPlayerPoints = table.Column<string>(nullable: true),
                    FirstPlayerGames = table.Column<int>(nullable: false),
                    SecondPlayerGames = table.Column<int>(nullable: false),
                    FirstPlayerSets = table.Column<int>(nullable: false),
                    SecondPlayerSets = table.Column<int>(nullable: false),
                    FirstPlayerTieBreakPoints = table.Column<int>(nullable: false),
                    SecondPlayerTieBreakPoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_FirstUserId",
                        column: x => x.FirstUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_SecondUserId",
                        column: x => x.SecondUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_UmpireId",
                        column: x => x.UmpireId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FirstUserId",
                table: "Matches",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SecondUserId",
                table: "Matches",
                column: "SecondUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UmpireId",
                table: "Matches",
                column: "UmpireId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");
        }
    }
}

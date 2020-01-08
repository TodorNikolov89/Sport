using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class addedTieBreak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTieBreak",
                table: "Sets");

            migrationBuilder.AddColumn<bool>(
                name: "HasTieBreak",
                table: "Sets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TieBreakId",
                table: "Points",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TieBreak",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TieBreak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TieBreak_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_TieBreakId",
                table: "Points",
                column: "TieBreakId");

            migrationBuilder.CreateIndex(
                name: "IX_TieBreak_SetId",
                table: "TieBreak",
                column: "SetId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Points_TieBreak_TieBreakId",
                table: "Points",
                column: "TieBreakId",
                principalTable: "TieBreak",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_TieBreak_TieBreakId",
                table: "Points");

            migrationBuilder.DropTable(
                name: "TieBreak");

            migrationBuilder.DropIndex(
                name: "IX_Points_TieBreakId",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "HasTieBreak",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "TieBreakId",
                table: "Points");

            migrationBuilder.AddColumn<bool>(
                name: "IsTieBreak",
                table: "Sets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

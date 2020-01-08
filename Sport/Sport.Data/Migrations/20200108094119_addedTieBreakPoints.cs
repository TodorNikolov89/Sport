using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class addedTieBreakPoints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Points_TieBreak_TieBreakId",
                table: "Points");

            migrationBuilder.DropForeignKey(
                name: "FK_TieBreak_Sets_SetId",
                table: "TieBreak");

            migrationBuilder.DropIndex(
                name: "IX_Points_TieBreakId",
                table: "Points");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TieBreak",
                table: "TieBreak");

            migrationBuilder.DropColumn(
                name: "TieBreakId",
                table: "Points");

            migrationBuilder.RenameTable(
                name: "TieBreak",
                newName: "TieBreaks");

            migrationBuilder.RenameIndex(
                name: "IX_TieBreak_SetId",
                table: "TieBreaks",
                newName: "IX_TieBreaks_SetId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TieBreaks",
                table: "TieBreaks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TieBreakPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstPlayerPoint = table.Column<int>(nullable: false),
                    SecondPlayerpoint = table.Column<int>(nullable: false),
                    TieBreakId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TieBreakPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TieBreakPoints_TieBreaks_TieBreakId",
                        column: x => x.TieBreakId,
                        principalTable: "TieBreaks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TieBreakPoints_TieBreakId",
                table: "TieBreakPoints",
                column: "TieBreakId");

            migrationBuilder.AddForeignKey(
                name: "FK_TieBreaks_Sets_SetId",
                table: "TieBreaks",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TieBreaks_Sets_SetId",
                table: "TieBreaks");

            migrationBuilder.DropTable(
                name: "TieBreakPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TieBreaks",
                table: "TieBreaks");

            migrationBuilder.RenameTable(
                name: "TieBreaks",
                newName: "TieBreak");

            migrationBuilder.RenameIndex(
                name: "IX_TieBreaks_SetId",
                table: "TieBreak",
                newName: "IX_TieBreak_SetId");

            migrationBuilder.AddColumn<int>(
                name: "TieBreakId",
                table: "Points",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TieBreak",
                table: "TieBreak",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Points_TieBreakId",
                table: "Points",
                column: "TieBreakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Points_TieBreak_TieBreakId",
                table: "Points",
                column: "TieBreakId",
                principalTable: "TieBreak",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TieBreak_Sets_SetId",
                table: "TieBreak",
                column: "SetId",
                principalTable: "Sets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

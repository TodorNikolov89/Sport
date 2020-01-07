using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class addedPopertyIsFinished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Results_ResultId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ResultId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ResultId",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsSetFinished",
                table: "Sets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsGameFinished",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSetFinished",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "IsGameFinished",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "ResultId",
                table: "Games",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ResultId",
                table: "Games",
                column: "ResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Results_ResultId",
                table: "Games",
                column: "ResultId",
                principalTable: "Results",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

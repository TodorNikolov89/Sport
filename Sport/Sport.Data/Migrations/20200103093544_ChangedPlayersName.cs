using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class ChangedPlayersName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_FirstUserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_SecondUserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_FirstUserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_SecondUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "FirstUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SecondUserId",
                table: "Matches");

            migrationBuilder.AlterColumn<string>(
                name: "SecondPlayerId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstPlayerId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FirstPlayerId",
                table: "Matches",
                column: "FirstPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SecondPlayerId",
                table: "Matches",
                column: "SecondPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_FirstPlayerId",
                table: "Matches",
                column: "FirstPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_SecondPlayerId",
                table: "Matches",
                column: "SecondPlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_FirstPlayerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_SecondPlayerId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_FirstPlayerId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_SecondPlayerId",
                table: "Matches");

            migrationBuilder.AlterColumn<string>(
                name: "SecondPlayerId",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstPlayerId",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstUserId",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondUserId",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_FirstUserId",
                table: "Matches",
                column: "FirstUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SecondUserId",
                table: "Matches",
                column: "SecondUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_FirstUserId",
                table: "Matches",
                column: "FirstUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_SecondUserId",
                table: "Matches",
                column: "SecondUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

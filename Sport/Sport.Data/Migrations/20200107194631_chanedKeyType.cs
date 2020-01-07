using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class chanedKeyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_PlayerId1",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_AspNetUsers_PlayerId1",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_PlayerId1",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayerId1",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "Sets",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PlayerId",
                table: "Games",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_PlayerId",
                table: "Sets",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerId",
                table: "Games",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_PlayerId",
                table: "Games",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_AspNetUsers_PlayerId",
                table: "Sets",
                column: "PlayerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_AspNetUsers_PlayerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Sets_AspNetUsers_PlayerId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Sets_PlayerId",
                table: "Sets");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayerId",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Sets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId1",
                table: "Sets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Games",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId1",
                table: "Games",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sets_PlayerId1",
                table: "Sets",
                column: "PlayerId1");

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerId1",
                table: "Games",
                column: "PlayerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_AspNetUsers_PlayerId1",
                table: "Games",
                column: "PlayerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_AspNetUsers_PlayerId1",
                table: "Sets",
                column: "PlayerId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

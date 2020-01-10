using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class changess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirsPlayerPoints",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerPoints",
                table: "Games",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPlayerPoints",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "FirsPlayerPoints",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

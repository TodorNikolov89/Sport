using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class changedPointName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirsPlayerPoints",
                table: "Points");

            migrationBuilder.AddColumn<int>(
                name: "FirstPlayerPoints",
                table: "Points",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstPlayerPoints",
                table: "Points");

            migrationBuilder.AddColumn<int>(
                name: "FirsPlayerPoints",
                table: "Points",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

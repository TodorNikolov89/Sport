using Microsoft.EntityFrameworkCore.Migrations;

namespace Sport.Data.Migrations
{
    public partial class ChangedPropertyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondPlayerpoint",
                table: "TieBreakPoints",
                newName: "SecondPlayerPoint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondPlayerPoint",
                table: "TieBreakPoints",
                newName: "SecondPlayerpoint");
        }
    }
}

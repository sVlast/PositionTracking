using Microsoft.EntityFrameworkCore.Migrations;

namespace PositionTracking.Data.Migrations
{
    public partial class AddProjectScreenshotUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "webisteScreenshotUrl",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "webisteScreenshotUrl",
                table: "Projects");
        }
    }
}

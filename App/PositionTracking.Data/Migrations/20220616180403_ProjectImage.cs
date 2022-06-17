using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PositionTracking.Data.Migrations
{
    public partial class ProjectImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProjectImage",
                table: "Projects",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectImage",
                table: "Projects");
        }
    }
}

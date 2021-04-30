using System;
using Microsoft.EntityFrameworkCore.Migrations;



namespace PositionTracking.Migrations
{
    public partial class InitialModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserPermissionId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Paths = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "Keyword",
                columns: table => new
                {
                    KeywordId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keyword", x => x.KeywordId);
                    table.ForeignKey(
                        name: "FK_Keyword_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                columns: table => new
                {
                    UserPermissionId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermissionType = table.Column<byte>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.UserPermissionId);
                    table.ForeignKey(
                        name: "FK_UserPermission_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KeywordEntry",
                columns: table => new
                {
                    KeywordEntryId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Language = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    KeywordId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordEntry", x => x.KeywordEntryId);
                    table.ForeignKey(
                        name: "FK_KeywordEntry_Keyword_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keyword",
                        principalColumn: "KeywordId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KeywordRating",
                columns: table => new
                {
                    KeywordRatingId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    SearchEngine = table.Column<string>(nullable: true),
                    KeywordEntryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordRating", x => x.KeywordRatingId);
                    table.ForeignKey(
                        name: "FK_KeywordRating_KeywordEntry_KeywordEntryId",
                        column: x => x.KeywordEntryId,
                        principalTable: "KeywordEntry",
                        principalColumn: "KeywordEntryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserPermissionId",
                table: "AspNetUsers",
                column: "UserPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Keyword_ProjectId",
                table: "Keyword",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordEntry_KeywordId",
                table: "KeywordEntry",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordRating_KeywordEntryId",
                table: "KeywordRating",
                column: "KeywordEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_ProjectId",
                table: "UserPermission",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserPermission_UserPermissionId",
                table: "AspNetUsers",
                column: "UserPermissionId",
                principalTable: "UserPermission",
                principalColumn: "UserPermissionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserPermission_UserPermissionId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "KeywordRating");

            migrationBuilder.DropTable(
                name: "UserPermission");

            migrationBuilder.DropTable(
                name: "KeywordEntry");

            migrationBuilder.DropTable(
                name: "Keyword");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserPermissionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserPermissionId",
                table: "AspNetUsers");
        }
    }
}

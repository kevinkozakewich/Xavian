using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Xavian.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Territories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abbreviation = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoogleAuthenticationId = table.Column<string>(nullable: true),
                    MicrosoftAuthenticationId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    WebAccess = table.Column<bool>(nullable: false),
                    DbAdmin = table.Column<bool>(nullable: false),
                    LastLoggedInDateTime = table.Column<DateTime>(nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_LastUpdatedUserId",
                        column: x => x.LastUpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(nullable: false),
                    Class = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    FurtherDetails = table.Column<string>(nullable: true),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_Users_LastUpdatedUserId",
                        column: x => x.LastUpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    TerritoryId = table.Column<long>(nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sites_Users_LastUpdatedUserId",
                        column: x => x.LastUpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sites_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sites_Territories_TerritoryId",
                        column: x => x.TerritoryId,
                        principalTable: "Territories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Users_LastUpdatedUserId",
                        column: x => x.LastUpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Teams_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SitePermissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<long>(nullable: false),
                    TeamId = table.Column<long>(nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SitePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SitePermissions_Users_LastUpdatedUserId",
                        column: x => x.LastUpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SitePermissions_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SitePermissions_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SitePermissions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false),
                    Manager = table.Column<bool>(nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedUserId = table.Column<long>(nullable: true),
                    OwnerUserId = table.Column<long>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Users_LastUpdatedUserId",
                        column: x => x.LastUpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppVersions",
                columns: new[] { "Id", "Version" },
                values: new object[] { 1L, 1 });

            migrationBuilder.InsertData(
                table: "Territories",
                columns: new[] { "Id", "Abbreviation", "Deleted", "FullName", "LastUpdatedDateTime", "LastUpdatedUserId", "OwnerUserId" },
                values: new object[,]
                {
                    { 1L, "CCA", false, "Central Canada", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 2L, "ECA", false, "Eastern Canada", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 3L, "WCA", false, "Western Canada", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 4L, "USA", false, "USA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 5L, "OTHER", false, "Other", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logs_LastUpdatedUserId",
                table: "Logs",
                column: "LastUpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_OwnerUserId",
                table: "Logs",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePermissions_LastUpdatedUserId",
                table: "SitePermissions",
                column: "LastUpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePermissions_OwnerUserId",
                table: "SitePermissions",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePermissions_SiteId",
                table: "SitePermissions",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SitePermissions_TeamId_SiteId",
                table: "SitePermissions",
                columns: new[] { "TeamId", "SiteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_LastUpdatedUserId",
                table: "Sites",
                column: "LastUpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_OwnerUserId",
                table: "Sites",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_TerritoryId",
                table: "Sites",
                column: "TerritoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_LastUpdatedUserId",
                table: "Teams",
                column: "LastUpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_OwnerUserId_Name",
                table: "Teams",
                columns: new[] { "OwnerUserId", "Name" },
                unique: true,
                filter: "[OwnerUserId] IS NOT NULL AND [Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_LastUpdatedUserId",
                table: "TeamUsers",
                column: "LastUpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_OwnerUserId",
                table: "TeamUsers",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_UserId",
                table: "TeamUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamUsers_TeamId_UserId",
                table: "TeamUsers",
                columns: new[] { "TeamId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastUpdatedUserId",
                table: "Users",
                column: "LastUpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OwnerUserId",
                table: "Users",
                column: "OwnerUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVersions");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "SitePermissions");

            migrationBuilder.DropTable(
                name: "TeamUsers");

            migrationBuilder.DropTable(
                name: "Sites");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Territories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

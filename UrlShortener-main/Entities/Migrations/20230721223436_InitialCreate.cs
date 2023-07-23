using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Urls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalUrl = table.Column<string>(type: "TEXT", nullable: false),
                    UrlToken = table.Column<string>(type: "TEXT", nullable: false),
                    ShortenedUrl = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Urls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UrlVisits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VisitorIPAddress = table.Column<string>(type: "TEXT", nullable: true),
                    VisitorUserAgent = table.Column<string>(type: "TEXT", nullable: true),
                    VisitorReferer = table.Column<string>(type: "TEXT", nullable: true),
                    ShortenedUrlId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UrlVisits_Urls_ShortenedUrlId",
                        column: x => x.ShortenedUrlId,
                        principalTable: "Urls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UrlVisits_ShortenedUrlId",
                table: "UrlVisits",
                column: "ShortenedUrlId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UrlVisits");

            migrationBuilder.DropTable(
                name: "Urls");
        }
    }
}

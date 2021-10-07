using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KarmaBot.Migrations
{
    public partial class v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Karma",
                columns: table => new
                {
                    KarmaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KarmaCount = table.Column<long>(type: "bigint", nullable: false),
                    HighestKarma = table.Column<long>(type: "bigint", nullable: false),
                    HighestKarmaDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LowestKarma = table.Column<long>(type: "bigint", nullable: false),
                    LowestKarmaDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karma", x => x.KarmaId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlackUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KarmaId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Karma_KarmaId",
                        column: x => x.KarmaId,
                        principalTable: "Karma",
                        principalColumn: "KarmaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_KarmaId",
                table: "Users",
                column: "KarmaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Karma");
        }
    }
}

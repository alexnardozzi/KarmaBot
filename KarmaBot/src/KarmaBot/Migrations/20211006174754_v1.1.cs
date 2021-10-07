using Microsoft.EntityFrameworkCore.Migrations;

namespace KarmaBot.Migrations
{
    public partial class v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Karma_KarmaId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_KarmaId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "KarmaId",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Karma",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Karma_UserId",
                table: "Karma",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Karma_Users_UserId",
                table: "Karma",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Karma_Users_UserId",
                table: "Karma");

            migrationBuilder.DropIndex(
                name: "IX_Karma_UserId",
                table: "Karma");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Karma");

            migrationBuilder.AddColumn<long>(
                name: "KarmaId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_KarmaId",
                table: "Users",
                column: "KarmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Karma_KarmaId",
                table: "Users",
                column: "KarmaId",
                principalTable: "Karma",
                principalColumn: "KarmaId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

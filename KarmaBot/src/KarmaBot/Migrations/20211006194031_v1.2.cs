using Microsoft.EntityFrameworkCore.Migrations;

namespace KarmaBot.Migrations
{
    public partial class v12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NegativeKarmaGiven",
                table: "Karma",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PositiveKarmaGiven",
                table: "Karma",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NegativeKarmaGiven",
                table: "Karma");

            migrationBuilder.DropColumn(
                name: "PositiveKarmaGiven",
                table: "Karma");
        }
    }
}

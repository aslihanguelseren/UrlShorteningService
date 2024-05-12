using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShorteningService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomShortUrl",
                table: "UrlMaps",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomShortUrl",
                table: "UrlMaps");
        }
    }
}

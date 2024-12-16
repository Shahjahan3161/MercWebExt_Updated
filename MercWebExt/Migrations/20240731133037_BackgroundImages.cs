using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercWebExt.Migrations
{
    /// <inheritdoc />
    public partial class BackgroundImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundImagePath",
                schema: "MercWebAdmin",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundImagePath",
                schema: "MercWebAdmin",
                table: "Settings");
        }
    }
}

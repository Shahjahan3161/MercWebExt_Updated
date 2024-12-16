using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercWebExt.Migrations
{
    /// <inheritdoc />
    public partial class isCorrectAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isCorrectAnswer",
                schema: "dbo",
                table: "Induction.Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCorrectAnswer",
                schema: "dbo",
                table: "Induction.Question");
        }
    }
}

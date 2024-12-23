﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercWebExt.Migrations
{
    /// <inheritdoc />
    public partial class GifFileIncluded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FrontAdImagePath",
                schema: "MercWebAdmin",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontAdImagePath",
                schema: "MercWebAdmin",
                table: "Settings");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plunger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeStarted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TimeStarted",
                table: "GameStatuses",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStarted",
                table: "GameStatuses");
        }
    }
}

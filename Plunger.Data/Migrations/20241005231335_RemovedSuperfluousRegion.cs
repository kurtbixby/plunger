using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plunger.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSuperfluousRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "CollectionGames");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Region",
                table: "CollectionGames",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}

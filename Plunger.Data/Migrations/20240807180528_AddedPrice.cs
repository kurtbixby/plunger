using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plunger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "CollectionGames",
                type: "numeric(20,0)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "CollectionGames");
        }
    }
}

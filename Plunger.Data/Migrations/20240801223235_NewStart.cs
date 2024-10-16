using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plunger.Data.Migrations
{
    /// <inheritdoc />
    public partial class NewStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegionName",
                table: "ReleaseDate",
                newName: "Region");

            migrationBuilder.CreateTable(
                name: "GameRegion",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    RegionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRegion", x => new { x.GameId, x.RegionsId });
                    table.ForeignKey(
                        name: "FK_GameRegion_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRegion_Regions_RegionsId",
                        column: x => x.RegionsId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRegion_RegionsId",
                table: "GameRegion",
                column: "RegionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameRegion");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "ReleaseDate",
                newName: "RegionName");
        }
    }
}

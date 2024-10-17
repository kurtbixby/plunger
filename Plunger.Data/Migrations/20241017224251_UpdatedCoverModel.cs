using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plunger.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCoverModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Covers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Covers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

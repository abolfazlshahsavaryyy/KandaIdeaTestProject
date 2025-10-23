using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalLandManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatureToCorner3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "index",
                table: "Corners",
                newName: "Index");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Index",
                table: "Corners",
                newName: "index");
        }
    }
}

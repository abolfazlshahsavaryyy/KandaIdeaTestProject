using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalLandManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OwnerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Production = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Area = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Corners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    LandId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Corners_Lands_LandId",
                        column: x => x.LandId,
                        principalTable: "Lands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CornerImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    CornerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CornerImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CornerImages_Corners_CornerId",
                        column: x => x.CornerId,
                        principalTable: "Corners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CornerImages_CornerId",
                table: "CornerImages",
                column: "CornerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Corners_LandId",
                table: "Corners",
                column: "LandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CornerImages");

            migrationBuilder.DropTable(
                name: "Corners");

            migrationBuilder.DropTable(
                name: "Lands");
        }
    }
}

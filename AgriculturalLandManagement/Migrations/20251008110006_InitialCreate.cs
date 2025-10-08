using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriculturalLandManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    X1 = table.Column<double>(type: "REAL", nullable: false),
                    Y1 = table.Column<double>(type: "REAL", nullable: false),
                    X2 = table.Column<double>(type: "REAL", nullable: false),
                    Y2 = table.Column<double>(type: "REAL", nullable: false),
                    Area = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lands", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lands");
        }
    }
}

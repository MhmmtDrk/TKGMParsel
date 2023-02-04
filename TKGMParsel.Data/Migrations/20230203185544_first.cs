using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TKGMParsel.Data.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "Varchar(50)", maxLength: 50, nullable: true),
                    TKGMValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parsel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ilceAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mevkii = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ilId = table.Column<int>(type: "int", nullable: true),
                    durum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parselId = table.Column<int>(type: "int", nullable: true),
                    zeminKmdurum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zeminId = table.Column<int>(type: "int", nullable: true),
                    parselNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nitelik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mahalleAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gittigiParselListe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gittigiParselSebep = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    alan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adaNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ilceId = table.Column<int>(type: "int", nullable: true),
                    ilAd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mahalleId = table.Column<int>(type: "int", nullable: true),
                    pafta = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parsel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "Varchar(50)", maxLength: 50, nullable: true),
                    TKGMValue = table.Column<int>(type: "int", nullable: true),
                    TKGMCityValue = table.Column<int>(type: "int", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Street",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "Varchar(50)", maxLength: 50, nullable: true),
                    TKGMValue = table.Column<int>(type: "int", nullable: true),
                    TKGMDistrictValue = table.Column<int>(type: "int", nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Street", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Street_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_District_CityId",
                table: "District",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Street_DistrictId",
                table: "Street",
                column: "DistrictId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parsel");

            migrationBuilder.DropTable(
                name: "Street");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "City");
        }
    }
}

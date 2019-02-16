using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRTest.Migrations
{
    public partial class RemovedIDManagementUsersTEmporarily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AddressDto");

            migrationBuilder.DropTable(
                name: "GeoCoordinatesDto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressDto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    cityName = table.Column<string>(nullable: true),
                    route = table.Column<string>(nullable: true),
                    stateName = table.Column<string>(nullable: true),
                    streetName = table.Column<string>(nullable: true),
                    streetNumber = table.Column<int>(nullable: false),
                    zipcode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeoCoordinatesDto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    latitude = table.Column<double>(nullable: false),
                    longitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeoCoordinatesDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(nullable: true),
                    addressDtoId = table.Column<int>(nullable: true),
                    clientConnection = table.Column<string>(nullable: true),
                    connectionStatus = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    firstName = table.Column<string>(nullable: true),
                    geoCoordinatesDtoId = table.Column<int>(nullable: true),
                    lastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AddressDto_addressDtoId",
                        column: x => x.addressDtoId,
                        principalTable: "AddressDto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_GeoCoordinatesDto_geoCoordinatesDtoId",
                        column: x => x.geoCoordinatesDtoId,
                        principalTable: "GeoCoordinatesDto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_addressDtoId",
                table: "Users",
                column: "addressDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_geoCoordinatesDtoId",
                table: "Users",
                column: "geoCoordinatesDtoId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRTest.Migrations
{
    public partial class UpdatedUserTableAndAddedApplicationUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "addressDtoId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "clientConnection",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "connectionStatus",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "firstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "geoCoordinatesDtoId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lastName",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AddressDto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    streetNumber = table.Column<int>(nullable: false),
                    streetName = table.Column<string>(nullable: true),
                    route = table.Column<string>(nullable: true),
                    cityName = table.Column<string>(nullable: true),
                    stateName = table.Column<string>(nullable: true),
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
                name: "IdManagementUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdManagementUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_addressDtoId",
                table: "Users",
                column: "addressDtoId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_geoCoordinatesDtoId",
                table: "Users",
                column: "geoCoordinatesDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_AddressDto_addressDtoId",
                table: "Users",
                column: "addressDtoId",
                principalTable: "AddressDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_GeoCoordinatesDto_geoCoordinatesDtoId",
                table: "Users",
                column: "geoCoordinatesDtoId",
                principalTable: "GeoCoordinatesDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_AddressDto_addressDtoId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_GeoCoordinatesDto_geoCoordinatesDtoId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AddressDto");

            migrationBuilder.DropTable(
                name: "GeoCoordinatesDto");

            migrationBuilder.DropTable(
                name: "IdManagementUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_addressDtoId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_geoCoordinatesDtoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "addressDtoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "clientConnection",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "connectionStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "firstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "geoCoordinatesDtoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "lastName",
                table: "Users");
        }
    }
}

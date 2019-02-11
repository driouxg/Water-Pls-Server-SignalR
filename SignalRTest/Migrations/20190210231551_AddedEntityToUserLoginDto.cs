using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRTest.Migrations
{
    public partial class AddedEntityToUserLoginDto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userLoginId",
                table: "IdManagementUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserLoginDto",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(nullable: false),
                    email = table.Column<string>(nullable: false),
                    password = table.Column<string>(maxLength: 100, nullable: false),
                    rememberMe = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginDto", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IdManagementUsers_userLoginId",
                table: "IdManagementUsers",
                column: "userLoginId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdManagementUsers_UserLoginDto_userLoginId",
                table: "IdManagementUsers",
                column: "userLoginId",
                principalTable: "UserLoginDto",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdManagementUsers_UserLoginDto_userLoginId",
                table: "IdManagementUsers");

            migrationBuilder.DropTable(
                name: "UserLoginDto");

            migrationBuilder.DropIndex(
                name: "IX_IdManagementUsers_userLoginId",
                table: "IdManagementUsers");

            migrationBuilder.DropColumn(
                name: "userLoginId",
                table: "IdManagementUsers");
        }
    }
}

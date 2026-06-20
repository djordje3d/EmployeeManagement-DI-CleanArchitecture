using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Name" },
                values: new object[,]
                {
                    { 1, "CanAccessKvalifikacije" },
                    { 2, "CanAccessOrganizacioneJedinice" },
                    { 3, "CanAccessKvalifikacije" },
                    { 4, "CanAddKvalifikacije" }
                });

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 28, 15, 21, 19, 755, DateTimeKind.Local).AddTicks(1385));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 28, 15, 21, 19, 755, DateTimeKind.Local).AddTicks(1466));

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 16, 23, 46, 56, 366, DateTimeKind.Local).AddTicks(7412));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 16, 23, 46, 56, 366, DateTimeKind.Local).AddTicks(7478));
        }
    }
}

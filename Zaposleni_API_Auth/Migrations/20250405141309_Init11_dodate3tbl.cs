using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init11_dodate3tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 16, 13, 7, 771, DateTimeKind.Local).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 16, 13, 7, 771, DateTimeKind.Local).AddTicks(7511));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 3, 27, 16, 3, 23, 761, DateTimeKind.Local).AddTicks(8065));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 3, 27, 16, 3, 23, 761, DateTimeKind.Local).AddTicks(8134));
        }
    }
}

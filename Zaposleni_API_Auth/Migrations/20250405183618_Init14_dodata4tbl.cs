using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init14_dodata4tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 20, 36, 16, 379, DateTimeKind.Local).AddTicks(7502));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 20, 36, 16, 379, DateTimeKind.Local).AddTicks(7578));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 16, 28, 25, 538, DateTimeKind.Local).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 16, 28, 25, 538, DateTimeKind.Local).AddTicks(7196));
        }
    }
}

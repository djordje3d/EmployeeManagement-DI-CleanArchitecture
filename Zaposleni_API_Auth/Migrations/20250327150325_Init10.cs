using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Kraj_RadnogOd",
                table: "Zaposleni",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Kraj_RadnogOd", "Pocetak_RadnogOd" },
                values: new object[] { null, new DateTime(2025, 3, 27, 16, 3, 23, 761, DateTimeKind.Local).AddTicks(8065) });

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Kraj_RadnogOd", "Pocetak_RadnogOd" },
                values: new object[] { null, new DateTime(2025, 3, 27, 16, 3, 23, 761, DateTimeKind.Local).AddTicks(8134) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Kraj_RadnogOd",
                table: "Zaposleni",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Kraj_RadnogOd", "Pocetak_RadnogOd" },
                values: new object[] { new DateTime(2025, 3, 27, 1, 40, 51, 940, DateTimeKind.Local).AddTicks(377), new DateTime(2025, 3, 27, 1, 40, 51, 940, DateTimeKind.Local).AddTicks(324) });

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Kraj_RadnogOd", "Pocetak_RadnogOd" },
                values: new object[] { new DateTime(2025, 3, 27, 1, 40, 51, 940, DateTimeKind.Local).AddTicks(390), new DateTime(2025, 3, 27, 1, 40, 51, 940, DateTimeKind.Local).AddTicks(389) });
        }
    }
}

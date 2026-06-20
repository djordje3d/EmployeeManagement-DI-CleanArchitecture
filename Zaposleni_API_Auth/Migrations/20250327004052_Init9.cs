using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefon",
                table: "Zaposleni",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Roditelj",
                table: "Zaposleni",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Prezime",
                table: "Zaposleni",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Ime",
                table: "Zaposleni",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Adresa",
                table: "Zaposleni",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "A_P",
                table: "Zaposleni",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Telefon",
                table: "Zaposleni",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Roditelj",
                table: "Zaposleni",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "Prezime",
                table: "Zaposleni",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Ime",
                table: "Zaposleni",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<string>(
                name: "Adresa",
                table: "Zaposleni",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "A_P",
                table: "Zaposleni",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Kraj_RadnogOd", "Pocetak_RadnogOd" },
                values: new object[] { new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1180), new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1124) });

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Kraj_RadnogOd", "Pocetak_RadnogOd" },
                values: new object[] { new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1189), new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1188) });
        }
    }
}

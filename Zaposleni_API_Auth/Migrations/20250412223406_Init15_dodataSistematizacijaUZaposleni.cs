using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init15_dodataSistematizacijaUZaposleni : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SistematizacijeId",
                table: "Zaposleni",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Sistematizacije",
                columns: new[] { "Id", "Beneficirani_Radni_Staz", "Bodovi", "KlasifikacijaZanimanjaId", "Koeficijent", "KvalifikacijaId", "NazivRadnogMesta", "Opis", "OrganizacioneJediniceId", "Radno_Iskustvo" },
                values: new object[,]
                {
                    { 1, 0, 100, null, 2.500m, 1, "Programer", "Opis radnog mesta za programera.", 1, "5 godina" },
                    { 2, 0, 80, null, 1.850m, 1, "Tehničar", "Opis za tehničara.", 1, "3 godine" }
                });

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Pocetak_RadnogOd", "SistematizacijeId" },
                values: new object[] { new DateTime(2025, 4, 13, 0, 34, 3, 808, DateTimeKind.Local).AddTicks(8127), 1 });

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Pocetak_RadnogOd", "SistematizacijeId" },
                values: new object[] { new DateTime(2025, 4, 13, 0, 34, 3, 808, DateTimeKind.Local).AddTicks(8196), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_SistematizacijeId",
                table: "Zaposleni",
                column: "SistematizacijeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zaposleni_Sistematizacije_SistematizacijeId",
                table: "Zaposleni",
                column: "SistematizacijeId",
                principalTable: "Sistematizacije",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zaposleni_Sistematizacije_SistematizacijeId",
                table: "Zaposleni");

            migrationBuilder.DropIndex(
                name: "IX_Zaposleni_SistematizacijeId",
                table: "Zaposleni");

            migrationBuilder.DeleteData(
                table: "Sistematizacije",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Sistematizacije",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "SistematizacijeId",
                table: "Zaposleni");

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 20, 40, 49, 954, DateTimeKind.Local).AddTicks(1739));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 20, 40, 49, 954, DateTimeKind.Local).AddTicks(1803));
        }
    }
}

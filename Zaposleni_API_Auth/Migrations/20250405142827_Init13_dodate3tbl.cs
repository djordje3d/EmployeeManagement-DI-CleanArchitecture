using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init13_dodate3tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClanoviDomacinstva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeClana = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PolClana = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: true),
                    SroClana = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DatumRodjenjaClana = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusClana = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Roditelj = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ZaposleniId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanoviDomacinstva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClanoviDomacinstva_Zaposleni_ZaposleniId",
                        column: x => x.ZaposleniId,
                        principalTable: "Zaposleni",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GrupeMestaTroskova",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grupa = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupeMestaTroskova", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacioneJedinice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OJ = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GrMestaTroskovaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacioneJedinice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacioneJedinice_GrupeMestaTroskova_GrMestaTroskovaId",
                        column: x => x.GrMestaTroskovaId,
                        principalTable: "GrupeMestaTroskova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GrupeMestaTroskova",
                columns: new[] { "Id", "Grupa", "Naziv" },
                values: new object[,]
                {
                    { 1, "01", "FIRMA" },
                    { 2, "02", "GENERALNI DIREKTOR" },
                    { 3, "03", "PROIZVODNJA" }
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ClanoviDomacinstva_ZaposleniId",
                table: "ClanoviDomacinstva",
                column: "ZaposleniId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacioneJedinice_GrMestaTroskovaId",
                table: "OrganizacioneJedinice",
                column: "GrMestaTroskovaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanoviDomacinstva");

            migrationBuilder.DropTable(
                name: "OrganizacioneJedinice");

            migrationBuilder.DropTable(
                name: "GrupeMestaTroskova");

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 1,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 16, 15, 11, 154, DateTimeKind.Local).AddTicks(7522));

            migrationBuilder.UpdateData(
                table: "Zaposleni",
                keyColumn: "Id",
                keyValue: 2,
                column: "Pocetak_RadnogOd",
                value: new DateTime(2025, 4, 5, 16, 15, 11, 154, DateTimeKind.Local).AddTicks(7584));
        }
    }
}

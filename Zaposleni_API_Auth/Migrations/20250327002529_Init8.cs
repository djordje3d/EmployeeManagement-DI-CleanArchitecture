using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kvalifikacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicniStepenKv = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kvalifikacije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mesta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Opstina = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PostanskiBroj = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mesta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zaposleni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Roditelj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Pocetak_RadnogOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kraj_RadnogOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vrsta_RadnogOdnosa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A_P = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KvalifikacijaId = table.Column<int>(type: "int", nullable: true),
                    MestoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposleni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Kvalifikacije_KvalifikacijaId",
                        column: x => x.KvalifikacijaId,
                        principalTable: "Kvalifikacije",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Zaposleni_Mesta_MestoId",
                        column: x => x.MestoId,
                        principalTable: "Mesta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Mesta",
                columns: new[] { "Id", "Naziv", "Opstina", "PostanskiBroj" },
                values: new object[,]
                {
                    { 1, "Beograd", "Vračar", "11000" },
                    { 2, "Novi Sad", "Stari Grad", "21000" }
                });

            migrationBuilder.InsertData(
                table: "Zaposleni",
                columns: new[] { "Id", "A_P", "Adresa", "DatumRodjenja", "Ime", "JMBG", "Kraj_RadnogOd", "KvalifikacijaId", "MestoId", "Pocetak_RadnogOd", "Prezime", "Roditelj", "Telefon", "Vrsta_RadnogOdnosa" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(1987, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marko", "2311487134221", new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1180), null, 1, new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1124), "Negovanovic", "Uros", null, null },
                    { 2, null, null, new DateTime(1960, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivan", "0210960849567", new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1189), null, 2, new DateTime(2025, 3, 27, 1, 25, 29, 124, DateTimeKind.Local).AddTicks(1188), "Lazarevic", "Dragan", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_KvalifikacijaId",
                table: "Zaposleni",
                column: "KvalifikacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_MestoId",
                table: "Zaposleni",
                column: "MestoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Zaposleni");

            migrationBuilder.DropTable(
                name: "Kvalifikacije");

            migrationBuilder.DropTable(
                name: "Mesta");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zaposleni_API_Auth.Migrations
{
    /// <inheritdoc />
    public partial class Init14_dodata4tabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sistematizacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivRadnogMesta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Koeficijent = table.Column<decimal>(type: "decimal(6,3)", nullable: true),
                    Radno_Iskustvo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Beneficirani_Radni_Staz = table.Column<int>(type: "int", nullable: true),
                    Bodovi = table.Column<int>(type: "int", nullable: true),
                    Opis = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    OrganizacioneJediniceId = table.Column<int>(type: "int", nullable: false),
                    KvalifikacijaId = table.Column<int>(type: "int", nullable: true),
                    KlasifikacijaZanimanjaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sistematizacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sistematizacije_Kvalifikacije_KvalifikacijaId",
                        column: x => x.KvalifikacijaId,
                        principalTable: "Kvalifikacije",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sistematizacije_OrganizacioneJedinice_OrganizacioneJediniceId",
                        column: x => x.OrganizacioneJediniceId,
                        principalTable: "OrganizacioneJedinice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Sistematizacije_KvalifikacijaId",
                table: "Sistematizacije",
                column: "KvalifikacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sistematizacije_OrganizacioneJediniceId",
                table: "Sistematizacije",
                column: "OrganizacioneJediniceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sistematizacije");

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
    }
}

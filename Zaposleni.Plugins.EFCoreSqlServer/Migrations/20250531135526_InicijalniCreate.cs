using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Zaposleni.Plugins.EFCoreSqlServer.Migrations
{
    /// <inheritdoc />
    public partial class InicijalniCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacioneJedinice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OJ = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    GrupaMestaTroskovaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacioneJedinice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacioneJedinice_GrupeMestaTroskova_GrupaMestaTroskovaId",
                        column: x => x.GrupaMestaTroskovaId,
                        principalTable: "GrupeMestaTroskova",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sistematizacije_OrganizacioneJedinice_OrganizacioneJediniceId",
                        column: x => x.OrganizacioneJediniceId,
                        principalTable: "OrganizacioneJedinice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zaposleni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Roditelj = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Telefon = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Pocetak_RadnogOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kraj_RadnogOd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vrsta_RadnogOdnosa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    A_P = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    KvalifikacijaId = table.Column<int>(type: "int", nullable: true),
                    MestoId = table.Column<int>(type: "int", nullable: true),
                    SistematizacijeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposleni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Kvalifikacije_KvalifikacijaId",
                        column: x => x.KvalifikacijaId,
                        principalTable: "Kvalifikacije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Mesta_MestoId",
                        column: x => x.MestoId,
                        principalTable: "Mesta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Zaposleni_Sistematizacije_SistematizacijeId",
                        column: x => x.SistematizacijeId,
                        principalTable: "Sistematizacije",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClanoviDomacinstva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeClana = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    PolClana = table.Column<string>(type: "NCHAR(1)", maxLength: 1, nullable: true),
                    SroClana = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DatumRodjenjaClana = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StatusClana = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    Roditelj = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ZaposlenId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanoviDomacinstva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClanoviDomacinstva_Zaposleni_ZaposlenId",
                        column: x => x.ZaposlenId,
                        principalTable: "Zaposleni",
                        principalColumn: "Id");
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

            migrationBuilder.InsertData(
                table: "Kvalifikacije",
                columns: new[] { "Id", "LicniStepenKv", "Naziv" },
                values: new object[,]
                {
                    { 1, "I", "Visoka stručna sprema" },
                    { 2, "II", "Srednja stručna sprema" }
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
                table: "OrganizacioneJedinice",
                columns: new[] { "Id", "GrupaMestaTroskovaId", "Naziv", "OJ" },
                values: new object[,]
                {
                    { 28, 1, "IT sektor", "0.5.00" },
                    { 29, 3, "ProizvodnjaBG", "1.0.03" }
                });

            migrationBuilder.InsertData(
                table: "Sistematizacije",
                columns: new[] { "Id", "Beneficirani_Radni_Staz", "Bodovi", "KlasifikacijaZanimanjaId", "Koeficijent", "KvalifikacijaId", "NazivRadnogMesta", "Opis", "OrganizacioneJediniceId", "Radno_Iskustvo" },
                values: new object[,]
                {
                    { 1, 0, 100, null, 2.5m, 1, "Programer", "Opis radnog mesta za programera.", 28, "5 godina" },
                    { 2, 0, 80, null, 1.85m, 1, "Tehničar", "Opis za tehničara.", 29, "3 godine" }
                });

            migrationBuilder.InsertData(
                table: "Zaposleni",
                columns: new[] { "Id", "A_P", "Adresa", "DatumRodjenja", "Ime", "JMBG", "Kraj_RadnogOd", "KvalifikacijaId", "MestoId", "Pocetak_RadnogOd", "Prezime", "Roditelj", "SistematizacijeId", "Telefon", "Vrsta_RadnogOdnosa" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(1987, 11, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marko", "2311487134221", null, null, 1, new DateTime(2025, 5, 31, 15, 55, 25, 950, DateTimeKind.Local).AddTicks(7723), "Negovanovic", "Uros", 1, null, null },
                    { 2, null, null, new DateTime(1960, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ivan", "0210960849567", null, null, 1, new DateTime(2025, 5, 31, 15, 55, 25, 950, DateTimeKind.Local).AddTicks(7788), "Lazarevic", "Dragan", 1, null, null }
                });

            migrationBuilder.InsertData(
                table: "ClanoviDomacinstva",
                columns: new[] { "Id", "DatumRodjenjaClana", "ImeClana", "JMBG", "PolClana", "Roditelj", "SroClana", "StatusClana", "ZaposlenId" },
                values: new object[,]
                {
                    { 1, new DateTime(1998, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mirjana", "1202998123456", "Ž", "Jelena", "Ćerka", "", 1 },
                    { 2, new DateTime(1997, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tihomir", "2309997654987", "M", "Jelena", "Sin", "", 1 },
                    { 3, new DateTime(2018, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Goran", "0906018557789", "M", "Ivana", "Sin", "", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClanoviDomacinstva_ZaposlenId",
                table: "ClanoviDomacinstva",
                column: "ZaposlenId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacioneJedinice_GrupaMestaTroskovaId",
                table: "OrganizacioneJedinice",
                column: "GrupaMestaTroskovaId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sistematizacije_KvalifikacijaId",
                table: "Sistematizacije",
                column: "KvalifikacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sistematizacije_OrganizacioneJediniceId",
                table: "Sistematizacije",
                column: "OrganizacioneJediniceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_KvalifikacijaId",
                table: "Zaposleni",
                column: "KvalifikacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_MestoId",
                table: "Zaposleni",
                column: "MestoId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposleni_SistematizacijeId",
                table: "Zaposleni",
                column: "SistematizacijeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ClanoviDomacinstva");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Zaposleni");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Mesta");

            migrationBuilder.DropTable(
                name: "Sistematizacije");

            migrationBuilder.DropTable(
                name: "Kvalifikacije");

            migrationBuilder.DropTable(
                name: "OrganizacioneJedinice");

            migrationBuilder.DropTable(
                name: "GrupeMestaTroskova");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certyfikat",
                columns: table => new
                {
                    CertyfikatID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Certyfikat_pk", x => x.CertyfikatID);
                });

            migrationBuilder.CreateTable(
                name: "Kort",
                columns: table => new
                {
                    KortID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Kort_pk", x => x.KortID);
                });

            migrationBuilder.CreateTable(
                name: "Osoba",
                columns: table => new
                {
                    OsobaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Haslo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataUr = table.Column<DateOnly>(type: "date", nullable: true),
                    NrTel = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Adres = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Osoba_pk", x => x.OsobaID);
                });

            migrationBuilder.CreateTable(
                name: "PoziomZajec",
                columns: table => new
                {
                    IdPoziomZajec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PoziomZajec_pk", x => x.IdPoziomZajec);
                });

            migrationBuilder.CreateTable(
                name: "Produkt",
                columns: table => new
                {
                    ProduktID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Producent = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LiczbaNaStanie = table.Column<int>(type: "int", nullable: false),
                    Koszt = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ZdjecieUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Produkt_pk", x => x.ProduktID);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Tag_pk", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "TypPracownika",
                columns: table => new
                {
                    IdTypPracownika = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TypPracownika_pk", x => x.IdTypPracownika);
                });

            migrationBuilder.CreateTable(
                name: "Klient",
                columns: table => new
                {
                    KlientID = table.Column<int>(type: "int", nullable: false),
                    Saldo = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ZnizkaNaZajecia = table.Column<int>(type: "int", nullable: true),
                    ZnizkaNaProdukty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Klient_pk", x => x.KlientID);
                    table.ForeignKey(
                        name: "Klient_Osoba",
                        column: x => x.KlientID,
                        principalTable: "Osoba",
                        principalColumn: "OsobaID");
                });

            migrationBuilder.CreateTable(
                name: "Zajecia",
                columns: table => new
                {
                    ZajeciaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdPoziomZajec = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Zajecia_pk", x => x.ZajeciaID);
                    table.ForeignKey(
                        name: "Zajecia_PoziomZajec",
                        column: x => x.IdPoziomZajec,
                        principalTable: "PoziomZajec",
                        principalColumn: "IdPoziomZajec");
                });

            migrationBuilder.CreateTable(
                name: "Pracownik",
                columns: table => new
                {
                    PracownikID = table.Column<int>(type: "int", nullable: false),
                    IdTypPracownika = table.Column<int>(type: "int", nullable: false),
                    DataZatrudnienia = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pracownik_pk", x => x.PracownikID);
                    table.ForeignKey(
                        name: "Pracownik_Osoba",
                        column: x => x.PracownikID,
                        principalTable: "Osoba",
                        principalColumn: "OsobaID");
                    table.ForeignKey(
                        name: "Pracownik_TypPracownika",
                        column: x => x.IdTypPracownika,
                        principalTable: "TypPracownika",
                        principalColumn: "IdTypPracownika");
                });

            migrationBuilder.CreateTable(
                name: "Klient_Tag",
                columns: table => new
                {
                    KlientID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Klient_Tag_pk", x => new { x.KlientID, x.TagID });
                    table.ForeignKey(
                        name: "Posiadanie_Klient",
                        column: x => x.KlientID,
                        principalTable: "Klient",
                        principalColumn: "KlientID");
                    table.ForeignKey(
                        name: "Posiadanie_Tag",
                        column: x => x.TagID,
                        principalTable: "Tag",
                        principalColumn: "TagID");
                });

            migrationBuilder.CreateTable(
                name: "GrafikZajec",
                columns: table => new
                {
                    GrafikZajecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CzasTrwania = table.Column<int>(type: "int", nullable: false),
                    ZajeciaID = table.Column<int>(type: "int", nullable: false),
                    PracownikID = table.Column<int>(type: "int", nullable: false),
                    LimitOsob = table.Column<int>(type: "int", nullable: false),
                    KortID = table.Column<int>(type: "int", nullable: false),
                    KoszBezSprzetu = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    KoszZeSprzetem = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GrafikZajec_pk", x => x.GrafikZajecID);
                    table.ForeignKey(
                        name: "GrafikZajec_Kort",
                        column: x => x.KortID,
                        principalTable: "Kort",
                        principalColumn: "KortID");
                    table.ForeignKey(
                        name: "GrafikZajec_Pracownik",
                        column: x => x.PracownikID,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikID");
                    table.ForeignKey(
                        name: "GrafikZajec_Zajecia",
                        column: x => x.ZajeciaID,
                        principalTable: "Zajecia",
                        principalColumn: "ZajeciaID");
                });

            migrationBuilder.CreateTable(
                name: "Rezerwacja",
                columns: table => new
                {
                    RezerwacjaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlientID = table.Column<int>(type: "int", nullable: false),
                    KortID = table.Column<int>(type: "int", nullable: false),
                    DataOd = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataDo = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataStworzenia = table.Column<DateOnly>(type: "date", nullable: false),
                    TrenerID = table.Column<int>(type: "int", nullable: true),
                    CzyUwzglednicSprzet = table.Column<bool>(type: "bit", nullable: false),
                    Koszt = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Rezerwacja_pk", x => x.RezerwacjaID);
                    table.ForeignKey(
                        name: "Rezerwacja_Klient",
                        column: x => x.KlientID,
                        principalTable: "Klient",
                        principalColumn: "KlientID");
                    table.ForeignKey(
                        name: "Rezerwacja_Kort",
                        column: x => x.KortID,
                        principalTable: "Kort",
                        principalColumn: "KortID");
                    table.ForeignKey(
                        name: "Rezerwacja_Pracownik",
                        column: x => x.TrenerID,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikID");
                });

            migrationBuilder.CreateTable(
                name: "Trener_Certifikat",
                columns: table => new
                {
                    PracownikID = table.Column<int>(type: "int", nullable: false),
                    CertyfikatID = table.Column<int>(type: "int", nullable: false),
                    DataOtrzymania = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Trener_Certifikat_pk", x => new { x.PracownikID, x.CertyfikatID });
                    table.ForeignKey(
                        name: "Posiadanie_Certyfikat",
                        column: x => x.CertyfikatID,
                        principalTable: "Certyfikat",
                        principalColumn: "CertyfikatID");
                    table.ForeignKey(
                        name: "Trener_Certifikat_Pracownik",
                        column: x => x.PracownikID,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikID");
                });

            migrationBuilder.CreateTable(
                name: "Zadanie",
                columns: table => new
                {
                    ZadanieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataDo = table.Column<DateOnly>(type: "date", nullable: true),
                    PracownikID = table.Column<int>(type: "int", nullable: false),
                    PracownikZlecajacyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Zadanie_pk", x => x.ZadanieID);
                    table.ForeignKey(
                        name: "Zadanie_Pracownik",
                        column: x => x.PracownikID,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikID");
                    table.ForeignKey(
                        name: "Zadanie_PracownikZlecajacy",
                        column: x => x.PracownikZlecajacyID,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikID");
                });

            migrationBuilder.CreateTable(
                name: "Zamowienie",
                columns: table => new
                {
                    ZamowienieID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlientID = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    DataOdbioru = table.Column<DateOnly>(type: "date", nullable: true),
                    DataRealizacji = table.Column<DateOnly>(type: "date", nullable: true),
                    PracownikID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Zamowienie_pk", x => x.ZamowienieID);
                    table.ForeignKey(
                        name: "Zamowienie_Klient",
                        column: x => x.KlientID,
                        principalTable: "Klient",
                        principalColumn: "KlientID");
                    table.ForeignKey(
                        name: "Zamowienie_Pracownik",
                        column: x => x.PracownikID,
                        principalTable: "Pracownik",
                        principalColumn: "PracownikID");
                });

            migrationBuilder.CreateTable(
                name: "DataZajec",
                columns: table => new
                {
                    DataZajecID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    GrafikZajecID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DataZajec_pk", x => x.DataZajecID);
                    table.ForeignKey(
                        name: "DataZajec_GrafikZajec",
                        column: x => x.GrafikZajecID,
                        principalTable: "GrafikZajec",
                        principalColumn: "GrafikZajecID");
                });

            migrationBuilder.CreateTable(
                name: "GrafikZajec_Klient",
                columns: table => new
                {
                    GrafikZajecKlientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrafikZajecID = table.Column<int>(type: "int", nullable: false),
                    KlientID = table.Column<int>(type: "int", nullable: false),
                    DataZapisu = table.Column<DateOnly>(type: "date", nullable: false),
                    DataWypisu = table.Column<DateOnly>(type: "date", nullable: true),
                    CzyUwzglednicSprzet = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GrafikZajec_Klient_pk", x => x.GrafikZajecKlientID);
                    table.ForeignKey(
                        name: "Table_34_GrafikZajec",
                        column: x => x.GrafikZajecID,
                        principalTable: "GrafikZajec",
                        principalColumn: "GrafikZajecID");
                    table.ForeignKey(
                        name: "Table_34_Klient",
                        column: x => x.KlientID,
                        principalTable: "Klient",
                        principalColumn: "KlientID");
                });

            migrationBuilder.CreateTable(
                name: "Zamowienie_Produkt",
                columns: table => new
                {
                    ZamowienieID = table.Column<int>(type: "int", nullable: false),
                    ProduktID = table.Column<int>(type: "int", nullable: false),
                    Liczba = table.Column<int>(type: "int", nullable: false),
                    Koszt = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Zamowienie_Produkt_pk", x => new { x.ZamowienieID, x.ProduktID });
                    table.ForeignKey(
                        name: "Zamowienie_Produkt_Produkt",
                        column: x => x.ProduktID,
                        principalTable: "Produkt",
                        principalColumn: "ProduktID");
                    table.ForeignKey(
                        name: "Zamowienie_Produkt_Zamowienie",
                        column: x => x.ZamowienieID,
                        principalTable: "Zamowienie",
                        principalColumn: "ZamowienieID");
                });

            migrationBuilder.CreateTable(
                name: "Ocena",
                columns: table => new
                {
                    OcenaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Opis = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gwiazdki = table.Column<int>(type: "int", nullable: false),
                    GrafikZajecKlientID = table.Column<int>(type: "int", nullable: false),
                    DataWystawienia = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Ocena_pk", x => x.OcenaID);
                    table.ForeignKey(
                        name: "Ocena_GrafikZajec_Klient",
                        column: x => x.GrafikZajecKlientID,
                        principalTable: "GrafikZajec_Klient",
                        principalColumn: "GrafikZajecKlientID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataZajec_GrafikZajecID",
                table: "DataZajec",
                column: "GrafikZajecID");

            migrationBuilder.CreateIndex(
                name: "IX_GrafikZajec_KortID",
                table: "GrafikZajec",
                column: "KortID");

            migrationBuilder.CreateIndex(
                name: "IX_GrafikZajec_PracownikID",
                table: "GrafikZajec",
                column: "PracownikID");

            migrationBuilder.CreateIndex(
                name: "IX_GrafikZajec_ZajeciaID",
                table: "GrafikZajec",
                column: "ZajeciaID");

            migrationBuilder.CreateIndex(
                name: "IX_GrafikZajec_Klient_GrafikZajecID",
                table: "GrafikZajec_Klient",
                column: "GrafikZajecID");

            migrationBuilder.CreateIndex(
                name: "IX_GrafikZajec_Klient_KlientID",
                table: "GrafikZajec_Klient",
                column: "KlientID");

            migrationBuilder.CreateIndex(
                name: "IX_Klient_Tag_TagID",
                table: "Klient_Tag",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_Ocena_GrafikZajecKlientID",
                table: "Ocena",
                column: "GrafikZajecKlientID");

            migrationBuilder.CreateIndex(
                name: "IX_Pracownik_IdTypPracownika",
                table: "Pracownik",
                column: "IdTypPracownika");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacja_KlientID",
                table: "Rezerwacja",
                column: "KlientID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacja_KortID",
                table: "Rezerwacja",
                column: "KortID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacja_TrenerID",
                table: "Rezerwacja",
                column: "TrenerID");

            migrationBuilder.CreateIndex(
                name: "IX_Trener_Certifikat_CertyfikatID",
                table: "Trener_Certifikat",
                column: "CertyfikatID");

            migrationBuilder.CreateIndex(
                name: "IX_Zadanie_PracownikID",
                table: "Zadanie",
                column: "PracownikID");

            migrationBuilder.CreateIndex(
                name: "IX_Zadanie_PracownikZlecajacyID",
                table: "Zadanie",
                column: "PracownikZlecajacyID");

            migrationBuilder.CreateIndex(
                name: "IX_Zajecia_IdPoziomZajec",
                table: "Zajecia",
                column: "IdPoziomZajec");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienie_KlientID",
                table: "Zamowienie",
                column: "KlientID");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienie_PracownikID",
                table: "Zamowienie",
                column: "PracownikID");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienie_Produkt_ProduktID",
                table: "Zamowienie_Produkt",
                column: "ProduktID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataZajec");

            migrationBuilder.DropTable(
                name: "Klient_Tag");

            migrationBuilder.DropTable(
                name: "Ocena");

            migrationBuilder.DropTable(
                name: "Rezerwacja");

            migrationBuilder.DropTable(
                name: "Trener_Certifikat");

            migrationBuilder.DropTable(
                name: "Zadanie");

            migrationBuilder.DropTable(
                name: "Zamowienie_Produkt");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "GrafikZajec_Klient");

            migrationBuilder.DropTable(
                name: "Certyfikat");

            migrationBuilder.DropTable(
                name: "Produkt");

            migrationBuilder.DropTable(
                name: "Zamowienie");

            migrationBuilder.DropTable(
                name: "GrafikZajec");

            migrationBuilder.DropTable(
                name: "Klient");

            migrationBuilder.DropTable(
                name: "Kort");

            migrationBuilder.DropTable(
                name: "Pracownik");

            migrationBuilder.DropTable(
                name: "Zajecia");

            migrationBuilder.DropTable(
                name: "Osoba");

            migrationBuilder.DropTable(
                name: "TypPracownika");

            migrationBuilder.DropTable(
                name: "PoziomZajec");
        }
    }
}

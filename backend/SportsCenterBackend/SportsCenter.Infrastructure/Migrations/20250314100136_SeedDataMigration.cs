using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace SportsCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seedowanie danych w tabelach
            migrationBuilder.InsertData(
                table: "TypPracownika",
                columns: new[] { "IdTypPracownika", "Nazwa" },
                values: new object[]
                {
                1, "Wlasciciel",
                2, "Pracownik administracyjny",
                3, "Trener",
                4, "Pomoc sprzatajaca"
                });

            migrationBuilder.InsertData(
                table: "GodzinyPracyKlubu",
                columns: new[] { "GodzinyPracyKlubuId", "GodzinaOtwarcia", "GodzinaZamkniecia", "DzienTygodnia" },
                values: new object[]
                {
                1, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "poniedzialek",
                2, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "wtorek",
                3, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "sroda",
                4, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "czwartek",
                5, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "piatek",
                6, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "sobota",
                7, TimeOnly.Parse("10:00"), TimeOnly.Parse("22:00"), "niedziela"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Opcjonalnie można usunąć dane, jeżeli migracja zostanie cofnięta
            migrationBuilder.DeleteData(table: "TypPracownika", keyColumn: "IdTypPracownika", keyValues: new object[] { 1, 2, 3, 4 });
            migrationBuilder.DeleteData(table: "GodzinyPracyKlubu", keyColumn: "GodzinyPracyKlubuId", keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7 });
        }
    }

}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsCenter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToZamowienie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "DataZajec_GrafikZajec",
                table: "DataZajec");

            migrationBuilder.DropForeignKey(
                name: "GrafikZajec_Kort",
                table: "GrafikZajec");

            migrationBuilder.DropForeignKey(
                name: "GrafikZajec_Pracownik",
                table: "GrafikZajec");

            migrationBuilder.DropForeignKey(
                name: "GrafikZajec_Zajecia",
                table: "GrafikZajec");

            migrationBuilder.DropForeignKey(
                name: "Posiadanie_Klient",
                table: "Klient_Tag");

            migrationBuilder.DropForeignKey(
                name: "Posiadanie_Tag",
                table: "Klient_Tag");

            migrationBuilder.DropForeignKey(
                name: "Zajecia_PoziomZajec",
                table: "Zajecia");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Zamowienie",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataWystawienia",
                table: "Ocena",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddForeignKey(
                name: "DataZajec_GrafikZajec",
                table: "DataZajec",
                column: "GrafikZajecID",
                principalTable: "GrafikZajec",
                principalColumn: "GrafikZajecID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "GrafikZajec_Kort",
                table: "GrafikZajec",
                column: "KortID",
                principalTable: "Kort",
                principalColumn: "KortID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "GrafikZajec_Pracownik",
                table: "GrafikZajec",
                column: "PracownikID",
                principalTable: "Pracownik",
                principalColumn: "PracownikID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "GrafikZajec_Zajecia",
                table: "GrafikZajec",
                column: "ZajeciaID",
                principalTable: "Zajecia",
                principalColumn: "ZajeciaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Posiadanie_Klient",
                table: "Klient_Tag",
                column: "KlientID",
                principalTable: "Klient",
                principalColumn: "KlientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Posiadanie_Tag",
                table: "Klient_Tag",
                column: "TagID",
                principalTable: "Tag",
                principalColumn: "TagID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Zajecia_PoziomZajec",
                table: "Zajecia",
                column: "IdPoziomZajec",
                principalTable: "PoziomZajec",
                principalColumn: "IdPoziomZajec",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "DataZajec_GrafikZajec",
                table: "DataZajec");

            migrationBuilder.DropForeignKey(
                name: "GrafikZajec_Kort",
                table: "GrafikZajec");

            migrationBuilder.DropForeignKey(
                name: "GrafikZajec_Pracownik",
                table: "GrafikZajec");

            migrationBuilder.DropForeignKey(
                name: "GrafikZajec_Zajecia",
                table: "GrafikZajec");

            migrationBuilder.DropForeignKey(
                name: "Posiadanie_Klient",
                table: "Klient_Tag");

            migrationBuilder.DropForeignKey(
                name: "Posiadanie_Tag",
                table: "Klient_Tag");

            migrationBuilder.DropForeignKey(
                name: "Zajecia_PoziomZajec",
                table: "Zajecia");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Zamowienie");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DataWystawienia",
                table: "Ocena",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "DataZajec_GrafikZajec",
                table: "DataZajec",
                column: "GrafikZajecID",
                principalTable: "GrafikZajec",
                principalColumn: "GrafikZajecID");

            migrationBuilder.AddForeignKey(
                name: "GrafikZajec_Kort",
                table: "GrafikZajec",
                column: "KortID",
                principalTable: "Kort",
                principalColumn: "KortID");

            migrationBuilder.AddForeignKey(
                name: "GrafikZajec_Pracownik",
                table: "GrafikZajec",
                column: "PracownikID",
                principalTable: "Pracownik",
                principalColumn: "PracownikID");

            migrationBuilder.AddForeignKey(
                name: "GrafikZajec_Zajecia",
                table: "GrafikZajec",
                column: "ZajeciaID",
                principalTable: "Zajecia",
                principalColumn: "ZajeciaID");

            migrationBuilder.AddForeignKey(
                name: "Posiadanie_Klient",
                table: "Klient_Tag",
                column: "KlientID",
                principalTable: "Klient",
                principalColumn: "KlientID");

            migrationBuilder.AddForeignKey(
                name: "Posiadanie_Tag",
                table: "Klient_Tag",
                column: "TagID",
                principalTable: "Tag",
                principalColumn: "TagID");

            migrationBuilder.AddForeignKey(
                name: "Zajecia_PoziomZajec",
                table: "Zajecia",
                column: "IdPoziomZajec",
                principalTable: "PoziomZajec",
                principalColumn: "IdPoziomZajec");
        }
    }
}

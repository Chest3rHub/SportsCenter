﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportsCenter.Infrastructure.DAL;

#nullable disable

namespace SportsCenter.Infrastructure.Migrations
{
    [DbContext(typeof(SportsCenterDbContext))]
    [Migration("20241017072609_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KlientTag", b =>
                {
                    b.Property<int>("KlientId")
                        .HasColumnType("int")
                        .HasColumnName("KlientID");

                    b.Property<int>("TagId")
                        .HasColumnType("int")
                        .HasColumnName("TagID");

                    b.HasKey("KlientId", "TagId")
                        .HasName("Klient_Tag_pk");

                    b.HasIndex("TagId");

                    b.ToTable("Klient_Tag", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Certyfikat", b =>
                {
                    b.Property<int>("CertyfikatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("CertyfikatID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CertyfikatId"));

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CertyfikatId")
                        .HasName("Certyfikat_pk");

                    b.ToTable("Certyfikat", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.DataZajec", b =>
                {
                    b.Property<int>("DataZajecId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("DataZajecID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DataZajecId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int>("GrafikZajecId")
                        .HasColumnType("int")
                        .HasColumnName("GrafikZajecID");

                    b.HasKey("DataZajecId")
                        .HasName("DataZajec_pk");

                    b.HasIndex("GrafikZajecId");

                    b.ToTable("DataZajec", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.GrafikZajec", b =>
                {
                    b.Property<int>("GrafikZajecId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("GrafikZajecID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GrafikZajecId"));

                    b.Property<int>("CzasTrwania")
                        .HasColumnType("int");

                    b.Property<int>("KortId")
                        .HasColumnType("int")
                        .HasColumnName("KortID");

                    b.Property<decimal>("KoszBezSprzetu")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<decimal>("KoszZeSprzetem")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int>("LimitOsob")
                        .HasColumnType("int");

                    b.Property<int>("PracownikId")
                        .HasColumnType("int")
                        .HasColumnName("PracownikID");

                    b.Property<int>("ZajeciaId")
                        .HasColumnType("int")
                        .HasColumnName("ZajeciaID");

                    b.HasKey("GrafikZajecId")
                        .HasName("GrafikZajec_pk");

                    b.HasIndex("KortId");

                    b.HasIndex("PracownikId");

                    b.HasIndex("ZajeciaId");

                    b.ToTable("GrafikZajec", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.GrafikZajecKlient", b =>
                {
                    b.Property<int>("GrafikZajecKlientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("GrafikZajecKlientID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GrafikZajecKlientId"));

                    b.Property<bool>("CzyUwzglednicSprzet")
                        .HasColumnType("bit");

                    b.Property<DateOnly?>("DataWypisu")
                        .HasColumnType("date");

                    b.Property<DateOnly>("DataZapisu")
                        .HasColumnType("date");

                    b.Property<int>("GrafikZajecId")
                        .HasColumnType("int")
                        .HasColumnName("GrafikZajecID");

                    b.Property<int>("KlientId")
                        .HasColumnType("int")
                        .HasColumnName("KlientID");

                    b.HasKey("GrafikZajecKlientId")
                        .HasName("GrafikZajec_Klient_pk");

                    b.HasIndex("GrafikZajecId");

                    b.HasIndex("KlientId");

                    b.ToTable("GrafikZajec_Klient", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Klient", b =>
                {
                    b.Property<int>("KlientId")
                        .HasColumnType("int")
                        .HasColumnName("KlientID");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int?>("ZnizkaNaProdukty")
                        .HasColumnType("int");

                    b.Property<int?>("ZnizkaNaZajecia")
                        .HasColumnType("int");

                    b.HasKey("KlientId")
                        .HasName("Klient_pk");

                    b.ToTable("Klient", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Kort", b =>
                {
                    b.Property<int>("KortId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("KortID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("KortId"));

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("KortId")
                        .HasName("Kort_pk");

                    b.ToTable("Kort", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Ocena", b =>
                {
                    b.Property<int>("OcenaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("OcenaID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OcenaId"));

                    b.Property<DateOnly>("DataWystawienia")
                        .HasColumnType("date");

                    b.Property<int>("GrafikZajecKlientId")
                        .HasColumnType("int")
                        .HasColumnName("GrafikZajecKlientID");

                    b.Property<int>("Gwiazdki")
                        .HasColumnType("int");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("OcenaId")
                        .HasName("Ocena_pk");

                    b.HasIndex("GrafikZajecKlientId");

                    b.ToTable("Ocena", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Osoba", b =>
                {
                    b.Property<int>("OsobaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("OsobaID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OsobaId"));

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateOnly?>("DataUr")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Haslo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NrTel")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("Pesel")
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.HasKey("OsobaId")
                        .HasName("Osoba_pk");

                    b.ToTable("Osoba", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.PoziomZajec", b =>
                {
                    b.Property<int>("IdPoziomZajec")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPoziomZajec"));

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("IdPoziomZajec")
                        .HasName("PoziomZajec_pk");

                    b.ToTable("PoziomZajec", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Pracownik", b =>
                {
                    b.Property<int>("PracownikId")
                        .HasColumnType("int")
                        .HasColumnName("PracownikID");

                    b.Property<DateOnly>("DataZatrudnienia")
                        .HasColumnType("date");

                    b.Property<int>("IdTypPracownika")
                        .HasColumnType("int");

                    b.HasKey("PracownikId")
                        .HasName("Pracownik_pk");

                    b.HasIndex("IdTypPracownika");

                    b.ToTable("Pracownik", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Produkt", b =>
                {
                    b.Property<int>("ProduktId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ProduktID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProduktId"));

                    b.Property<decimal>("Koszt")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int>("LiczbaNaStanie")
                        .HasColumnType("int");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Producent")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ZdjecieUrl")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProduktId")
                        .HasName("Produkt_pk");

                    b.ToTable("Produkt", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Rezerwacja", b =>
                {
                    b.Property<int>("RezerwacjaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("RezerwacjaID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RezerwacjaId"));

                    b.Property<bool>("CzyUwzglednicSprzet")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DataDo")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DataOd")
                        .HasColumnType("datetime");

                    b.Property<DateOnly>("DataStworzenia")
                        .HasColumnType("date");

                    b.Property<int>("KlientId")
                        .HasColumnType("int")
                        .HasColumnName("KlientID");

                    b.Property<int>("KortId")
                        .HasColumnType("int")
                        .HasColumnName("KortID");

                    b.Property<decimal>("Koszt")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int?>("TrenerId")
                        .HasColumnType("int")
                        .HasColumnName("TrenerID");

                    b.HasKey("RezerwacjaId")
                        .HasName("Rezerwacja_pk");

                    b.HasIndex("KlientId");

                    b.HasIndex("KortId");

                    b.HasIndex("TrenerId");

                    b.ToTable("Rezerwacja", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("TagID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("TagId")
                        .HasName("Tag_pk");

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.TrenerCertifikat", b =>
                {
                    b.Property<int>("PracownikId")
                        .HasColumnType("int")
                        .HasColumnName("PracownikID");

                    b.Property<int>("CertyfikatId")
                        .HasColumnType("int")
                        .HasColumnName("CertyfikatID");

                    b.Property<DateOnly>("DataOtrzymania")
                        .HasColumnType("date");

                    b.HasKey("PracownikId", "CertyfikatId")
                        .HasName("Trener_Certifikat_pk");

                    b.HasIndex("CertyfikatId");

                    b.ToTable("Trener_Certifikat", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.TypPracownika", b =>
                {
                    b.Property<int>("IdTypPracownika")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTypPracownika"));

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("IdTypPracownika")
                        .HasName("TypPracownika_pk");

                    b.ToTable("TypPracownika", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zadanie", b =>
                {
                    b.Property<int>("ZadanieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ZadanieID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ZadanieId"));

                    b.Property<DateOnly?>("DataDo")
                        .HasColumnType("date");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("PracownikId")
                        .HasColumnType("int")
                        .HasColumnName("PracownikID");

                    b.Property<int>("PracownikZlecajacyId")
                        .HasColumnType("int")
                        .HasColumnName("PracownikZlecajacyID");

                    b.HasKey("ZadanieId")
                        .HasName("Zadanie_pk");

                    b.HasIndex("PracownikId");

                    b.HasIndex("PracownikZlecajacyId");

                    b.ToTable("Zadanie", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zajecium", b =>
                {
                    b.Property<int>("ZajeciaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ZajeciaID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ZajeciaId"));

                    b.Property<int>("IdPoziomZajec")
                        .HasColumnType("int");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ZajeciaId")
                        .HasName("Zajecia_pk");

                    b.HasIndex("IdPoziomZajec");

                    b.ToTable("Zajecia");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zamowienie", b =>
                {
                    b.Property<int>("ZamowienieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ZamowienieID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ZamowienieId"));

                    b.Property<DateOnly>("Data")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DataOdbioru")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DataRealizacji")
                        .HasColumnType("date");

                    b.Property<int>("KlientId")
                        .HasColumnType("int")
                        .HasColumnName("KlientID");

                    b.Property<int>("PracownikId")
                        .HasColumnType("int")
                        .HasColumnName("PracownikID");

                    b.HasKey("ZamowienieId")
                        .HasName("Zamowienie_pk");

                    b.HasIndex("KlientId");

                    b.HasIndex("PracownikId");

                    b.ToTable("Zamowienie", (string)null);
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.ZamowienieProdukt", b =>
                {
                    b.Property<int>("ZamowienieId")
                        .HasColumnType("int")
                        .HasColumnName("ZamowienieID");

                    b.Property<int>("ProduktId")
                        .HasColumnType("int")
                        .HasColumnName("ProduktID");

                    b.Property<decimal>("Koszt")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<int>("Liczba")
                        .HasColumnType("int");

                    b.HasKey("ZamowienieId", "ProduktId")
                        .HasName("Zamowienie_Produkt_pk");

                    b.HasIndex("ProduktId");

                    b.ToTable("Zamowienie_Produkt", (string)null);
                });

            modelBuilder.Entity("KlientTag", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Klient", null)
                        .WithMany()
                        .HasForeignKey("KlientId")
                        .IsRequired()
                        .HasConstraintName("Posiadanie_Klient");

                    b.HasOne("SportsCenter.Core.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagId")
                        .IsRequired()
                        .HasConstraintName("Posiadanie_Tag");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.DataZajec", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.GrafikZajec", "GrafikZajec")
                        .WithMany("DataZajecs")
                        .HasForeignKey("GrafikZajecId")
                        .IsRequired()
                        .HasConstraintName("DataZajec_GrafikZajec");

                    b.Navigation("GrafikZajec");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.GrafikZajec", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Kort", "Kort")
                        .WithMany("GrafikZajecs")
                        .HasForeignKey("KortId")
                        .IsRequired()
                        .HasConstraintName("GrafikZajec_Kort");

                    b.HasOne("SportsCenter.Core.Entities.Pracownik", "Pracownik")
                        .WithMany("GrafikZajecs")
                        .HasForeignKey("PracownikId")
                        .IsRequired()
                        .HasConstraintName("GrafikZajec_Pracownik");

                    b.HasOne("SportsCenter.Core.Entities.Zajecium", "Zajecia")
                        .WithMany("GrafikZajecs")
                        .HasForeignKey("ZajeciaId")
                        .IsRequired()
                        .HasConstraintName("GrafikZajec_Zajecia");

                    b.Navigation("Kort");

                    b.Navigation("Pracownik");

                    b.Navigation("Zajecia");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.GrafikZajecKlient", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.GrafikZajec", "GrafikZajec")
                        .WithMany("GrafikZajecKlients")
                        .HasForeignKey("GrafikZajecId")
                        .IsRequired()
                        .HasConstraintName("Table_34_GrafikZajec");

                    b.HasOne("SportsCenter.Core.Entities.Klient", "Klient")
                        .WithMany("GrafikZajecKlients")
                        .HasForeignKey("KlientId")
                        .IsRequired()
                        .HasConstraintName("Table_34_Klient");

                    b.Navigation("GrafikZajec");

                    b.Navigation("Klient");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Klient", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Osoba", "KlientNavigation")
                        .WithOne("Klient")
                        .HasForeignKey("SportsCenter.Core.Entities.Klient", "KlientId")
                        .IsRequired()
                        .HasConstraintName("Klient_Osoba");

                    b.Navigation("KlientNavigation");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Ocena", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.GrafikZajecKlient", "GrafikZajecKlient")
                        .WithMany("Ocenas")
                        .HasForeignKey("GrafikZajecKlientId")
                        .IsRequired()
                        .HasConstraintName("Ocena_GrafikZajec_Klient");

                    b.Navigation("GrafikZajecKlient");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Pracownik", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.TypPracownika", "IdTypPracownikaNavigation")
                        .WithMany("Pracowniks")
                        .HasForeignKey("IdTypPracownika")
                        .IsRequired()
                        .HasConstraintName("Pracownik_TypPracownika");

                    b.HasOne("SportsCenter.Core.Entities.Osoba", "PracownikNavigation")
                        .WithOne("Pracownik")
                        .HasForeignKey("SportsCenter.Core.Entities.Pracownik", "PracownikId")
                        .IsRequired()
                        .HasConstraintName("Pracownik_Osoba");

                    b.Navigation("IdTypPracownikaNavigation");

                    b.Navigation("PracownikNavigation");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Rezerwacja", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Klient", "Klient")
                        .WithMany("Rezerwacjas")
                        .HasForeignKey("KlientId")
                        .IsRequired()
                        .HasConstraintName("Rezerwacja_Klient");

                    b.HasOne("SportsCenter.Core.Entities.Kort", "Kort")
                        .WithMany("Rezerwacjas")
                        .HasForeignKey("KortId")
                        .IsRequired()
                        .HasConstraintName("Rezerwacja_Kort");

                    b.HasOne("SportsCenter.Core.Entities.Pracownik", "Trener")
                        .WithMany("Rezerwacjas")
                        .HasForeignKey("TrenerId")
                        .HasConstraintName("Rezerwacja_Pracownik");

                    b.Navigation("Klient");

                    b.Navigation("Kort");

                    b.Navigation("Trener");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.TrenerCertifikat", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Certyfikat", "Certyfikat")
                        .WithMany("TrenerCertifikats")
                        .HasForeignKey("CertyfikatId")
                        .IsRequired()
                        .HasConstraintName("Posiadanie_Certyfikat");

                    b.HasOne("SportsCenter.Core.Entities.Pracownik", "Pracownik")
                        .WithMany("TrenerCertifikats")
                        .HasForeignKey("PracownikId")
                        .IsRequired()
                        .HasConstraintName("Trener_Certifikat_Pracownik");

                    b.Navigation("Certyfikat");

                    b.Navigation("Pracownik");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zadanie", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Pracownik", "Pracownik")
                        .WithMany("ZadaniePracowniks")
                        .HasForeignKey("PracownikId")
                        .IsRequired()
                        .HasConstraintName("Zadanie_Pracownik");

                    b.HasOne("SportsCenter.Core.Entities.Pracownik", "PracownikZlecajacy")
                        .WithMany("ZadaniePracownikZlecajacies")
                        .HasForeignKey("PracownikZlecajacyId")
                        .IsRequired()
                        .HasConstraintName("Zadanie_PracownikZlecajacy");

                    b.Navigation("Pracownik");

                    b.Navigation("PracownikZlecajacy");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zajecium", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.PoziomZajec", "IdPoziomZajecNavigation")
                        .WithMany("Zajecia")
                        .HasForeignKey("IdPoziomZajec")
                        .IsRequired()
                        .HasConstraintName("Zajecia_PoziomZajec");

                    b.Navigation("IdPoziomZajecNavigation");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zamowienie", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Klient", "Klient")
                        .WithMany("Zamowienies")
                        .HasForeignKey("KlientId")
                        .IsRequired()
                        .HasConstraintName("Zamowienie_Klient");

                    b.HasOne("SportsCenter.Core.Entities.Pracownik", "Pracownik")
                        .WithMany("Zamowienies")
                        .HasForeignKey("PracownikId")
                        .IsRequired()
                        .HasConstraintName("Zamowienie_Pracownik");

                    b.Navigation("Klient");

                    b.Navigation("Pracownik");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.ZamowienieProdukt", b =>
                {
                    b.HasOne("SportsCenter.Core.Entities.Produkt", "Produkt")
                        .WithMany("ZamowienieProdukts")
                        .HasForeignKey("ProduktId")
                        .IsRequired()
                        .HasConstraintName("Zamowienie_Produkt_Produkt");

                    b.HasOne("SportsCenter.Core.Entities.Zamowienie", "Zamowienie")
                        .WithMany("ZamowienieProdukts")
                        .HasForeignKey("ZamowienieId")
                        .IsRequired()
                        .HasConstraintName("Zamowienie_Produkt_Zamowienie");

                    b.Navigation("Produkt");

                    b.Navigation("Zamowienie");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Certyfikat", b =>
                {
                    b.Navigation("TrenerCertifikats");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.GrafikZajec", b =>
                {
                    b.Navigation("DataZajecs");

                    b.Navigation("GrafikZajecKlients");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.GrafikZajecKlient", b =>
                {
                    b.Navigation("Ocenas");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Klient", b =>
                {
                    b.Navigation("GrafikZajecKlients");

                    b.Navigation("Rezerwacjas");

                    b.Navigation("Zamowienies");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Kort", b =>
                {
                    b.Navigation("GrafikZajecs");

                    b.Navigation("Rezerwacjas");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Osoba", b =>
                {
                    b.Navigation("Klient");

                    b.Navigation("Pracownik");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.PoziomZajec", b =>
                {
                    b.Navigation("Zajecia");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Pracownik", b =>
                {
                    b.Navigation("GrafikZajecs");

                    b.Navigation("Rezerwacjas");

                    b.Navigation("TrenerCertifikats");

                    b.Navigation("ZadaniePracownikZlecajacies");

                    b.Navigation("ZadaniePracowniks");

                    b.Navigation("Zamowienies");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Produkt", b =>
                {
                    b.Navigation("ZamowienieProdukts");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.TypPracownika", b =>
                {
                    b.Navigation("Pracowniks");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zajecium", b =>
                {
                    b.Navigation("GrafikZajecs");
                });

            modelBuilder.Entity("SportsCenter.Core.Entities.Zamowienie", b =>
                {
                    b.Navigation("ZamowienieProdukts");
                });
#pragma warning restore 612, 618
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SportsCenterBackend.Entities;

namespace SportsCenterBackend.Context;

public partial class SportsCenterDbContext : DbContext
{
    
    public SportsCenterDbContext(DbContextOptions<SportsCenterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Certyfikat> Certyfikats { get; set; }

    public virtual DbSet<Grafik> Grafiks { get; set; }

    public virtual DbSet<Klient> Klients { get; set; }

    public virtual DbSet<KlientTag> KlientTags { get; set; }

    public virtual DbSet<Kort> Korts { get; set; }

    public virtual DbSet<Ocena> Ocenas { get; set; }

    public virtual DbSet<Osoba> Osobas { get; set; }

    public virtual DbSet<PomocSprzatajaca> PomocSprzatajacas { get; set; }

    public virtual DbSet<Posiadanie> Posiadanies { get; set; }

    public virtual DbSet<PracownikAdministracyjny> PracownikAdministracyjnies { get; set; }

    public virtual DbSet<Produkt> Produkts { get; set; }

    public virtual DbSet<Rezerwacja> Rezerwacjas { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Todo> Todos { get; set; }

    public virtual DbSet<Trener> Treners { get; set; }

    public virtual DbSet<WlascicielKlubu> WlascicielKlubus { get; set; }

    public virtual DbSet<ZajeciaWGrafiku> ZajeciaWGrafikus { get; set; }

    public virtual DbSet<Zajecium> Zajecia { get; set; }

    public virtual DbSet<Zamowienie> Zamowienies { get; set; }

    public virtual DbSet<ZamowienieProdukt> ZamowienieProdukts { get; set; }

    public virtual DbSet<Zapis> Zapis { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Certyfikat>(entity =>
        {
            entity.HasKey(e => e.CertyfikatId).HasName("Certyfikat_pk");

            entity.ToTable("Certyfikat");

            entity.Property(e => e.CertyfikatId).HasColumnName("CertyfikatID");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Grafik>(entity =>
        {
            entity.HasKey(e => e.GrafikId).HasName("Grafik_pk");

            entity.ToTable("Grafik");

            entity.Property(e => e.GrafikId).HasColumnName("GrafikID");
            entity.Property(e => e.CzasTrwania).HasColumnName("Czas_trwania");
            entity.Property(e => e.Data).HasColumnType("date");
            entity.Property(e => e.GodzinaOd).HasColumnName("Godzina_od");
            entity.Property(e => e.RezerwacjaId).HasColumnName("RezerwacjaID");
            entity.Property(e => e.ZajeciaWGrafikuId).HasColumnName("Zajecia_w_grafikuID");

            entity.HasOne(d => d.Rezerwacja).WithMany(p => p.Grafiks)
                .HasForeignKey(d => d.RezerwacjaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grafik_Rezerwacja");

            entity.HasOne(d => d.ZajeciaWGrafiku).WithMany(p => p.Grafiks)
                .HasForeignKey(d => d.ZajeciaWGrafikuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grafik_Zajecia_w_grafiku");
        });

        modelBuilder.Entity<Klient>(entity =>
        {
            entity.HasKey(e => e.KlientId).HasName("Klient_pk");

            entity.ToTable("Klient");

            entity.Property(e => e.KlientId)
                .HasColumnName("KlientID");
            entity.Property(e => e.ZnizkaProdukty).HasColumnName("Znizka_produkty");
            entity.Property(e => e.ZnizkaZajecia).HasColumnName("Znizka_zajecia");

            entity.HasOne(d => d.KlientNavigation).WithOne(p => p.Klient)
                .HasForeignKey<Klient>(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Klient_Osoba");
        });

        modelBuilder.Entity<KlientTag>(entity =>
        {
            entity.HasKey(e => e.KlientTagId).HasName("Klient_Tag_pk");

            entity.ToTable("Klient_Tag");

            entity.Property(e => e.KlientTagId).HasColumnName("KlientTagID");
            entity.Property(e => e.KlientKlientId).HasColumnName("Klient_KlientID");
            entity.Property(e => e.TagTagId).HasColumnName("Tag_TagID");

            entity.HasOne(d => d.KlientKlient).WithMany(p => p.KlientTags)
                .HasForeignKey(d => d.KlientKlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Posiadanie_Klient");

            entity.HasOne(d => d.TagTag).WithMany(p => p.KlientTags)
                .HasForeignKey(d => d.TagTagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Posiadanie_Tag");
        });

        modelBuilder.Entity<Kort>(entity =>
        {
            entity.HasKey(e => e.KortId).HasName("Kort_pk");

            entity.ToTable("Kort");

            entity.Property(e => e.KortId).HasColumnName("KortID");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Ocena>(entity =>
        {
            entity.HasKey(e => e.OcenaId).HasName("Ocena_pk");

            entity.ToTable("Ocena");

            entity.Property(e => e.OcenaId).HasColumnName("OcenaID");
            entity.Property(e => e.KlientKlientId).HasColumnName("Klient_KlientID");
            entity.Property(e => e.Opis)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TrenerTrenerId).HasColumnName("Trener_TrenerID");
            entity.Property(e => e.ZajeciaWGrafikuZajeciaWGrafikuId).HasColumnName("Zajecia_w_grafiku_Zajecia_w_grafikuID");

            entity.HasOne(d => d.KlientKlient).WithMany(p => p.Ocenas)
                .HasForeignKey(d => d.KlientKlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ocena_Klient");

            entity.HasOne(d => d.TrenerTrener).WithMany(p => p.Ocenas)
                .HasForeignKey(d => d.TrenerTrenerId)
                .HasConstraintName("Ocena_Trener");

            entity.HasOne(d => d.ZajeciaWGrafikuZajeciaWGrafiku).WithMany(p => p.Ocenas)
                .HasForeignKey(d => d.ZajeciaWGrafikuZajeciaWGrafikuId)
                .HasConstraintName("Ocena_Zajecia_w_grafiku");
        });

        modelBuilder.Entity<Osoba>(entity =>
        {
            entity.HasKey(e => e.OsobaId).HasName("Osoba_pk");

            entity.ToTable("Osoba");

            entity.Property(e => e.OsobaId).HasColumnName("OsobaID");
            entity.Property(e => e.Adres)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DataUr)
                .HasColumnType("date")
                .HasColumnName("Data_ur");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Haslo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Imie)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nazwisko)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NrTel)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Nr_tel");
            entity.Property(e => e.Pesel)
                .HasMaxLength(11)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PomocSprzatajaca>(entity =>
        {
            entity.HasKey(e => e.PomocSprzatajacaId).HasName("Pomoc_sprzatajaca_pk");

            entity.ToTable("Pomoc_sprzatajaca");

            entity.Property(e => e.PomocSprzatajacaId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Pomoc_sprzatajacaID");

            entity.HasOne(d => d.PomocSprzatajacaNavigation).WithOne(p => p.PomocSprzatajaca)
                .HasForeignKey<PomocSprzatajaca>(d => d.PomocSprzatajacaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Pomoc_sprzatajaca_Osoba");
        });

        modelBuilder.Entity<Posiadanie>(entity =>
        {
            entity.HasKey(e => e.PosiadanieId).HasName("Posiadanie_pk");

            entity.ToTable("Posiadanie");

            entity.Property(e => e.PosiadanieId).HasColumnName("PosiadanieID");
            entity.Property(e => e.CertyfikatId).HasColumnName("CertyfikatID");
            entity.Property(e => e.DataOtrzymania)
                .HasColumnType("date")
                .HasColumnName("Data_otrzymania");
            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");

            entity.HasOne(d => d.Certyfikat).WithMany(p => p.Posiadanies)
                .HasForeignKey(d => d.CertyfikatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Posiadanie_Certyfikat");

            entity.HasOne(d => d.Trener).WithMany(p => p.Posiadanies)
                .HasForeignKey(d => d.TrenerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Posiadanie_Trener");
        });

        modelBuilder.Entity<PracownikAdministracyjny>(entity =>
        {
            entity.HasKey(e => e.PracownikAdmId).HasName("Pracownik_administracyjny_pk");

            entity.ToTable("Pracownik_administracyjny");

            entity.Property(e => e.PracownikAdmId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Pracownik_admID");

            entity.HasOne(d => d.PracownikAdm).WithOne(p => p.PracownikAdministracyjny)
                .HasForeignKey<PracownikAdministracyjny>(d => d.PracownikAdmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Pracownik_administracyjny_Osoba");
        });

        modelBuilder.Entity<Produkt>(entity =>
        {
            entity.HasKey(e => e.ProduktId).HasName("Produkt_pk");

            entity.ToTable("Produkt");

            entity.Property(e => e.ProduktId).HasColumnName("ProduktID");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(20)
                .IsUnicode(false);
           // entity.Property(e => e.Producent)
               // .HasMaxLength(20)
               // .IsUnicode(false);
            //entity.Property(e => e.Zdjecie).HasColumnType("image");
        });

        modelBuilder.Entity<Rezerwacja>(entity =>
        {
            entity.HasKey(e => e.RezerwacjaId).HasName("Rezerwacja_pk");

            entity.ToTable("Rezerwacja");

            entity.Property(e => e.RezerwacjaId).HasColumnName("RezerwacjaID");
            entity.Property(e => e.KlientId).HasColumnName("KlientID");
            entity.Property(e => e.KortId).HasColumnName("KortID");
            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");
            entity.Property(e => e.ZajeciaZajeciaId).HasColumnName("Zajecia_ZajeciaID");

            entity.HasOne(d => d.Klient).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rezerwacja_Klient");

            entity.HasOne(d => d.Kort).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.KortId)
                .HasConstraintName("Rezerwacja_Kort");

            entity.HasOne(d => d.Trener).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.TrenerId)
                .HasConstraintName("Rezerwacja_Trener");

            entity.HasOne(d => d.ZajeciaZajecia).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.ZajeciaZajeciaId)
                .HasConstraintName("Rezerwacja_Zajecia");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tag_pk");

            entity.ToTable("Tag");

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.TodoId).HasName("TODO_pk");

            entity.ToTable("Todo");

            entity.Property(e => e.TodoId).HasColumnName("TodoID");
            entity.Property(e => e.PomocSprzatajacaId).HasColumnName("Pomoc_sprzatajacaID");
            entity.Property(e => e.PracownikAdmId).HasColumnName("Pracownik_admID");
            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");
            entity.Property(e => e.WlascicielKlubuId).HasColumnName("Wlasciciel_klubuID");

            entity.HasOne(d => d.PomocSprzatajaca).WithMany(p => p.Todos)
                .HasForeignKey(d => d.PomocSprzatajacaId)
                .HasConstraintName("TODO_Pomoc_sprzatajaca");

            entity.HasOne(d => d.PracownikAdm).WithMany(p => p.Todos)
                .HasForeignKey(d => d.PracownikAdmId)
                .HasConstraintName("TODO_Pracownik_administracyjny");

            entity.HasOne(d => d.Trener).WithMany(p => p.Todos)
                .HasForeignKey(d => d.TrenerId)
                .HasConstraintName("TODO_Trener");

            entity.HasOne(d => d.WlascicielKlubu).WithMany(p => p.Todos)
                .HasForeignKey(d => d.WlascicielKlubuId)
                .HasConstraintName("TODO_Wlasciciel_klubu");
        });

        modelBuilder.Entity<Trener>(entity =>
        {
            entity.HasKey(e => e.TrenerId).HasName("Trener_pk");

            entity.ToTable("Trener");

            entity.Property(e => e.TrenerId)
                .ValueGeneratedOnAdd()
                .HasColumnName("TrenerID");

            entity.HasOne(d => d.TrenerNavigation).WithOne(p => p.Trener)
                .HasForeignKey<Trener>(d => d.TrenerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Trener_Osoba");
        });

        modelBuilder.Entity<WlascicielKlubu>(entity =>
        {
            entity.HasKey(e => e.WlascicielKlubuId).HasName("Wlasciciel_klubu_pk");

            entity.ToTable("Wlasciciel_klubu");

            entity.Property(e => e.WlascicielKlubuId)
                .ValueGeneratedOnAdd()
                .HasColumnName("Wlasciciel_klubuID");

            entity.HasOne(d => d.WlascicielKlubuNavigation).WithOne(p => p.WlascicielKlubu)
                .HasForeignKey<WlascicielKlubu>(d => d.WlascicielKlubuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Wlasciciel_klubu_Osoba");
        });

        modelBuilder.Entity<ZajeciaWGrafiku>(entity =>
        {
            entity.HasKey(e => e.ZajeciaWGrafikuId).HasName("Zajecia_w_grafiku_pk");

            entity.ToTable("Zajecia_w_grafiku");

            entity.Property(e => e.ZajeciaWGrafikuId).HasColumnName("Zajecia_w_grafikuID");
            entity.Property(e => e.KortId).HasColumnName("KortID");
            entity.Property(e => e.NazwaGrupy)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nazwa_grupy");
            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");
            entity.Property(e => e.ZajeciaId).HasColumnName("ZajeciaID");

            entity.HasOne(d => d.Kort).WithMany(p => p.ZajeciaWGrafikus)
                .HasForeignKey(d => d.KortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grafik_Kort");

            entity.HasOne(d => d.Trener).WithMany(p => p.ZajeciaWGrafikus)
                .HasForeignKey(d => d.TrenerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grafik_Trener");

            entity.HasOne(d => d.Zajecia).WithMany(p => p.ZajeciaWGrafikus)
                .HasForeignKey(d => d.ZajeciaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Grafik_Zajecia");
        });

        modelBuilder.Entity<Zajecium>(entity =>
        {
            entity.HasKey(e => e.ZajeciaId).HasName("Zajecia_pk");

            entity.Property(e => e.ZajeciaId).HasColumnName("ZajeciaID");
            entity.Property(e => e.CzyRezerwacjaPrywatna).HasColumnName("Czy_rezerwacja_prywatna");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Poziom)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("poziom");
        });

        modelBuilder.Entity<Zamowienie>(entity =>
        {
            entity.HasKey(e => e.ZamowienieId).HasName("Zamowienie_pk");

            entity.ToTable("Zamowienie");

            entity.Property(e => e.ZamowienieId).HasColumnName("ZamowienieID");
            entity.Property(e => e.KlientId).HasColumnName("KlientID");

            entity.HasOne(d => d.Klient).WithMany(p => p.Zamowienies)
                .HasForeignKey(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zamowienie_Klient");
        });

        modelBuilder.Entity<ZamowienieProdukt>(entity =>
        {
            entity.HasKey(e => e.ZamowienieProduktId).HasName("Zamowienie_Produkt_pk");

            entity.ToTable("Zamowienie_Produkt");

            entity.Property(e => e.ZamowienieProduktId).HasColumnName("ZamowienieProduktID");
            entity.Property(e => e.DataZamowienia)
                .HasColumnType("date")
                .HasColumnName("Data_zamowienia");
            entity.Property(e => e.ProduktId).HasColumnName("ProduktID");
            entity.Property(e => e.ZamowienieId).HasColumnName("ZamowienieID");

            entity.HasOne(d => d.Produkt).WithMany(p => p.ZamowienieProdukts)
                .HasForeignKey(d => d.ProduktId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zamowienie_Produkt_Produkt");

            entity.HasOne(d => d.Zamowienie).WithMany(p => p.ZamowienieProdukts)
                .HasForeignKey(d => d.ZamowienieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zamowienie_Produkt_Zamowienie");
        });

        modelBuilder.Entity<Zapis>(entity =>
        {
            entity.HasKey(e => e.ZapisId).HasName("Zapis_pk");

            entity.Property(e => e.ZapisId).HasColumnName("ZapisID");
            entity.Property(e => e.KlientId).HasColumnName("KlientID");
            entity.Property(e => e.ZajeciaWGrafikuId).HasColumnName("Zajecia_w_grafikuID");

            entity.HasOne(d => d.Klient).WithMany(p => p.Zapis)
                .HasForeignKey(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zapis_Klient");

            entity.HasOne(d => d.ZajeciaWGrafiku).WithMany(p => p.Zapis)
                .HasForeignKey(d => d.ZajeciaWGrafikuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zapis_Zajecia_w_grafiku");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

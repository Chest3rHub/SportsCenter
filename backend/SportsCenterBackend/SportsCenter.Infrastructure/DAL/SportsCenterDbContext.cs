using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SportsCenter.Core.Entities;

namespace SportsCenter.Infrastructure.DAL;

public partial class SportsCenterDbContext : DbContext
{
    public SportsCenterDbContext()
    {
    }

    public SportsCenterDbContext(DbContextOptions<SportsCenterDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aktualnosci> Aktualnoscis { get; set; }

    public virtual DbSet<BrakDostepnosci> BrakDostepnoscis { get; set; }

    public virtual DbSet<Certyfikat> Certyfikats { get; set; }

    public virtual DbSet<DataZajec> DataZajecs { get; set; }

    public virtual DbSet<GodzinyPracyKlubu> GodzinyPracyKlubus { get; set; }

    public virtual DbSet<GrafikZajec> GrafikZajecs { get; set; }

    public virtual DbSet<GrafikZajecKlient> GrafikZajecKlients { get; set; }

    public virtual DbSet<Klient> Klients { get; set; }

    public virtual DbSet<Kort> Korts { get; set; }

    public virtual DbSet<Ocena> Ocenas { get; set; }

    public virtual DbSet<Osoba> Osobas { get; set; }

    public virtual DbSet<PoziomZajec> PoziomZajecs { get; set; }

    public virtual DbSet<Pracownik> Pracowniks { get; set; }

    public virtual DbSet<Produkt> Produkts { get; set; }

    public virtual DbSet<Rezerwacja> Rezerwacjas { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TrenerCertyfikat> TrenerCertyfikats { get; set; }

    public virtual DbSet<TypPracownika> TypPracownikas { get; set; }

    public virtual DbSet<WyjatkoweGodzinyPracy> WyjatkoweGodzinyPracies { get; set; }

    public virtual DbSet<Zadanie> Zadanies { get; set; }

    public virtual DbSet<Zajecium> Zajecia { get; set; }

    public virtual DbSet<Zamowienie> Zamowienies { get; set; }

    public virtual DbSet<ZamowienieProdukt> ZamowienieProdukts { get; set; }

    public virtual DbSet<Zastepstwo> Zastepstwos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=SportsCenter;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aktualnosci>(entity =>
        {
            entity.HasKey(e => e.AktualnosciId).HasName("Aktualnosci_pk");

            entity.ToTable("Aktualnosci");

            entity.Property(e => e.AktualnosciId).HasColumnName("AktualnosciID");
            entity.Property(e => e.Nazwa)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Opis).HasMaxLength(4000);
            entity.Property(e => e.WazneDo).HasColumnType("datetime");
            entity.Property(e => e.WazneOd).HasColumnType("datetime");
        });

        modelBuilder.Entity<BrakDostepnosci>(entity =>
        {
            entity.HasKey(e => e.BrakDostepnosciId).HasName("BrakDostepnosci_pk");

            entity.ToTable("BrakDostepnosci");

            entity.Property(e => e.BrakDostepnosciId)
                .HasColumnName("BrakDostepnosciID");
            entity.Property(e => e.GodzinaDo).HasPrecision(0);
            entity.Property(e => e.GodzinaOd).HasPrecision(0);
            entity.Property(e => e.CzyZatwierdzone)
            .HasColumnName("CzyZatwierdzone")
            .HasConversion<bool>();
            entity.Property(e => e.PracownikId).HasColumnName("PracownikID");

            entity.HasOne(d => d.Pracownik).WithMany(p => p.BrakDostepnoscis)
                .HasForeignKey(d => d.PracownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BrakDostepnosci_Pracownik");
        });

        modelBuilder.Entity<Certyfikat>(entity =>
        {
            entity.HasKey(e => e.CertyfikatId).HasName("Certyfikat_pk");

            entity.ToTable("Certyfikat");

            entity.Property(e => e.CertyfikatId).HasColumnName("CertyfikatID");
            entity.Property(e => e.Nazwa).HasMaxLength(255);
        });

        modelBuilder.Entity<DataZajec>(entity =>
        {
            entity.HasKey(e => e.DataZajecId).HasName("DataZajec_pk");

            entity.ToTable("DataZajec");

            entity.Property(e => e.DataZajecId).HasColumnName("DataZajecID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.GrafikZajecId).HasColumnName("GrafikZajecID");

            entity.HasOne(d => d.GrafikZajec).WithMany(p => p.DataZajecs)
                .HasForeignKey(d => d.GrafikZajecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DataZajec_GrafikZajec");
        });

        modelBuilder.Entity<GodzinyPracyKlubu>(entity =>
        {
            entity.HasKey(e => e.GodzinyPracyKlubuId).HasName("GodzinyPracyKlubuId");

            entity.ToTable("GodzinyPracyKlubu");

            entity.Property(e => e.GodzinyPracyKlubuId).HasColumnName("GodzinyPracyKlubuID");
            entity.Property(e => e.DzienTygodnia).HasMaxLength(20);
            entity.Property(e => e.GodzinaOtwarcia).HasPrecision(0);
            entity.Property(e => e.GodzinaZamkniecia).HasPrecision(0);
        });

        modelBuilder.Entity<GrafikZajec>(entity =>
        {
            entity.HasKey(e => e.GrafikZajecId).HasName("GrafikZajec_pk");

            entity.ToTable("GrafikZajec");

            entity.Property(e => e.GrafikZajecId).HasColumnName("GrafikZajecID");
            entity.Property(e => e.KortId).HasColumnName("KortID");
            entity.Property(e => e.KoszBezSprzetu).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.KoszZeSprzetem).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PracownikId).HasColumnName("PracownikID");
            entity.Property(e => e.ZajeciaId).HasColumnName("ZajeciaID");

            entity.HasOne(d => d.Kort).WithMany(p => p.GrafikZajecs)
                .HasForeignKey(d => d.KortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GrafikZajec_Kort");

            entity.HasOne(d => d.Pracownik).WithMany(p => p.GrafikZajecs)
                .HasForeignKey(d => d.PracownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GrafikZajec_Pracownik");

            entity.HasOne(d => d.Zajecia).WithMany(p => p.GrafikZajecs)
                .HasForeignKey(d => d.ZajeciaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GrafikZajec_Zajecia");
        });

        modelBuilder.Entity<GrafikZajecKlient>(entity =>
        {
            entity.HasKey(e => e.GrafikZajecKlientId).HasName("GrafikZajec_Klient_pk");

            entity.ToTable("GrafikZajec_Klient");

            entity.Property(e => e.GrafikZajecKlientId)
                .ValueGeneratedNever()
                .HasColumnName("GrafikZajecKlientID");
            entity.Property(e => e.GrafikZajecId).HasColumnName("GrafikZajecID");
            entity.Property(e => e.KlientId).HasColumnName("KlientID");

            entity.HasOne(d => d.GrafikZajec).WithMany(p => p.GrafikZajecKlients)
                .HasForeignKey(d => d.GrafikZajecId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_34_GrafikZajec");

            entity.HasOne(d => d.Klient).WithMany(p => p.GrafikZajecKlients)
                .HasForeignKey(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Table_34_Klient");
        });

        modelBuilder.Entity<Klient>(entity =>
        {
            entity.HasKey(e => e.KlientId).HasName("Klient_pk");

            entity.ToTable("Klient");

            entity.Property(e => e.KlientId)
                .ValueGeneratedNever()
                .HasColumnName("KlientID");
            entity.Property(e => e.Saldo).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.KlientNavigation).WithOne(p => p.Klient)
                .HasForeignKey<Klient>(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Klient_Osoba");

            entity.HasMany(d => d.Tags).WithMany(p => p.Klients)
                .UsingEntity<Dictionary<string, object>>(
                    "KlientTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("Posiadanie_Tag"),
                    l => l.HasOne<Klient>().WithMany()
                        .HasForeignKey("KlientId")
                        .HasConstraintName("Posiadanie_Klient"),
                    j =>
                    {
                        j.HasKey("KlientId", "TagId").HasName("Klient_Tag_pk");
                        j.ToTable("Klient_Tag");
                        j.IndexerProperty<int>("KlientId").HasColumnName("KlientID");
                        j.IndexerProperty<int>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<Kort>(entity =>
        {
            entity.HasKey(e => e.KortId).HasName("Kort_pk");

            entity.ToTable("Kort");

            entity.Property(e => e.KortId).HasColumnName("KortID");
            entity.Property(e => e.Nazwa).HasMaxLength(50);
        });

        modelBuilder.Entity<Ocena>(entity =>
        {
            entity.HasKey(e => e.OcenaId).HasName("Ocena_pk");

            entity.ToTable("Ocena");

            entity.Property(e => e.OcenaId).HasColumnName("OcenaID");
            entity.Property(e => e.GrafikZajecKlientId).HasColumnName("GrafikZajecKlientID");
            entity.Property(e => e.Opis).HasMaxLength(255);

            entity.HasOne(d => d.GrafikZajecKlient).WithMany(p => p.Ocenas)
                .HasForeignKey(d => d.GrafikZajecKlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Ocena_GrafikZajec_Klient");
        });

        modelBuilder.Entity<Osoba>(entity =>
        {
            entity.HasKey(e => e.OsobaId).HasName("Osoba_pk");

            entity.ToTable("Osoba");

            entity.Property(e => e.OsobaId).HasColumnName("OsobaID");
            entity.Property(e => e.Adres).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Haslo).HasMaxLength(255);
            entity.Property(e => e.Imie).HasMaxLength(50);
            entity.Property(e => e.Nazwisko).HasMaxLength(50);
            entity.Property(e => e.NrTel).HasMaxLength(15);
            entity.Property(e => e.Pesel).HasMaxLength(11);
        });

        modelBuilder.Entity<PoziomZajec>(entity =>
        {
            entity.HasKey(e => e.IdPoziomZajec).HasName("PoziomZajec_pk");

            entity.ToTable("PoziomZajec");

            entity.Property(e => e.Nazwa).HasMaxLength(100);
        });

        modelBuilder.Entity<Pracownik>(entity =>
        {
            entity.HasKey(e => e.PracownikId).HasName("Pracownik_pk");

            entity.ToTable("Pracownik");

            entity.Property(e => e.PracownikId)
                .ValueGeneratedNever()
                .HasColumnName("PracownikID");

            entity.HasOne(d => d.IdTypPracownikaNavigation).WithMany(p => p.Pracowniks)
                .HasForeignKey(d => d.IdTypPracownika)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Pracownik_TypPracownika");

            entity.HasOne(d => d.PracownikNavigation).WithOne(p => p.Pracownik)
                .HasForeignKey<Pracownik>(d => d.PracownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Pracownik_Osoba");
        });

        modelBuilder.Entity<Produkt>(entity =>
        {
            entity.HasKey(e => e.ProduktId).HasName("Produkt_pk");

            entity.ToTable("Produkt");

            entity.Property(e => e.ProduktId).HasColumnName("ProduktID");
            entity.Property(e => e.Koszt).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Nazwa).HasMaxLength(100);
            entity.Property(e => e.Producent).HasMaxLength(100);
            entity.Property(e => e.ZdjecieUrl).HasMaxLength(100);
        });

        modelBuilder.Entity<Rezerwacja>(entity =>
        {
            entity.HasKey(e => e.RezerwacjaId).HasName("Rezerwacja_pk");

            entity.ToTable("Rezerwacja");

            entity.Property(e => e.RezerwacjaId).HasColumnName("RezerwacjaID");
            entity.Property(e => e.DataDo).HasColumnType("datetime");
            entity.Property(e => e.DataOd).HasColumnType("datetime");
            entity.Property(e => e.KlientId).HasColumnName("KlientID");
            entity.Property(e => e.KortId).HasColumnName("KortID");
            entity.Property(e => e.Koszt).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TrenerId).HasColumnName("TrenerID");

            entity.HasOne(d => d.Klient).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rezerwacja_Klient");

            entity.HasOne(d => d.Kort).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.KortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rezerwacja_Kort");

            entity.HasOne(d => d.Trener).WithMany(p => p.Rezerwacjas)
                .HasForeignKey(d => d.TrenerId)
                .HasConstraintName("Rezerwacja_Pracownik");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("Tag_pk");

            entity.ToTable("Tag");

            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.Nazwa).HasMaxLength(70);
        });

        modelBuilder.Entity<TrenerCertyfikat>(entity =>
        {
            entity.HasKey(e => new { e.PracownikId, e.CertyfikatId }).HasName("Trener_Certyfikat_pk");

            entity.ToTable("Trener_Certyfikat");

            entity.Property(e => e.PracownikId).HasColumnName("PracownikID");
            entity.Property(e => e.CertyfikatId).HasColumnName("CertyfikatID");

            entity.HasOne(d => d.Certyfikat).WithMany(p => p.TrenerCertyfikats)
                .HasForeignKey(d => d.CertyfikatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Posiadanie_Certyfikat");

            entity.HasOne(d => d.Pracownik).WithMany(p => p.TrenerCertyfikats)
                .HasForeignKey(d => d.PracownikId)
                .HasConstraintName("Trener_Certyfikat_Pracownik");
        });

        modelBuilder.Entity<TypPracownika>(entity =>
        {
            entity.HasKey(e => e.IdTypPracownika).HasName("TypPracownika_pk");

            entity.ToTable("TypPracownika");

            entity.Property(e => e.Nazwa).HasMaxLength(200);
        });

        modelBuilder.Entity<WyjatkoweGodzinyPracy>(entity =>
        {
            entity.HasKey(e => e.WyjatkoweGodzinyPracyId).HasName("WyjatkoweGodzinyPracy_pk");

            entity.ToTable("WyjatkoweGodzinyPracy");

            entity.Property(e => e.WyjatkoweGodzinyPracyId)
                .ValueGeneratedOnAdd()
                .HasColumnName("WyjatkoweGodzinyPracyID");
            entity.Property(e => e.GodzinaOtwarcia).HasPrecision(0);
            entity.Property(e => e.GodzinaZamkniecia).HasPrecision(0);
        });

        modelBuilder.Entity<Zadanie>(entity =>
        {
            entity.HasKey(e => e.ZadanieId).HasName("Zadanie_pk");

            entity.ToTable("Zadanie");

            entity.Property(e => e.ZadanieId).HasColumnName("ZadanieID");
            entity.Property(e => e.Opis).HasMaxLength(500);
            entity.Property(e => e.PracownikId).HasColumnName("PracownikID");
            entity.Property(e => e.PracownikZlecajacyId).HasColumnName("PracownikZlecajacyID");

            entity.HasOne(d => d.Pracownik).WithMany(p => p.ZadaniePracowniks)
                .HasForeignKey(d => d.PracownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zadanie_Pracownik");

            entity.HasOne(d => d.PracownikZlecajacy).WithMany(p => p.ZadaniePracownikZlecajacies)
                .HasForeignKey(d => d.PracownikZlecajacyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zadanie_PracownikZlecajacy");
        });

        modelBuilder.Entity<Zajecium>(entity =>
        {
            entity.HasKey(e => e.ZajeciaId).HasName("Zajecia_pk");

            entity.Property(e => e.ZajeciaId).HasColumnName("ZajeciaID");
            entity.Property(e => e.Nazwa).HasMaxLength(100);

            entity.HasOne(d => d.IdPoziomZajecNavigation).WithMany(p => p.Zajecia)
                .HasForeignKey(d => d.IdPoziomZajec)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zajecia_PoziomZajec");
        });

        modelBuilder.Entity<Zamowienie>(entity =>
        {
            entity.HasKey(e => e.ZamowienieId).HasName("Zamowienie_pk");

            entity.ToTable("Zamowienie");

            entity.Property(e => e.ZamowienieId).HasColumnName("ZamowienieID");
            entity.Property(e => e.KlientId).HasColumnName("KlientID");
            entity.Property(e => e.PracownikId).HasColumnName("PracownikID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Klient).WithMany(p => p.Zamowienies)
                .HasForeignKey(d => d.KlientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zamowienie_Klient");

            entity.HasOne(d => d.Pracownik).WithMany(p => p.Zamowienies)
                .HasForeignKey(d => d.PracownikId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zamowienie_Pracownik");
        });

        modelBuilder.Entity<ZamowienieProdukt>(entity =>
        {
            entity.HasKey(e => new { e.ZamowienieId, e.ProduktId }).HasName("Zamowienie_Produkt_pk");

            entity.ToTable("Zamowienie_Produkt");

            entity.Property(e => e.ZamowienieId).HasColumnName("ZamowienieID");
            entity.Property(e => e.ProduktId).HasColumnName("ProduktID");
            entity.Property(e => e.Koszt).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Produkt).WithMany(p => p.ZamowienieProdukts)
                .HasForeignKey(d => d.ProduktId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zamowienie_Produkt_Produkt");

            entity.HasOne(d => d.Zamowienie).WithMany(p => p.ZamowienieProdukts)
                .HasForeignKey(d => d.ZamowienieId)
                .HasConstraintName("Zamowienie_Produkt_Zamowienie");
        });

        modelBuilder.Entity<Zastepstwo>(entity =>
        {
            entity.HasKey(e => e.ZastepstwoId).HasName("Zastepstwo_pk");

            entity.ToTable("Zastepstwo");

            entity.Property(e => e.ZastepstwoId)
                .ValueGeneratedNever()
                .HasColumnName("ZastepstwoID");
            entity.Property(e => e.GodzinaDo).HasPrecision(0);
            entity.Property(e => e.GodzinaOd).HasPrecision(0);
            entity.Property(e => e.PracownikNieobecnyId).HasColumnName("PracownikNieobecnyID");
            entity.Property(e => e.PracownikZastepujacyId).HasColumnName("PracownikZastepujacyID");
            entity.Property(e => e.PracownikZatwierdzajacyId).HasColumnName("PracownikZatwierdzajacyID");

            entity.HasOne(d => d.PracownikNieobecny).WithMany(p => p.ZastepstwoPracownikNieobecnies)
                .HasForeignKey(d => d.PracownikNieobecnyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zastepstwo_PracownikNieobecny");

            entity.HasOne(d => d.PracownikZastepujacy).WithMany(p => p.ZastepstwoPracownikZastepujacies)
                .HasForeignKey(d => d.PracownikZastepujacyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zastepstwo_PracownikZastepujacy");

            entity.HasOne(d => d.PracownikZatwierdzajacy).WithMany(p => p.ZastepstwoPracownikZatwierdzajacies)
                .HasForeignKey(d => d.PracownikZatwierdzajacyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Zastepstwo_PracownikZatwierdzajacy");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

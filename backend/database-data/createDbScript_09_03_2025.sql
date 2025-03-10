
CREATE TABLE Aktualnosci (
    AktualnosciID int  NOT NULL IDENTITY(1, 1),
    Nazwa varchar(20)  NOT NULL,
    Opis nvarchar(4000)  NOT NULL,
    WazneOd datetime  NOT NULL,
    WazneDo datetime  NULL,
    CONSTRAINT Aktualnosci_pk PRIMARY KEY  (AktualnosciID)
);

CREATE TABLE BrakDostepnosci (
    BrakDostepnosciID int  NOT NULL IDENTITY(1, 1),
    Data date  NOT NULL,
    GodzinaOd time(0)  NOT NULL,
    GodzinaDo time(0)  NOT NULL,
    CzyZatwierdzone bit  NOT NULL,
    PracownikID int  NOT NULL,
    CONSTRAINT BrakDostepnosci_pk PRIMARY KEY  (BrakDostepnosciID)
);

CREATE TABLE Certyfikat (
    CertyfikatID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(255)  NOT NULL,
    CONSTRAINT Certyfikat_pk PRIMARY KEY  (CertyfikatID)
);

CREATE TABLE DataZajec (
    DataZajecID int  NOT NULL IDENTITY(1, 1),
    Date datetime  NOT NULL,
    GrafikZajecID int  NOT NULL,
    CONSTRAINT DataZajec_pk PRIMARY KEY  (DataZajecID)
);

CREATE TABLE GodzinyPracyKlubu (
    GodzinyPracyKlubuID int  NOT NULL IDENTITY(1, 1),
    GodzinaOtwarcia time(0)  NOT NULL,
    GodzinaZamkniecia time(0)  NOT NULL,
    DzienTygodnia nvarchar(20)  NOT NULL,
    CONSTRAINT GodzinyPracyKlubuId PRIMARY KEY  (GodzinyPracyKlubuID)
);

CREATE TABLE GrafikZajec (
    GrafikZajecID int  NOT NULL IDENTITY(1, 1),
    CzasTrwania int  NOT NULL,
    ZajeciaID int  NOT NULL,
    PracownikID int  NOT NULL,
    LimitOsob int  NOT NULL,
    KortID int  NOT NULL,
    KoszBezSprzetu decimal(5, 2)  NOT NULL,
    KoszZeSprzetem decimal(5, 2)  NOT NULL,
    CONSTRAINT GrafikZajec_pk PRIMARY KEY  (GrafikZajecID)
);

CREATE TABLE GrafikZajec_Klient (
    GrafikZajecKlientID int  NOT NULL,
    GrafikZajecID int  NOT NULL,
    KlientID int  NOT NULL,
    DataZapisu date  NOT NULL,
    DataWypisu date  NULL,
    CzyUwzglednicSprzet bit  NOT NULL,
    CONSTRAINT GrafikZajec_Klient_pk PRIMARY KEY  (GrafikZajecKlientID)
);

CREATE TABLE Klient (
    KlientID int  NOT NULL,
    Saldo decimal(5,2)  NOT NULL,
    ZnizkaNaZajecia int  NULL,
    ZnizkaNaProdukty int  NULL,
    CONSTRAINT Klient_pk PRIMARY KEY  (KlientID)
);

CREATE TABLE Klient_Tag (
    KlientID int  NOT NULL,
    TagID int  NOT NULL,
    CONSTRAINT Klient_Tag_pk PRIMARY KEY  (KlientID,TagID)
);

CREATE TABLE Kort (
    KortID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(50)  NOT NULL,
    CONSTRAINT Kort_pk PRIMARY KEY  (KortID)
);

CREATE TABLE Ocena (
    OcenaID int  NOT NULL IDENTITY(1, 1),
    Opis nvarchar(255)  NOT NULL,
    Gwiazdki int  NOT NULL,
    GrafikZajecKlientID int  NOT NULL,
    DataWystawienia date  NOT NULL,
    CONSTRAINT Ocena_pk PRIMARY KEY  (OcenaID)
);

CREATE TABLE Osoba (
    OsobaID int  NOT NULL IDENTITY(1, 1),
    Imie nvarchar(50)  NOT NULL,
    Nazwisko nvarchar(50)  NOT NULL,
    Email nvarchar(50)  NOT NULL,
    Haslo nvarchar(255)  NOT NULL,
    DataUr date  NULL,
    NrTel nvarchar(15)  NOT NULL,
    Pesel nvarchar(11)  NULL,
    Adres nvarchar(255)  NOT NULL,
    CONSTRAINT Osoba_pk PRIMARY KEY  (OsobaID)
);

CREATE TABLE PoziomZajec (
    IdPoziomZajec int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(100)  NOT NULL,
    CONSTRAINT PoziomZajec_pk PRIMARY KEY  (IdPoziomZajec)
);

CREATE TABLE Pracownik (
    PracownikID int  NOT NULL,
    IdTypPracownika int  NOT NULL,
    DataZatrudnienia date  NOT NULL,
    DataZwolnienia date  NULL,
    CONSTRAINT Pracownik_pk PRIMARY KEY  (PracownikID)
);

CREATE TABLE Produkt (
    ProduktID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(100)  NOT NULL,
    Producent nvarchar(100)  NOT NULL,
    LiczbaNaStanie int  NOT NULL,
    Koszt decimal(5, 2)  NOT NULL,
    ZdjecieUrl nvarchar(100)  NOT NULL,
    CONSTRAINT Produkt_pk PRIMARY KEY  (ProduktID)
);

CREATE TABLE Rezerwacja (
    RezerwacjaID int  NOT NULL IDENTITY(1, 1),
    KlientID int  NOT NULL,
    KortID int  NOT NULL,
    DataOd datetime  NOT NULL,
    DataDo datetime  NOT NULL,
    DataStworzenia date  NOT NULL,
    TrenerID int  NULL,
    CzyUwzglednicSprzet bit  NOT NULL,
    Koszt decimal(5, 2)  NOT NULL,
    CONSTRAINT Rezerwacja_pk PRIMARY KEY  (RezerwacjaID)
);

CREATE TABLE Tag (
    TagID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(70)  NOT NULL,
    CONSTRAINT Tag_pk PRIMARY KEY  (TagID)
);

CREATE TABLE Trener_Certyfikat (
    PracownikID int  NOT NULL,
    CertyfikatID int  NOT NULL,
    DataOtrzymania date  NOT NULL,
    CONSTRAINT Trener_Certyfikat_pk PRIMARY KEY  (PracownikID,CertyfikatID)
);

CREATE TABLE TypPracownika (
    IdTypPracownika int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(200)  NOT NULL,
    CONSTRAINT TypPracownika_pk PRIMARY KEY  (IdTypPracownika)
);

CREATE TABLE WyjatkoweGodzinyPracy (
    WyjatkoweGodzinyPracyID int  NOT NULL IDENTITY(1, 1),
    Data date  NOT NULL,
    GodzinaOtwarcia time(0)  NOT NULL,
    GodzinaZamkniecia time(0)  NOT NULL,
    CONSTRAINT WyjatkoweGodzinyPracy_pk PRIMARY KEY  (WyjatkoweGodzinyPracyID)
);

CREATE TABLE Zadanie (
    ZadanieID int  NOT NULL IDENTITY(1, 1),
    Opis nvarchar(500)  NOT NULL,
    DataDo date  NULL,
    PracownikID int  NOT NULL,
    PracownikZlecajacyID int  NOT NULL,
    CONSTRAINT Zadanie_pk PRIMARY KEY  (ZadanieID)
);

CREATE TABLE Zajecia (
    ZajeciaID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(100)  NOT NULL,
    IdPoziomZajec int  NOT NULL,
    CONSTRAINT Zajecia_pk PRIMARY KEY  (ZajeciaID)
);

CREATE TABLE Zamowienie (
    ZamowienieID int  NOT NULL IDENTITY(1, 1),
    KlientID int  NOT NULL,
    Data date  NOT NULL,
    DataOdbioru date  NULL,
    DataRealizacji date  NULL,
    PracownikID int  NOT NULL,
    Status nvarchar(50)  NOT NULL,
    CONSTRAINT Zamowienie_pk PRIMARY KEY  (ZamowienieID)
);

CREATE TABLE Zamowienie_Produkt (
    ZamowienieID int  NOT NULL,
    ProduktID int  NOT NULL,
    Liczba int  NOT NULL,
    Koszt decimal(5, 2)  NOT NULL,
    CONSTRAINT Zamowienie_Produkt_pk PRIMARY KEY  (ZamowienieID,ProduktID)
);

CREATE TABLE Zastepstwo (
    ZastepstwoID int  NOT NULL IDENTITY(1, 1),
    Data date  NOT NULL,
    GodzinaOd time(0)  NOT NULL,
    GodzinaDo time(0)  NOT NULL,
    ZajeciaID int  NULL,
    RezerwacjaID int  NULL,
    PracownikNieobecnyID int  NOT NULL,
    PracownikZastepujacyID int  NULL,
    PracownikZatwierdzajacyID int  NULL,
    CONSTRAINT Zastepstwo_pk PRIMARY KEY  (ZastepstwoID)
);

ALTER TABLE BrakDostepnosci ADD CONSTRAINT BrakDostepnosci_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE DataZajec ADD CONSTRAINT DataZajec_GrafikZajec
    FOREIGN KEY (GrafikZajecID)
    REFERENCES GrafikZajec (GrafikZajecID);

ALTER TABLE GrafikZajec ADD CONSTRAINT GrafikZajec_Kort
    FOREIGN KEY (KortID)
    REFERENCES Kort (KortID);

ALTER TABLE GrafikZajec ADD CONSTRAINT GrafikZajec_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE GrafikZajec ADD CONSTRAINT GrafikZajec_Zajecia
    FOREIGN KEY (ZajeciaID)
    REFERENCES Zajecia (ZajeciaID);

ALTER TABLE Klient ADD CONSTRAINT Klient_Osoba
    FOREIGN KEY (KlientID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_GrafikZajec_Klient
    FOREIGN KEY (GrafikZajecKlientID)
    REFERENCES GrafikZajec_Klient (GrafikZajecKlientID);

ALTER TABLE Trener_Certyfikat ADD CONSTRAINT Posiadanie_Certyfikat
    FOREIGN KEY (CertyfikatID)
    REFERENCES Certyfikat (CertyfikatID);

ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID)
    ON DELETE  CASCADE 
    ON UPDATE  CASCADE;

ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Tag
    FOREIGN KEY (TagID)
    REFERENCES Tag (TagID);

ALTER TABLE Pracownik ADD CONSTRAINT Pracownik_Osoba
    FOREIGN KEY (PracownikID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Pracownik ADD CONSTRAINT Pracownik_TypPracownika
    FOREIGN KEY (IdTypPracownika)
    REFERENCES TypPracownika (IdTypPracownika);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Kort
    FOREIGN KEY (KortID)
    REFERENCES Kort (KortID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Pracownik
    FOREIGN KEY (TrenerID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE GrafikZajec_Klient ADD CONSTRAINT Table_34_GrafikZajec
    FOREIGN KEY (GrafikZajecID)
    REFERENCES GrafikZajec (GrafikZajecID);

ALTER TABLE GrafikZajec_Klient ADD CONSTRAINT Table_34_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Trener_Certyfikat ADD CONSTRAINT Trener_Certyfikat_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID)
    ON DELETE  CASCADE 
    ON UPDATE  CASCADE;

ALTER TABLE Zadanie ADD CONSTRAINT Zadanie_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE Zadanie ADD CONSTRAINT Zadanie_PracownikZlecajacy
    FOREIGN KEY (PracownikZlecajacyID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE Zajecia ADD CONSTRAINT Zajecia_PoziomZajec
    FOREIGN KEY (IdPoziomZajec)
    REFERENCES PoziomZajec (IdPoziomZajec);

ALTER TABLE Zamowienie ADD CONSTRAINT Zamowienie_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Zamowienie ADD CONSTRAINT Zamowienie_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Produkt
    FOREIGN KEY (ProduktID)
    REFERENCES Produkt (ProduktID);

ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Zamowienie
    FOREIGN KEY (ZamowienieID)
    REFERENCES Zamowienie (ZamowienieID)
    ON DELETE  CASCADE 
    ON UPDATE  CASCADE;

ALTER TABLE Zastepstwo ADD CONSTRAINT Zastepstwo_PracownikNieobecny
    FOREIGN KEY (PracownikNieobecnyID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE Zastepstwo ADD CONSTRAINT Zastepstwo_PracownikZastepujacy
    FOREIGN KEY (PracownikZastepujacyID)
    REFERENCES Pracownik (PracownikID);

ALTER TABLE Zastepstwo ADD CONSTRAINT Zastepstwo_PracownikZatwierdzajacy
    FOREIGN KEY (PracownikZatwierdzajacyID)
    REFERENCES Pracownik (PracownikID);


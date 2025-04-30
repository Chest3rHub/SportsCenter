-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2025-03-21 11:01:58.069

-- tables
-- Table: Aktualnosci
CREATE TABLE Aktualnosci (
    AktualnosciID int  NOT NULL IDENTITY(1, 1),
    Nazwa varchar(20)  NOT NULL,
    Opis nvarchar(4000)  NOT NULL,
    WazneOd datetime  NOT NULL,
    WazneDo datetime  NULL,
    CONSTRAINT Aktualnosci_pk PRIMARY KEY  (AktualnosciID)
);

-- Table: BrakDostepnosci
CREATE TABLE BrakDostepnosci (
    BrakDostepnosciID int  NOT NULL IDENTITY(1, 1),
    Data date  NOT NULL,
    GodzinaOd time(0)  NOT NULL,
    GodzinaDo time(0)  NOT NULL,
    CzyZatwierdzone bit  NOT NULL,
    PracownikID int  NOT NULL,
    CONSTRAINT BrakDostepnosci_pk PRIMARY KEY  (BrakDostepnosciID)
);

-- Table: Certyfikat
CREATE TABLE Certyfikat (
    CertyfikatID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(255)  NOT NULL,
    CONSTRAINT Certyfikat_pk PRIMARY KEY  (CertyfikatID)
);

-- Table: GodzinyPracyKlubu
CREATE TABLE GodzinyPracyKlubu (
    GodzinyPracyKlubuID int  NOT NULL IDENTITY(1, 1),
    GodzinaOtwarcia time(0)  NOT NULL,
    GodzinaZamkniecia time(0)  NOT NULL,
    DzienTygodnia nvarchar(20)  NOT NULL,
    CONSTRAINT GodzinyPracyKlubuId PRIMARY KEY  (GodzinyPracyKlubuID)
);

-- Table: GrafikZajec
CREATE TABLE GrafikZajec (
    GrafikZajecID int  NOT NULL IDENTITY(1, 1),
    DataStartuZajec date  NOT NULL,
    DzienTygodnia nvarchar(20)  NOT NULL,
    GodzinaOd time(0)  NOT NULL,
    CzasTrwania int  NOT NULL,
    ZajeciaID int  NOT NULL,
    PracownikID int  NOT NULL,
    LimitOsob int  NOT NULL,
    KortID int  NOT NULL,
    KosztBezSprzetu decimal(5, 2)  NOT NULL,
    KosztZeSprzetem decimal(5, 2)  NOT NULL,
    CONSTRAINT GrafikZajec_pk PRIMARY KEY  (GrafikZajecID)
);

-- Table: InstancjaZajec
CREATE TABLE InstancjaZajec (
    InstancjaZajecID int  NOT NULL IDENTITY(1, 1),
    Data date  NOT NULL,
    CzyOdwolane bit  NULL,
    GrafikZajecID int  NOT NULL,
    CONSTRAINT InstancjaZajec_pk PRIMARY KEY  (InstancjaZajecID)
);

-- Table: InstancjaZajec_Klient
CREATE TABLE InstancjaZajec_Klient (
    InstancjaZajecKlientID int  NOT NULL IDENTITY(1, 1),
    KlientID int  NOT NULL,
    DataZapisu date  NOT NULL,
    DataWypisu date  NULL,
    CzyUwzglednicSprzet bit  NOT NULL DEFAULT 0,
    InstancjaZajecID int  NOT NULL,
    CzyOplacone bit  NOT NULL,
    CzyZwroconoPieniadze bit  NOT NULL,
    CONSTRAINT InstancjaZajec_Klient_pk PRIMARY KEY  (InstancjaZajecKlientID)
);

-- Table: Klient
CREATE TABLE Klient (
    KlientID int  NOT NULL,
    Saldo decimal(7,2)  NOT NULL,
    ZnizkaNaZajecia int  NULL,
    ZnizkaNaProdukty int  NULL,
    CONSTRAINT Klient_pk PRIMARY KEY  (KlientID)
);

-- Table: Klient_Tag
CREATE TABLE Klient_Tag (
    KlientID int  NOT NULL,
    TagID int  NOT NULL,
    CONSTRAINT Klient_Tag_pk PRIMARY KEY  (KlientID,TagID)
);

-- Table: Kort
CREATE TABLE Kort (
    KortID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(50)  NOT NULL,
    CONSTRAINT Kort_pk PRIMARY KEY  (KortID)
);

-- Table: Ocena
CREATE TABLE Ocena (
    OcenaID int  NOT NULL IDENTITY(1, 1),
    Opis nvarchar(255)  NOT NULL,
    Gwiazdki int  NOT NULL,
    InstancjaZajecKlientID int  NULL,
    DataWystawienia date  NOT NULL,
    CONSTRAINT Ocena_pk PRIMARY KEY  (OcenaID)
);

-- Table: Osoba
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

-- Table: PoziomZajec
CREATE TABLE PoziomZajec (
    IdPoziomZajec int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(100)  NOT NULL,
    CONSTRAINT PoziomZajec_pk PRIMARY KEY  (IdPoziomZajec)
);

-- Table: Pracownik
CREATE TABLE Pracownik (
    PracownikID int  NOT NULL,
    IdTypPracownika int  NOT NULL,
    DataZatrudnienia date  NOT NULL,
    DataZwolnienia date  NULL,
    CONSTRAINT Pracownik_pk PRIMARY KEY  (PracownikID)
);

-- Table: Produkt
CREATE TABLE Produkt (
    ProduktID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(100)  NOT NULL,
    Producent nvarchar(100)  NOT NULL,
    LiczbaNaStanie int  NOT NULL,
    Koszt decimal(5, 2)  NOT NULL,
    ZdjecieUrl nvarchar(100)  NOT NULL,
    CONSTRAINT Produkt_pk PRIMARY KEY  (ProduktID)
);

-- Table: Rezerwacja
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
    CzyOplacona bit  NOT NULL,
    CzyOdwolana bit  NOT NULL,
    CzyZwroconoPieniadze bit  NOT NULL,
    CONSTRAINT Rezerwacja_pk PRIMARY KEY  (RezerwacjaID)
);

-- Table: Tag
CREATE TABLE Tag (
    TagID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(70)  NOT NULL,
    CONSTRAINT Tag_pk PRIMARY KEY  (TagID)
);

-- Table: Trener_Certyfikat
CREATE TABLE Trener_Certyfikat (
    PracownikID int  NOT NULL,
    CertyfikatID int  NOT NULL,
    DataOtrzymania date  NOT NULL,
    CONSTRAINT Trener_Certyfikat_pk PRIMARY KEY  (PracownikID,CertyfikatID)
);

-- Table: TypPracownika
CREATE TABLE TypPracownika (
    IdTypPracownika int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(200)  NOT NULL,
    CONSTRAINT TypPracownika_pk PRIMARY KEY  (IdTypPracownika)
);

-- Table: WyjatkoweGodzinyPracy
CREATE TABLE WyjatkoweGodzinyPracy (
    WyjatkoweGodzinyPracyID int  NOT NULL IDENTITY(1, 1),
    Data date  NOT NULL,
    GodzinaOtwarcia time(0)  NOT NULL,
    GodzinaZamkniecia time(0)  NOT NULL,
    CONSTRAINT WyjatkoweGodzinyPracy_pk PRIMARY KEY  (WyjatkoweGodzinyPracyID)
);

-- Table: Zadanie
CREATE TABLE Zadanie (
    ZadanieID int  NOT NULL IDENTITY(1, 1),
    Opis nvarchar(500)  NOT NULL,
    DataDo date  NULL,
    PracownikID int  NOT NULL,
    PracownikZlecajacyID int  NOT NULL,
    CONSTRAINT Zadanie_pk PRIMARY KEY  (ZadanieID)
);

-- Table: Zajecia
CREATE TABLE Zajecia (
    ZajeciaID int  NOT NULL IDENTITY(1, 1),
    Nazwa nvarchar(100)  NOT NULL,
    IdPoziomZajec int  NOT NULL,
    CONSTRAINT Zajecia_pk PRIMARY KEY  (ZajeciaID)
);

-- Table: Zamowienie
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

-- Table: Zamowienie_Produkt
CREATE TABLE Zamowienie_Produkt (
    ZamowienieID int  NOT NULL,
    ProduktID int  NOT NULL,
    Liczba int  NOT NULL,
    Koszt decimal(5, 2)  NOT NULL,
    CONSTRAINT Zamowienie_Produkt_pk PRIMARY KEY  (ZamowienieID,ProduktID)
);

-- Table: Zastepstwo
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

-- foreign keys
-- Reference: BrakDostepnosci_Pracownik (table: BrakDostepnosci)
ALTER TABLE BrakDostepnosci ADD CONSTRAINT BrakDostepnosci_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

-- Reference: GrafikZajec_Kort (table: GrafikZajec)
ALTER TABLE GrafikZajec ADD CONSTRAINT GrafikZajec_Kort
    FOREIGN KEY (KortID)
    REFERENCES Kort (KortID);

-- Reference: GrafikZajec_Pracownik (table: GrafikZajec)
ALTER TABLE GrafikZajec ADD CONSTRAINT GrafikZajec_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

-- Reference: GrafikZajec_Zajecia (table: GrafikZajec)
ALTER TABLE GrafikZajec ADD CONSTRAINT GrafikZajec_Zajecia
    FOREIGN KEY (ZajeciaID)
    REFERENCES Zajecia (ZajeciaID);

-- Reference: InstancjaZajec_GrafikZajec (table: InstancjaZajec)
ALTER TABLE InstancjaZajec ADD CONSTRAINT InstancjaZajec_GrafikZajec
    FOREIGN KEY (GrafikZajecID)
    REFERENCES GrafikZajec (GrafikZajecID);

-- Reference: InstancjaZajec_Klient (table: InstancjaZajec_Klient)
ALTER TABLE InstancjaZajec_Klient ADD CONSTRAINT InstancjaZajec_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID);

-- Reference: InstancjaZajec_Klient_InstancjaZajec (table: InstancjaZajec_Klient)
ALTER TABLE InstancjaZajec_Klient ADD CONSTRAINT InstancjaZajec_Klient_InstancjaZajec
    FOREIGN KEY (InstancjaZajecID)
    REFERENCES InstancjaZajec (InstancjaZajecID);

-- Reference: Klient_Osoba (table: Klient)
ALTER TABLE Klient ADD CONSTRAINT Klient_Osoba
    FOREIGN KEY (KlientID)
    REFERENCES Osoba (OsobaID);

-- Reference: Ocena_InstancjaZajec_Klient (table: Ocena)
ALTER TABLE Ocena ADD CONSTRAINT Ocena_InstancjaZajec_Klient
    FOREIGN KEY (InstancjaZajecKlientID)
    REFERENCES InstancjaZajec_Klient (InstancjaZajecKlientID);

-- Reference: Posiadanie_Certyfikat (table: Trener_Certyfikat)
ALTER TABLE Trener_Certyfikat ADD CONSTRAINT Posiadanie_Certyfikat
    FOREIGN KEY (CertyfikatID)
    REFERENCES Certyfikat (CertyfikatID);

-- Reference: Posiadanie_Klient (table: Klient_Tag)
ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID)
    ON DELETE  CASCADE 
    ON UPDATE  CASCADE;

-- Reference: Posiadanie_Tag (table: Klient_Tag)
ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Tag
    FOREIGN KEY (TagID)
    REFERENCES Tag (TagID);

-- Reference: Pracownik_Osoba (table: Pracownik)
ALTER TABLE Pracownik ADD CONSTRAINT Pracownik_Osoba
    FOREIGN KEY (PracownikID)
    REFERENCES Osoba (OsobaID);

-- Reference: Pracownik_TypPracownika (table: Pracownik)
ALTER TABLE Pracownik ADD CONSTRAINT Pracownik_TypPracownika
    FOREIGN KEY (IdTypPracownika)
    REFERENCES TypPracownika (IdTypPracownika);

-- Reference: Rezerwacja_Klient (table: Rezerwacja)
ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID);

-- Reference: Rezerwacja_Kort (table: Rezerwacja)
ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Kort
    FOREIGN KEY (KortID)
    REFERENCES Kort (KortID);

-- Reference: Rezerwacja_Pracownik (table: Rezerwacja)
ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Pracownik
    FOREIGN KEY (TrenerID)
    REFERENCES Pracownik (PracownikID);

-- Reference: Trener_Certyfikat_Pracownik (table: Trener_Certyfikat)
ALTER TABLE Trener_Certyfikat ADD CONSTRAINT Trener_Certyfikat_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID)
    ON DELETE  CASCADE 
    ON UPDATE  CASCADE;

-- Reference: Zadanie_Pracownik (table: Zadanie)
ALTER TABLE Zadanie ADD CONSTRAINT Zadanie_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

-- Reference: Zadanie_PracownikZlecajacy (table: Zadanie)
ALTER TABLE Zadanie ADD CONSTRAINT Zadanie_PracownikZlecajacy
    FOREIGN KEY (PracownikZlecajacyID)
    REFERENCES Pracownik (PracownikID);

-- Reference: Zajecia_PoziomZajec (table: Zajecia)
ALTER TABLE Zajecia ADD CONSTRAINT Zajecia_PoziomZajec
    FOREIGN KEY (IdPoziomZajec)
    REFERENCES PoziomZajec (IdPoziomZajec);

-- Reference: Zamowienie_Klient (table: Zamowienie)
ALTER TABLE Zamowienie ADD CONSTRAINT Zamowienie_Klient
    FOREIGN KEY (KlientID)
    REFERENCES Klient (KlientID);

-- Reference: Zamowienie_Pracownik (table: Zamowienie)
ALTER TABLE Zamowienie ADD CONSTRAINT Zamowienie_Pracownik
    FOREIGN KEY (PracownikID)
    REFERENCES Pracownik (PracownikID);

-- Reference: Zamowienie_Produkt_Produkt (table: Zamowienie_Produkt)
ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Produkt
    FOREIGN KEY (ProduktID)
    REFERENCES Produkt (ProduktID);

-- Reference: Zamowienie_Produkt_Zamowienie (table: Zamowienie_Produkt)
ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Zamowienie
    FOREIGN KEY (ZamowienieID)
    REFERENCES Zamowienie (ZamowienieID)
    ON DELETE  CASCADE 
    ON UPDATE  CASCADE;

-- Reference: Zastepstwo_PracownikNieobecny (table: Zastepstwo)
ALTER TABLE Zastepstwo ADD CONSTRAINT Zastepstwo_PracownikNieobecny
    FOREIGN KEY (PracownikNieobecnyID)
    REFERENCES Pracownik (PracownikID);

-- Reference: Zastepstwo_PracownikZastepujacy (table: Zastepstwo)
ALTER TABLE Zastepstwo ADD CONSTRAINT Zastepstwo_PracownikZastepujacy
    FOREIGN KEY (PracownikZastepujacyID)
    REFERENCES Pracownik (PracownikID);

-- Reference: Zastepstwo_PracownikZatwierdzajacy (table: Zastepstwo)
ALTER TABLE Zastepstwo ADD CONSTRAINT Zastepstwo_PracownikZatwierdzajacy
    FOREIGN KEY (PracownikZatwierdzajacyID)
    REFERENCES Pracownik (PracownikID);

-- End of file.


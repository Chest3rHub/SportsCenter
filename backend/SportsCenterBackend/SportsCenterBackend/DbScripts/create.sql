CREATE DATABASE SportsCenter;

CREATE TABLE Certyfikat (
                            CertyfikatID int  NOT NULL IDENTITY,
                            Nazwa varchar(255)  NOT NULL,
                            CONSTRAINT Certyfikat_pk PRIMARY KEY  (CertyfikatID)
);

CREATE TABLE Grafik (
                        GrafikID int  NOT NULL IDENTITY,
                        Zajecia_w_grafikuID int  NOT NULL,
                        RezerwacjaID int  NOT NULL,
                        Data date  NOT NULL,
                        Godzina_od time  NOT NULL,
                        Czas_trwania int  NOT NULL,
                        CONSTRAINT Grafik_pk PRIMARY KEY  (GrafikID)
);

CREATE TABLE Klient (
                        KlientID int  NOT NULL,
                        Saldo int  NOT NULL,
                        Znizka_zajecia int  NULL,
                        Znizka_produkty int  NULL,
                        CONSTRAINT Klient_pk PRIMARY KEY  (KlientID)
);

CREATE TABLE Klient_Tag (
                            KlientTagID int  NOT NULL IDENTITY,
                            Klient_KlientID int  NOT NULL,
                            Tag_TagID int  NOT NULL,
                            CONSTRAINT Klient_Tag_pk PRIMARY KEY  (KlientTagID)
);

CREATE TABLE Kort (
                      KortID int  NOT NULL IDENTITY,
                      Nazwa varchar(20)  NOT NULL,
                      CONSTRAINT Kort_pk PRIMARY KEY  (KortID)
);

CREATE TABLE Ocena (
                       OcenaID int  NOT NULL IDENTITY,
                       Klient_KlientID int  NOT NULL,
                       Trener_TrenerID int  NULL,
                       Zajecia_w_grafiku_Zajecia_w_grafikuID int  NULL,
                       Opis varchar(255)  NOT NULL,
                       Gwiazdki int  NOT NULL,
                       CONSTRAINT Ocena_pk PRIMARY KEY  (OcenaID)
);

CREATE TABLE Osoba (
                       OsobaID int  NOT NULL IDENTITY,
                       Imie varchar(50)  NOT NULL,
                       Nazwisko varchar(50)  NOT NULL,
                       Email varchar(50)  NOT NULL,
                       Haslo varchar(100)  NOT NULL,
                       Data_ur date  NULL,
                       Nr_tel varchar(15)  NOT NULL,
                       Pesel varchar(11)  NULL,
                       Adres varchar(255)  NOT NULL,
                       CONSTRAINT Osoba_pk PRIMARY KEY  (OsobaID)
);

CREATE TABLE Pomoc_sprzatajaca (
                                   Pomoc_sprzatajacaID int  NOT NULL IDENTITY,
                                   CONSTRAINT Pomoc_sprzatajaca_pk PRIMARY KEY  (Pomoc_sprzatajacaID)
);

CREATE TABLE Posiadanie (
                            PosiadanieID int  NOT NULL IDENTITY,
                            TrenerID int  NOT NULL,
                            CertyfikatID int  NOT NULL,
                            Data_otrzymania date  NOT NULL,
                            CONSTRAINT Posiadanie_pk PRIMARY KEY  (PosiadanieID)
);

CREATE TABLE Pracownik_administracyjny (
                                           Pracownik_admID int  NOT NULL IDENTITY,
                                           CONSTRAINT Pracownik_administracyjny_pk PRIMARY KEY  (Pracownik_admID)
);

CREATE TABLE Produkt (
                         ProduktID int  NOT NULL IDENTITY,
                         Nazwa varchar(20)  NOT NULL,
                         Producent varchar(20)  NOT NULL,
                         Ilosc int  NOT NULL,
                         Koszt int  NOT NULL,
                         Zdjecie image  NOT NULL,
                         CONSTRAINT Produkt_pk PRIMARY KEY  (ProduktID)
);

CREATE TABLE Rezerwacja (
                            RezerwacjaID int  NOT NULL IDENTITY,
                            KlientID int  NOT NULL,
                            KortID int  NULL,
                            TrenerID int  NULL,
                            Zajecia_ZajeciaID int  NULL,
                            CONSTRAINT Rezerwacja_pk PRIMARY KEY  (RezerwacjaID)
);

CREATE TABLE Todo (
                      TodoID int  NOT NULL IDENTITY,
                      Pracownik_admID int  NULL,
                      Wlasciciel_klubuID int  NULL,
                      Pomoc_sprzatajacaID int  NULL,
                      TrenerID int  NULL,
                      Opis int  NOT NULL,
                      CONSTRAINT TODO_pk PRIMARY KEY  (TodoID)
);

CREATE TABLE Tag (
                     TagID int  NOT NULL IDENTITY,
                     Nazwa varchar(20)  NOT NULL,
                     CONSTRAINT Tag_pk PRIMARY KEY  (TagID)
);

CREATE TABLE Trener (
                        TrenerID int  NOT NULL IDENTITY,
                        CONSTRAINT Trener_pk PRIMARY KEY  (TrenerID)
);

CREATE TABLE Wlasciciel_klubu (
                                  Wlasciciel_klubuID int  NOT NULL IDENTITY,
                                  CONSTRAINT Wlasciciel_klubu_pk PRIMARY KEY  (Wlasciciel_klubuID)
);

CREATE TABLE Zajecia (
                         ZajeciaID int  NOT NULL IDENTITY,
                         Nazwa varchar(50)  NOT NULL,
                         poziom varchar(50)  NOT NULL,
                         Czy_rezerwacja_prywatna bit  NOT NULL,
                         CONSTRAINT Zajecia_pk PRIMARY KEY  (ZajeciaID)
);

CREATE TABLE Zajecia_w_grafiku (
                                   Zajecia_w_grafikuID int  NOT NULL IDENTITY,
                                   ZajeciaID int  NOT NULL,
                                   TrenerID int  NOT NULL,
                                   KortID int  NOT NULL,
                                   Nazwa_grupy varchar(100)  NOT NULL,
                                   CONSTRAINT Zajecia_w_grafiku_pk PRIMARY KEY  (Zajecia_w_grafikuID)
);

CREATE TABLE Zamowienie (
                            ZamowienieID int  NOT NULL IDENTITY,
                            KlientID int  NOT NULL,
                            CONSTRAINT Zamowienie_pk PRIMARY KEY  (ZamowienieID)
);

CREATE TABLE Zamowienie_Produkt (
                                    ZamowienieProduktID int  NOT NULL IDENTITY,
                                    ZamowienieID int  NOT NULL,
                                    ProduktID int  NOT NULL,
                                    Koszt int  NOT NULL,
                                    Data_zamowienia date  NOT NULL,
                                    CONSTRAINT Zamowienie_Produkt_pk PRIMARY KEY  (ZamowienieProduktID)
);

CREATE TABLE Zapis (
                       ZapisID int  NOT NULL IDENTITY,
                       Zajecia_w_grafikuID int  NOT NULL,
                       KlientID int  NOT NULL,
                       CONSTRAINT Zapis_pk PRIMARY KEY  (ZapisID)
);


ALTER TABLE Zajecia_w_grafiku ADD CONSTRAINT Grafik_Kort
    FOREIGN KEY (KortID)
        REFERENCES Kort (KortID);

ALTER TABLE Grafik ADD CONSTRAINT Grafik_Rezerwacja
    FOREIGN KEY (RezerwacjaID)
        REFERENCES Rezerwacja (RezerwacjaID);

ALTER TABLE Zajecia_w_grafiku ADD CONSTRAINT Grafik_Trener
    FOREIGN KEY (TrenerID)
        REFERENCES Trener (TrenerID);

ALTER TABLE Zajecia_w_grafiku ADD CONSTRAINT Grafik_Zajecia
    FOREIGN KEY (ZajeciaID)
        REFERENCES Zajecia (ZajeciaID);

ALTER TABLE Grafik ADD CONSTRAINT Grafik_Zajecia_w_grafiku
    FOREIGN KEY (Zajecia_w_grafikuID)
        REFERENCES Zajecia_w_grafiku (Zajecia_w_grafikuID);

ALTER TABLE Klient ADD CONSTRAINT Klient_Osoba
    FOREIGN KEY (KlientID)
        REFERENCES Osoba (OsobaID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_Klient
    FOREIGN KEY (Klient_KlientID)
        REFERENCES Klient (KlientID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_Trener
    FOREIGN KEY (Trener_TrenerID)
        REFERENCES Trener (TrenerID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_Zajecia_w_grafiku
    FOREIGN KEY (Zajecia_w_grafiku_Zajecia_w_grafikuID)
        REFERENCES Zajecia_w_grafiku (Zajecia_w_grafikuID);

ALTER TABLE Pomoc_sprzatajaca ADD CONSTRAINT Pomoc_sprzatajaca_Osoba
    FOREIGN KEY (Pomoc_sprzatajacaID)
        REFERENCES Osoba (OsobaID);

ALTER TABLE Posiadanie ADD CONSTRAINT Posiadanie_Certyfikat
    FOREIGN KEY (CertyfikatID)
        REFERENCES Certyfikat (CertyfikatID);

ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Klient
    FOREIGN KEY (Klient_KlientID)
        REFERENCES Klient (KlientID);

ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Tag
    FOREIGN KEY (Tag_TagID)
        REFERENCES Tag (TagID);

ALTER TABLE Posiadanie ADD CONSTRAINT Posiadanie_Trener
    FOREIGN KEY (TrenerID)
        REFERENCES Trener (TrenerID);

ALTER TABLE Pracownik_administracyjny ADD CONSTRAINT Pracownik_administracyjny_Osoba
    FOREIGN KEY (Pracownik_admID)
        REFERENCES Osoba (OsobaID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Klient
    FOREIGN KEY (KlientID)
        REFERENCES Klient (KlientID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Kort
    FOREIGN KEY (KortID)
        REFERENCES Kort (KortID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Trener
    FOREIGN KEY (TrenerID)
        REFERENCES Trener (TrenerID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Zajecia
    FOREIGN KEY (Zajecia_ZajeciaID)
        REFERENCES Zajecia (ZajeciaID);

ALTER TABLE Todo ADD CONSTRAINT TODO_Pomoc_sprzatajaca
    FOREIGN KEY (Pomoc_sprzatajacaID)
        REFERENCES Pomoc_sprzatajaca (Pomoc_sprzatajacaID);

ALTER TABLE Todo ADD CONSTRAINT TODO_Pracownik_administracyjny
    FOREIGN KEY (Pracownik_admID)
        REFERENCES Pracownik_administracyjny (Pracownik_admID);

ALTER TABLE Todo ADD CONSTRAINT TODO_Trener
    FOREIGN KEY (TrenerID)
        REFERENCES Trener (TrenerID);

ALTER TABLE Todo ADD CONSTRAINT TODO_Wlasciciel_klubu
    FOREIGN KEY (Wlasciciel_klubuID)
        REFERENCES Wlasciciel_klubu (Wlasciciel_klubuID);

ALTER TABLE Trener ADD CONSTRAINT Trener_Osoba
    FOREIGN KEY (TrenerID)
        REFERENCES Osoba (OsobaID);

ALTER TABLE Wlasciciel_klubu ADD CONSTRAINT Wlasciciel_klubu_Osoba
    FOREIGN KEY (Wlasciciel_klubuID)
        REFERENCES Osoba (OsobaID);

ALTER TABLE Zamowienie ADD CONSTRAINT Zamowienie_Klient
    FOREIGN KEY (KlientID)
        REFERENCES Klient (KlientID);

ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Produkt
    FOREIGN KEY (ProduktID)
        REFERENCES Produkt (ProduktID);

ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Zamowienie
    FOREIGN KEY (ZamowienieID)
        REFERENCES Zamowienie (ZamowienieID);

ALTER TABLE Zapis ADD CONSTRAINT Zapis_Klient
    FOREIGN KEY (KlientID)
        REFERENCES Klient (KlientID);

ALTER TABLE Zapis ADD CONSTRAINT Zapis_Zajecia_w_grafiku
    FOREIGN KEY (Zajecia_w_grafikuID)
        REFERENCES Zajecia_w_grafiku (Zajecia_w_grafikuID);



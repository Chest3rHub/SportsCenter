
CREATE TABLE Certyfikat (
                            CertyfikatID int  NOT NULL AUTO_INCREMENT,
                            Nazwa varchar(255)  NOT NULL,
                            CONSTRAINT Certyfikat_pk PRIMARY KEY (CertyfikatID)
);

CREATE TABLE Grafik (
                        GrafikID int  NOT NULL,
                        Zajecia_w_grafikuID int  NOT NULL,
                        RezerwacjaID int  NOT NULL,
                        Data date  NOT NULL,
                        Godzina_od time  NOT NULL,
                        Czas_trwania int  NOT NULL,
                        CONSTRAINT Grafik_pk PRIMARY KEY (GrafikID)
);

CREATE TABLE Klient (
                        KlientID int  NOT NULL,
                        Saldo int  NOT NULL,
                        Znizka_zajecia int  NULL,
                        Znizka_produkty int  NULL,
                        CONSTRAINT Klient_pk PRIMARY KEY (KlientID)
);

CREATE TABLE Klient_Tag (
                            KlientTagID int  NOT NULL,
                            Klient_KlientID int  NOT NULL,
                            Tag_TagID int  NOT NULL,
                            CONSTRAINT Klient_Tag_pk PRIMARY KEY (KlientTagID)
);

CREATE TABLE Kort (
                      KortID int  NOT NULL AUTO_INCREMENT,
                      Nazwa varchar(20)  NOT NULL,
                      CONSTRAINT Kort_pk PRIMARY KEY (KortID)
);

CREATE TABLE Ocena (
                       OcenaID int  NOT NULL AUTO_INCREMENT,
                       Klient_KlientID int  NOT NULL,
                       Trener_TrenerID int  NULL,
                       Zajecia_w_grafiku_Zajecia_w_grafikuID int  NULL,
                       Opis varchar(255)  NOT NULL,
                       Gwiazdki int  NOT NULL,
                       CONSTRAINT Ocena_pk PRIMARY KEY (OcenaID)
);

CREATE TABLE Osoba (
                       OsobaID int  NOT NULL AUTO_INCREMENT,
                       Imie varchar(50)  NOT NULL,
                       Nazwisko varchar(50)  NOT NULL,
                       Email varchar(50)  NOT NULL,
                       Haslo varchar(50)  NOT NULL,
                       Data_ur date  NULL,
                       Nr_tel varchar(15)  NOT NULL,
                       Pesel varchar(11)  NULL,
                       Adres varchar(255)  NOT NULL,
                       CONSTRAINT Osoba_pk PRIMARY KEY (OsobaID)
);

CREATE TABLE Pomoc_sprzatajaca (
                                   Pomoc_sprzatajacaID int  NOT NULL,
                                   CONSTRAINT Pomoc_sprzatajaca_pk PRIMARY KEY (Pomoc_sprzatajacaID)
);

CREATE TABLE Posiadanie (
                            PosiadanieID int  NOT NULL,
                            TrenerID int  NOT NULL,
                            CertyfikatID int  NOT NULL,
                            Data_otrzymania date  NOT NULL,
                            CONSTRAINT Posiadanie_pk PRIMARY KEY (PosiadanieID)
);

CREATE TABLE Pracownik_administracyjny (
                                           Pracownik_admID int  NOT NULL,
                                           CONSTRAINT Pracownik_administracyjny_pk PRIMARY KEY (Pracownik_admID)
);

CREATE TABLE Produkt (
                         ProduktID int  NOT NULL AUTO_INCREMENT,
                         Nazwa varchar(20)  NOT NULL,
                         Producent varchar(20)  NOT NULL,
                         Ilosc int  NOT NULL,
                         Koszt int  NOT NULL,
                         Zdjecie text  NOT NULL,
                         CONSTRAINT Produkt_pk PRIMARY KEY (ProduktID)
);

CREATE TABLE Rezerwacja (
                            RezerwacjaID int  NOT NULL,
                            KlientID int  NOT NULL,
                            KortID int  NULL,
                            TrenerID int  NULL,
                            Zajecia_ZajeciaID int  NULL,
                            CONSTRAINT Rezerwacja_pk PRIMARY KEY (RezerwacjaID)
);

CREATE TABLE TODO (
                      TODOID int  NOT NULL AUTO_INCREMENT,
                      Pracownik_admID int  NULL,
                      Wlasciciel_klubuID int  NULL,
                      Pomoc_sprzatajacaID int  NULL,
                      TrenerID int  NULL,
                      Opis int  NOT NULL,
                      CONSTRAINT TODO_pk PRIMARY KEY (TODOID)
);

CREATE TABLE Tag (
                     TagID int  NOT NULL AUTO_INCREMENT,
                     Nazwa varchar(20)  NOT NULL,
                     CONSTRAINT Tag_pk PRIMARY KEY (TagID)
);

CREATE TABLE Trener (
                        TrenerID int  NOT NULL,
                        CONSTRAINT Trener_pk PRIMARY KEY (TrenerID)
);

CREATE TABLE Wlasciciel_klubu (
                                  Wlasciciel_klubuID int  NOT NULL,
                                  CONSTRAINT Wlasciciel_klubu_pk PRIMARY KEY (Wlasciciel_klubuID)
);

CREATE TABLE Zajecia (
                         ZajeciaID int  NOT NULL AUTO_INCREMENT,
                         Nazwa varchar(50)  NOT NULL,
                         poziom varchar(50)  NOT NULL,
                         Czy_rezerwacja_prywatna bool  NOT NULL,
                         CONSTRAINT Zajecia_pk PRIMARY KEY (ZajeciaID)
);

CREATE TABLE Zajecia_w_grafiku (
                                   Zajecia_w_grafikuID int  NOT NULL AUTO_INCREMENT,
                                   ZajeciaID int  NOT NULL,
                                   TrenerID int  NOT NULL,
                                   KortID int  NOT NULL,
                                   Nazwa_grupy varchar(100)  NOT NULL,
                                   CONSTRAINT Zajecia_w_grafiku_pk PRIMARY KEY (Zajecia_w_grafikuID)
);

CREATE TABLE Zamowienie (
                            ZamowienieID int  NOT NULL,
                            KlientID int  NOT NULL,
                            CONSTRAINT Zamowienie_pk PRIMARY KEY (ZamowienieID)
);

CREATE TABLE Zamowienie_Produkt (
                                    ZamowienieProduktID int  NOT NULL,
                                    ZamowienieID int  NOT NULL,
                                    ProduktID int  NOT NULL,
                                    Koszt int  NOT NULL,
                                    Data_zamowienia date  NOT NULL,
                                    CONSTRAINT Zamowienie_Produkt_pk PRIMARY KEY (ZamowienieProduktID)
);

CREATE TABLE Zapis (
                       ZapisID int  NOT NULL,
                       Zajecia_w_grafikuID int  NOT NULL,
                       KlientID int  NOT NULL,
                       CONSTRAINT Zapis_pk PRIMARY KEY (ZapisID)
);

ALTER TABLE Zajecia_w_grafiku ADD CONSTRAINT Grafik_Kort FOREIGN KEY Grafik_Kort (KortID)
    REFERENCES Kort (KortID);

ALTER TABLE Grafik ADD CONSTRAINT Grafik_Rezerwacja FOREIGN KEY Grafik_Rezerwacja (RezerwacjaID)
    REFERENCES Rezerwacja (RezerwacjaID);

ALTER TABLE Zajecia_w_grafiku ADD CONSTRAINT Grafik_Trener FOREIGN KEY Grafik_Trener (TrenerID)
    REFERENCES Trener (TrenerID);

ALTER TABLE Zajecia_w_grafiku ADD CONSTRAINT Grafik_Zajecia FOREIGN KEY Grafik_Zajecia (ZajeciaID)
    REFERENCES Zajecia (ZajeciaID);

ALTER TABLE Grafik ADD CONSTRAINT Grafik_Zajecia_w_grafiku FOREIGN KEY Grafik_Zajecia_w_grafiku (Zajecia_w_grafikuID)
    REFERENCES Zajecia_w_grafiku (Zajecia_w_grafikuID);

ALTER TABLE Klient ADD CONSTRAINT Klient_Osoba FOREIGN KEY Klient_Osoba (KlientID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_Klient FOREIGN KEY Ocena_Klient (Klient_KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_Trener FOREIGN KEY Ocena_Trener (Trener_TrenerID)
    REFERENCES Trener (TrenerID);

ALTER TABLE Ocena ADD CONSTRAINT Ocena_Zajecia_w_grafiku FOREIGN KEY Ocena_Zajecia_w_grafiku (Zajecia_w_grafiku_Zajecia_w_grafikuID)
    REFERENCES Zajecia_w_grafiku (Zajecia_w_grafikuID);

ALTER TABLE Pomoc_sprzatajaca ADD CONSTRAINT Pomoc_sprzatajaca_Osoba FOREIGN KEY Pomoc_sprzatajaca_Osoba (Pomoc_sprzatajacaID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Posiadanie ADD CONSTRAINT Posiadanie_Certyfikat FOREIGN KEY Posiadanie_Certyfikat (CertyfikatID)
    REFERENCES Certyfikat (CertyfikatID);

ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Klient FOREIGN KEY Posiadanie_Klient (Klient_KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Klient_Tag ADD CONSTRAINT Posiadanie_Tag FOREIGN KEY Posiadanie_Tag (Tag_TagID)
    REFERENCES Tag (TagID);

ALTER TABLE Posiadanie ADD CONSTRAINT Posiadanie_Trener FOREIGN KEY Posiadanie_Trener (TrenerID)
    REFERENCES Trener (TrenerID);

ALTER TABLE Pracownik_administracyjny ADD CONSTRAINT Pracownik_administracyjny_Osoba FOREIGN KEY Pracownik_administracyjny_Osoba (Pracownik_admID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Klient FOREIGN KEY Rezerwacja_Klient (KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Kort FOREIGN KEY Rezerwacja_Kort (KortID)
    REFERENCES Kort (KortID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Trener FOREIGN KEY Rezerwacja_Trener (TrenerID)
    REFERENCES Trener (TrenerID);

ALTER TABLE Rezerwacja ADD CONSTRAINT Rezerwacja_Zajecia FOREIGN KEY Rezerwacja_Zajecia (Zajecia_ZajeciaID)
    REFERENCES Zajecia (ZajeciaID);

ALTER TABLE TODO ADD CONSTRAINT TODO_Pomoc_sprzatajaca FOREIGN KEY TODO_Pomoc_sprzatajaca (Pomoc_sprzatajacaID)
    REFERENCES Pomoc_sprzatajaca (Pomoc_sprzatajacaID);

ALTER TABLE TODO ADD CONSTRAINT TODO_Pracownik_administracyjny FOREIGN KEY TODO_Pracownik_administracyjny (Pracownik_admID)
    REFERENCES Pracownik_administracyjny (Pracownik_admID);

ALTER TABLE TODO ADD CONSTRAINT TODO_Trener FOREIGN KEY TODO_Trener (TrenerID)
    REFERENCES Trener (TrenerID);

ALTER TABLE TODO ADD CONSTRAINT TODO_Wlasciciel_klubu FOREIGN KEY TODO_Wlasciciel_klubu (Wlasciciel_klubuID)
    REFERENCES Wlasciciel_klubu (Wlasciciel_klubuID);

ALTER TABLE Trener ADD CONSTRAINT Trener_Osoba FOREIGN KEY Trener_Osoba (TrenerID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Wlasciciel_klubu ADD CONSTRAINT Wlasciciel_klubu_Osoba FOREIGN KEY Wlasciciel_klubu_Osoba (Wlasciciel_klubuID)
    REFERENCES Osoba (OsobaID);

ALTER TABLE Zamowienie ADD CONSTRAINT Zamowienie_Klient FOREIGN KEY Zamowienie_Klient (KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Produkt FOREIGN KEY Zamowienie_Produkt_Produkt (ProduktID)
    REFERENCES Produkt (ProduktID);

ALTER TABLE Zamowienie_Produkt ADD CONSTRAINT Zamowienie_Produkt_Zamowienie FOREIGN KEY Zamowienie_Produkt_Zamowienie (ZamowienieID)
    REFERENCES Zamowienie (ZamowienieID);

ALTER TABLE Zapis ADD CONSTRAINT Zapis_Klient FOREIGN KEY Zapis_Klient (KlientID)
    REFERENCES Klient (KlientID);

ALTER TABLE Zapis ADD CONSTRAINT Zapis_Zajecia_w_grafiku FOREIGN KEY Zapis_Zajecia_w_grafiku (Zajecia_w_grafikuID)
    REFERENCES Zajecia_w_grafiku (Zajecia_w_grafikuID);



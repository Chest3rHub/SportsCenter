
--insert roles into table TypPracownika
insert into TypPracownika values('Wlasciciel');
insert into TypPracownika values('Pracownik administracyjny');
insert into TypPracownika values('Trener');
insert into TypPracownika values('Pomoc sprzatajaca');

--insert sportsClub working hours into table GodzinyPracyKlubu
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'poniedzialek')
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'wtorek')
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'sroda')
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'czwartek')
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'piatek')
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'sobota')
insert into GodzinyPracyKlubu values ('10:00', '22:00', 'niedziela')

-- Wyjątkowe godziny pracy
INSERT INTO WyjatkoweGodzinyPracy VALUES
  ('2025-05-01', '12:00', '20:00'),  -- Święto pracy – późniejszy start
  ('2025-06-15', '08:00', '18:00'),  -- Zawody juniors – krótszy dzień
  ('2025-12-24', '08:00', '14:00');  -- Wigilia

-- Osoba
-- Klienci
INSERT INTO Osoba VALUES ('Jan', 'Dzban', 'klient@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1990-01-15', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Kasia', 'Zpodlasia', 'kasia@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1995-07-22', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Marek', 'Marucha', 'marek@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1987-12-03', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Dagmara', 'Puńska', 'dagmara@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1999-04-10', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Franek', 'Kasztanek', 'franek@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2000-09-17', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Muller', 'Milch', 'muller@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1985-03-05', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Barbara', 'Rabarbarowa', 'barbara@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1992-06-30', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Andrzej', 'Duduś', 'andrzej@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1989-11-21', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Rafał', 'Czaskoski', 'rafal@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1996-02-14', '123456789', null, 'Kliencka 15');
INSERT INTO Osoba VALUES ('Sławek', 'Memcen', 'slawek@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '1993-08-08', '123456789', null, 'Kliencka 15');

INSERT INTO Klient (KlientID, Saldo, ZnizkaNaZajecia, ZnizkaNaProdukty)
VALUES
    (1, 150.00, 10, 5),
    (2, 200.00, 15, 10),
    (3, 100.00, 5, 0),
    (4, 250.00, 20, 5),
    (5, 300.00, 10, 10),
    (6, 50.00, 0, 0),
    (7, 180.00, 5, 5),
    (8, 120.00, 0, 5),
    (9, 220.00, 15, 0),
    (10, 170.00, 10, 10);

-- Pracownicy
INSERT INTO Osoba VALUES ('Właściciel', 'Właścicielski', 'wlasciciel@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2001-04-06', '123456789', null, 'Pracownicza 15');
INSERT INTO Osoba VALUES ('Pracownik', 'Administrajski', 'pracownik@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2001-04-06', '123456789', null, 'Pracownicza 15');
INSERT INTO Osoba VALUES ('Trener', 'Trenerski', 'trener@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2001-04-06', '123456789', null, 'Pracownicza 15');
INSERT INTO Osoba VALUES ('Tomasz', 'Kolejowy', 'trener2@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2001-04-06', '123456789', null, 'Pracownicza 15');
INSERT INTO Osoba VALUES ('Kamil', 'Kort', 'trener3@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2001-04-06', '123456789', null, 'Pracownicza 15');
INSERT INTO Osoba VALUES ('Katarzyna', 'Sprzatajska', 'sprzataczka@wp.pl', 'AQAAAAIAAYagAAAAEGUJewyb+BNnmYNzKO+cJTkdGOneGfxOCoIGGJ3MZgK/qZymPY8PEvDcNAyP+Xz3yA==', '2001-04-06', '123456789', null, 'Pracownicza 15');

INSERT INTO Pracownik (PracownikID, IdTypPracownika, DataZatrudnienia, DataZwolnienia)
VALUES
    (11, 1, '2025-01-01', NULL),  -- Właściciel
    (12, 2, '2025-02-01', NULL),  -- Pracownik administracyjny
    (13, 3, '2025-03-01', NULL),  -- Trener
    (14, 3, '2025-04-01', NULL),  -- Trener2
    (15, 3, '2025-01-15', NULL),  -- Trener3
    (16, 4, '2025-01-20', NULL);  -- Pani sprzątająca

-- Aktualności
INSERT INTO Aktualnosci (Nazwa, Opis, WazneOd, WazneDo)
VALUES
    ('Cennik Rezerwacji',
     'Obowiązujący cennik: 70 zł/h za kort, 50 zł/h za trenera, 10 zł za sprzęt (jednorazowo w ramach rezerwacji). Dla stałych klientów dostępne zniżki! Szczegóły w recepcji.',
     '2025-05-29', NULL),

    ('Turniej Squasha',
     'Zapraszamy na wielki turniej squasha! Emocje, pot i rakiety w ruchu. Nagrody czekają, a najlepszy zgarnia wszystko!',
     '2025-04-10', '2025-10-15'),

    ('Promka na Badmintona',
     'Tylko w tym tygodniu badminton za pół ceny! Wbijaj z koleżką albo wujkiem, niech piórko leci jak szalone.',
     '2025-04-06', '2025-10-13'),

    ('Nowy Trener Tenisa',
     'Poznajcie Andrzeja – nowego trenera z 20-letnim doświadczeniem i bekhendem jak u Federera. Pierwszy trening gratis!',
     '2025-04-08', NULL),

    ('Wakacyjny Karnet',
     'Lato coraz bliżej, forma sama się nie zrobi. Kup wakacyjny karnet już dziś i zgarnij stylowy bidon gratis!',
     '2025-05-01', '2025-10-31'),

    ('Dzień Otwarty!',
     'Wpadnij z rodziną na Dzień Otwarty! Darmowe zajęcia, loteria fantowa i mini-turniej w tenisa stołowego. Będzie się działo!',
     '2025-04-20', '2025-10-20'),

    ('Badminton After Dark',
     'Nowość! Wieczorne granie przy ledach i muzyce – Badminton After Dark. Startujemy co piątek od 21:00. Wbijaj!',
     '2025-04-12', NULL);

-- Certyfikaty 
INSERT INTO Certyfikat (Nazwa)
VALUES
    ('Certyfikat Ukończenia Kursu Squasha – Poziom 1'),
    ('Certyfikat Mistrza Badmintona 2025'),
    ('Certyfikat Trenera Tenisa – Licencja A'),
    ('Certyfikat Uczestnictwa w Turnieju „Rakieta Roku”'),
    ('Certyfikat „Fair Play” – Sportowy Duch 2025'),
    ('Certyfikat Kondycji Żelaznej – Challenge 30 Dni'),
    ('Certyfikat Ukończenia Treningu Personalnego'),
    ('Certyfikat Dla Najbardziej Punktualnego Klubowicza'),
    ('Certyfikat Zaangażowania – Grupa Treningowa X'),
    ('Certyfikat Zwycięzcy Nocnego Turnieju Badmintona');

-- Certyfikat - Trener
-- Trener 1
INSERT INTO Trener_Certyfikat (PracownikID, CertyfikatID, DataOtrzymania)
VALUES
    (13, 1, '2023-06-10'),  -- Kurs Squasha – Poziom 1
    (13, 8, '2024-01-02');  -- Najbardziej Punktualny Klubowicz

-- Trener 2 
INSERT INTO Trener_Certyfikat (PracownikID, CertyfikatID, DataOtrzymania)
VALUES
    (14, 2, '2025-02-15'),  -- Mistrz Badmintona
    (14, 5, '2025-03-10'),  -- Fair Play
    (14, 10, '2025-03-20'); -- Nocny Turniej Badmintona

-- Trener 3
INSERT INTO Trener_Certyfikat (PracownikID, CertyfikatID, DataOtrzymania)
VALUES
    (15, 3, '2022-11-20'),  -- Trener Tenisa – Licencja A
    (15, 4, '2023-08-30'),  -- Turniej „Rakieta Roku”
    (15, 6, '2024-05-01'),  -- Kondycja Żelazna
    (15, 7, '2024-07-12'),  -- Trening Personalny
    (15, 9, '2025-01-05');  -- Zaangażowanie – Grupa X

-- Tagi
INSERT INTO Tag (Nazwa)
VALUES
    ('Squash – Początkujący'),
    ('Squash – Zaawansowany'),
    ('Badminton – Rodzinny'),
    ('Tenis – Junior'),
    ('Tenis – Senior'),
    ('Poranny Klubowicz'),
    ('Nocny Wojownik'),
    ('VIP Klubowicz'),
    ('Zaległość w płatności'),
    ('Rehabilitacja / powrót po kontuzji');


-- Tag - Klient
-- KlientID 1 
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (1, 1); -- Squash – Początkujący
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (1, 8); -- VIP Klubowicz

-- KlientID 2 
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (2, 3); -- Badminton – Rodzinny

-- KlientID 3 
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (3, 4); -- Tenis – Junior
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (3, 7); -- Nocny Wojownik
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (3, 10); -- Rehabilitacja

-- KlientID 4 
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (4, 5); -- Tenis – Senior
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (4, 9); -- Zaległość

-- KlientID 5 
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (5, 2); -- Squash – Zaawansowany
INSERT INTO Klient_Tag (KlientID, TagID) VALUES (5, 6); -- Poranny Klubowicz

-- Zadania TODO
INSERT INTO Zadanie (Opis, DataDo, PracownikID, PracownikZlecajacyID)
VALUES
    (N'Przygotuj playlistę zagrzewających hitów do treningów cardio.', '2025-04-15', 3, 1),
    (N'Zorganizuj mini-turniej squasha dla początkujących.', '2025-04-20', 3, 2),
    (N'Napisz motywujący cytat na tablicę w holu – coś z mocą!', '2025-04-08', 4, 1),
    (N'Przeprowadź test kondycji "Spocony Jeleń" na grupie B.', '2025-04-10', 4, 2),
    (N'Wymyśl nazwę dla nowego kursu badmintonowego (najlepiej rymowaną).', NULL, 4, 1),
    (N'Zaktualizuj grafik zajęć i wydrukuj nowe harmonogramy.', '2025-04-12', 5, 1),
    (N'Poprowadź pokazowy trening tenisa w przebraniu banana.', '2025-04-18', 5, 2),
    (N'Przygotuj certyfikaty dla uczestników turnieju „Rakieta Roku”.', '2025-04-22', 4, 1),
    (N'Zrób zdjęcia do galerii "Zawodnicy w akcji" i wrzuć na stronę.', NULL, 3, 2),
    (N'Zamów 10 nowych piłek do badmintona – różowych, jak prosili.', '2025-04-09', 4, 1);

-- Poziom zajęć
INSERT INTO PoziomZajec VALUES ('Poczatkujacy');
INSERT INTO PoziomZajec VALUES ('Srednio zaawansowany');
INSERT INTO PoziomZajec VALUES ('Zaawansowany');

-- Zajecia
INSERT INTO Zajecia VALUES ('Tenis', 3);
INSERT INTO Zajecia VALUES ('Tenis', 2);
INSERT INTO Zajecia VALUES ('Tenis', 1);
INSERT INTO Zajecia VALUES ('Badminton', 3);
INSERT INTO Zajecia VALUES ('Badminton', 2);
INSERT INTO Zajecia VALUES ('Badminton', 1);
INSERT INTO Zajecia VALUES ('Squash', 3);
INSERT INTO Zajecia VALUES ('Squash', 2);
INSERT INTO Zajecia VALUES ('Squash', 1);

-- Kort
INSERT INTO Kort VALUES ('Kort B1');
INSERT INTO Kort VALUES ('Kort B2');
INSERT INTO Kort VALUES ('Kort T1');
INSERT INTO Kort VALUES ('Kort S1');

/* --------- GrafikZajec (26 V – 1 VI 2025) --------- */
INSERT INTO GrafikZajec VALUES ('2025-05-31','sobota',     '10:00:00',90, 1,13,10,1, 80.00,120.00);
INSERT INTO GrafikZajec VALUES ('2025-05-31','sobota',     '12:00:00',90, 2,13, 8,2, 75.00,115.00);
INSERT INTO GrafikZajec VALUES ('2025-05-31','sobota',     '14:00:00',90, 3,13,12,3, 70.00,110.00);
INSERT INTO GrafikZajec VALUES ('2025-06-01','niedziela',  '16:00:00',90, 4,14,10,4, 85.00,130.00);
INSERT INTO GrafikZajec VALUES ('2025-06-01','niedziela',  '18:00:00',90, 5,14, 8,2, 80.00,125.00);
INSERT INTO GrafikZajec VALUES ('2025-05-26','poniedzialek','20:00:00',90, 6,14,10,1, 75.00,120.00);
INSERT INTO GrafikZajec VALUES ('2025-05-27','wtorek',     '10:00:00',90, 7,15,10,3, 90.00,140.00);
INSERT INTO GrafikZajec VALUES ('2025-05-27','wtorek',     '12:00:00',90, 8,15,10,4, 85.00,135.00);
INSERT INTO GrafikZajec VALUES ('2025-05-27','wtorek',     '14:00:00',90, 9,15,12,1, 80.00,130.00);
INSERT INTO GrafikZajec VALUES ('2025-05-28','sroda',      '10:00:00',60, 8,13,11,1, 84.67,124.67);
INSERT INTO GrafikZajec VALUES ('2025-05-28','sroda',      '14:00:00',60, 9,14,10,2, 85.38,125.38);
INSERT INTO GrafikZajec VALUES ('2025-05-28','sroda',      '20:00:00',60, 1,13, 9,2, 86.83,126.83);
INSERT INTO GrafikZajec VALUES ('2025-05-29','czwartek',   '14:00:00',60, 2,15, 8,3, 78.93,118.93);
INSERT INTO GrafikZajec VALUES ('2025-05-29','czwartek',   '10:00:00',60, 3,14,11,2, 76.94,116.94);
INSERT INTO GrafikZajec VALUES ('2025-05-29','czwartek',   '12:00:00',60, 5,14,11,4, 81.58,121.58);
INSERT INTO GrafikZajec VALUES ('2025-05-30','piatek',     '10:00:00',60, 6,15,11,4, 70.39,110.39);
INSERT INTO GrafikZajec VALUES ('2025-05-30','piatek',     '12:00:00',60, 4,15, 9,2, 76.93,116.93);
INSERT INTO GrafikZajec VALUES ('2025-05-30','piatek',     '14:00:00',60, 3,14,10,2, 71.97,111.97);

/* --------- InstancjaZajec (mapa do powyższego grafiku) --------- */
INSERT INTO InstancjaZajec VALUES ('2025-05-31',0, 1);
INSERT INTO InstancjaZajec VALUES ('2025-05-31',0, 2);
INSERT INTO InstancjaZajec VALUES ('2025-05-31',0, 3);
INSERT INTO InstancjaZajec VALUES ('2025-06-01',0, 4);
INSERT INTO InstancjaZajec VALUES ('2025-06-01',0, 5);
INSERT INTO InstancjaZajec VALUES ('2025-05-26',0, 6);
INSERT INTO InstancjaZajec VALUES ('2025-05-27',0, 7);
INSERT INTO InstancjaZajec VALUES ('2025-05-27',0, 8);
INSERT INTO InstancjaZajec VALUES ('2025-05-27',0, 9);
INSERT INTO InstancjaZajec VALUES ('2025-05-28',0,10);
INSERT INTO InstancjaZajec VALUES ('2025-05-28',0,11);
INSERT INTO InstancjaZajec VALUES ('2025-05-28',0,12);
INSERT INTO InstancjaZajec VALUES ('2025-05-29',0,13);
INSERT INTO InstancjaZajec VALUES ('2025-05-29',0,14);
INSERT INTO InstancjaZajec VALUES ('2025-05-29',0,15);
INSERT INTO InstancjaZajec VALUES ('2025-05-30',0,16);
INSERT INTO InstancjaZajec VALUES ('2025-05-30',0,17);
INSERT INTO InstancjaZajec VALUES ('2025-05-30',0,18);

-- InstancjaZajec_Klient 
INSERT INTO InstancjaZajec_Klient
VALUES
-- Sobota 31 V  (Instancje 1-3)
    (1,'2025-05-31',NULL,1, 1,1,0),
    (2,'2025-05-31',NULL,0, 2,0,0),
    (3,'2025-05-31',NULL,1, 3,1,1),
-- Środa 28 V  (Instancje 10-12)
    (4,'2025-05-27',NULL,0,10,1,0),
    (5,'2025-05-27',NULL,1,10,1,0),
    (6,'2025-05-27',NULL,1,11,1,1),
    (7,'2025-05-27',NULL,0,11,1,0),
    (8,'2025-05-27',NULL,1,12,0,0),
-- Czwartek 29 V (Instancje 13-15)
    (9 ,'2025-05-28',NULL,1,13,1,0),
    (10,'2025-05-28',NULL,1,13,1,0),
    (1 ,'2025-05-28',NULL,0,14,1,1),
    (2 ,'2025-05-28',NULL,1,14,0,0),
    (3 ,'2025-05-28',NULL,0,15,1,0),
-- Piątek 30 V  (Instancje 16-18)
    (4 ,'2025-05-29',NULL,1,16,1,0),
    (5 ,'2025-05-29',NULL,0,16,0,0),
    (6 ,'2025-05-29',NULL,1,17,1,1),
    (7 ,'2025-05-29',NULL,1,17,1,0),
    (8 ,'2025-05-29',NULL,0,18,1,0),
    (9 ,'2025-05-29',NULL,1,18,0,0),
-- Niedziela 1 VI (Instancje 4-5)
    (1 ,'2025-05-31',NULL,0, 4,1,0),
    (4 ,'2025-05-31',NULL,1, 4,1,0),
    (7 ,'2025-05-31',NULL,0, 4,1,0),
    (2 ,'2025-05-31',NULL,1, 5,1,0),
    (5 ,'2025-05-31',NULL,0, 5,1,0),
    (8 ,'2025-05-31',NULL,1, 5,1,0),
-- Poniedziałek 26 V  (Instancja 6)
    (3 ,'2025-05-26',NULL,0, 6,1,0),
    (6 ,'2025-05-26',NULL,1, 6,1,0),
    (9 ,'2025-05-26',NULL,0, 6,1,0),
-- Wtorek 27 V  (Instancje 7-9)
    (1 ,'2025-05-26',NULL,1, 7,1,0),
    (2 ,'2025-05-26',NULL,0, 7,1,0),
    (10,'2025-05-26',NULL,1, 7,1,0),
    (3 ,'2025-05-26',NULL,1, 8,1,0),
    (4 ,'2025-05-26',NULL,0, 8,1,0),
    (5 ,'2025-05-26',NULL,1, 8,1,0),
    (6 ,'2025-05-26',NULL,0, 9,1,0),
    (7 ,'2025-05-26',NULL,1, 9,1,0),
    (8 ,'2025-05-26',NULL,0, 9,1,0),
    (9 ,'2025-05-26',NULL,1, 9,1,0),
    (10,'2025-05-26',NULL,0, 9,1,0);


-- Ocena
INSERT INTO Ocena
VALUES
    ('Bardzo dobra jakość treningu, polecam!', 5, 1, '2025-04-06'),
    ('Świetny trener, ale miejsce mogłoby być lepsze.', 4, 2, '2025-04-06'),
    ('Trening ok, ale brakowało motywacji.', 3, 3, '2025-04-06');


-- Rezerwacja
INSERT INTO Rezerwacja
VALUES
    (1, 1, '2025-04-10 10:00:00', '2025-04-10 11:00:00', '2025-04-06', 13, 1, 50.00, 1, 0, 0),
    (2, 1, '2025-04-11 10:00:00', '2025-04-11 11:00:00', '2025-04-06', NULL, 0, 40.00, 0, 0, 0),
    (3, 2, '2025-04-12 14:00:00', '2025-04-12 15:00:00', '2025-04-06', 14, 1, 60.00, 1, 1, 1);

-- Czerwiec 2025
INSERT INTO Rezerwacja
VALUES
    (1, 1, '2025-06-05 10:00:00', '2025-06-05 11:00:00', '2025-05-30', 13, 1, 60.00, 1, 0, 0),
    (2, 2, '2025-06-10 18:00:00', '2025-06-10 19:30:00', '2025-06-01', 14, 1, 105.00, 1, 0, 0),
    (3, 3, '2025-06-12 20:00:00', '2025-06-12 22:00:00', '2025-06-02', NULL, 0, 140.00, 1, 0, 0),
    (4, 4, '2025-06-15 13:00:00', '2025-06-15 14:30:00', '2025-06-05', 15, 0, 75.00, 1, 0, 0),
    (5, 1, '2025-06-20 14:00:00', '2025-06-20 15:00:00', '2025-06-10', NULL, 1, 60.00, 0, 1, 1),
    (6, 2, '2025-06-25 11:00:00', '2025-06-25 12:00:00', '2025-06-15', 13, 0, 40.00, 1, 0, 0),
    (1, 1, '2025-06-28 17:00:00', '2025-06-28 18:00:00', '2025-06-20', 14, 1, 60.00, 1, 0, 0);

-- Lipiec 2025
INSERT INTO Rezerwacja
VALUES
    (1, 3, '2025-07-03 10:00:00', '2025-07-03 11:30:00', '2025-06-25', 13, 1, 105.00, 1, 0, 0),
    (2, 4, '2025-07-05 16:00:00', '2025-07-05 17:00:00', '2025-06-27', NULL, 0, 40.00, 1, 0, 0),
    (3, 1, '2025-07-10 10:00:00', '2025-07-10 12:00:00', '2025-07-01', 15, 1, 60.00, 1, 0, 0),
    (4, 2, '2025-07-12 19:00:00', '2025-07-12 20:30:00', '2025-07-02', 13, 0, 105.00, 1, 0, 0),
    (5, 3, '2025-07-18 13:00:00', '2025-07-18 14:00:00', '2025-07-05', NULL, 1, 60.00, 0, 1, 1),
    (6, 4, '2025-07-22 15:00:00', '2025-07-22 16:00:00', '2025-07-10', 14, 0, 40.00, 1, 0, 0),
    (1, 2, '2025-07-25 11:00:00', '2025-07-25 12:00:00', '2025-07-15', 15, 1, 60.00, 1, 0, 0);

-- Sierpień 2025
INSERT INTO Rezerwacja
VALUES
    (1, 2, '2025-08-01 17:30:00', '2025-08-01 18:30:00', '2025-07-25', 15, 1, 60.00, 1, 0, 0),
    (2, 3, '2025-08-04 18:00:00', '2025-08-04 19:00:00', '2025-07-27', NULL, 0, 40.00, 1, 0, 0),
    (3, 4, '2025-08-10 20:00:00', '2025-08-10 22:00:00', '2025-08-01', 13, 1, 140.00, 1, 0, 0),
    (4, 1, '2025-08-15 15:00:00', '2025-08-15 16:00:00', '2025-08-05', 14, 0, 40.00, 1, 0, 0),
    (5, 1, '2025-08-20 14:00:00', '2025-08-20 15:30:00', '2025-08-10', 15, 1, 105.00, 1, 0, 0),
    (6, 3, '2025-08-25 11:00:00', '2025-08-25 12:00:00', '2025-08-15', NULL, 1, 60.00, 0, 1, 1),
    (1, 4, '2025-08-28 13:00:00', '2025-08-28 14:00:00', '2025-08-18', 13, 1, 60.00, 1, 0, 0);

INSERT INTO Rezerwacja
VALUES
    (2, 2, '2025-06-30 18:00:00', '2025-06-30 19:00:00', '2025-06-20', 13, 1, 60.00, 1, 0, 0),
    (5, 3, '2025-07-29 12:00:00', '2025-07-29 13:30:00', '2025-07-19', NULL, 0, 40.00, 1, 0, 0);

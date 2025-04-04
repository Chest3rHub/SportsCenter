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

-- Poziom zajęć
INSERT INTO PoziomZajec VALUES ('Poczatkujacy');
INSERT INTO PoziomZajec VALUES ('Srednio zaawansowany');
INSERT INTO PoziomZajec VALUES ('Zaawansowany');

-- Zajecia
INSERT INTO Zajecia VALUES ('Tenis grupa zaawansowana', 3);
INSERT INTO Zajecia VALUES ('Tenis grupa srednio zaawansowana', 2);
INSERT INTO Zajecia VALUES ('Tenis grupa poczatkujaca', 1);
INSERT INTO Zajecia VALUES ('Badminton grupa zaawansowana', 3);
INSERT INTO Zajecia VALUES ('Badminton grupa srednio zaawansowana', 2);
INSERT INTO Zajecia VALUES ('Badminton grupa poczatkujaca', 1);
INSERT INTO Zajecia VALUES ('Squash grupa zaawansowana', 3);
INSERT INTO Zajecia VALUES ('Squash grupa srednio zaawansowana', 2);
INSERT INTO Zajecia VALUES ('Squash grupa poczatkujaca', 1);

-- Kort
INSERT INTO Kort VALUES ('Kort A');
INSERT INTO Kort VALUES ('Kort B');
INSERT INTO Kort VALUES ('Kort C');
INSERT INTO Kort VALUES ('Kort D');

-- GrafikZajec
INSERT INTO GrafikZajec VALUES ('2025-04-05', 'sobota', '10:00:00', 90, 1, 1, 10, 1, 80.00, 120.00);
INSERT INTO GrafikZajec VALUES ('2025-04-05', 'sobota', '12:00:00', 90, 2, 1, 8, 2, 75.00, 115.00);
INSERT INTO GrafikZajec VALUES ('2025-04-05', 'sobota', '14:00:00', 90, 3, 1, 12, 3, 70.00, 110.00);
INSERT INTO GrafikZajec VALUES ('2025-04-06', 'niedziela', '16:00:00', 90, 4, 1, 10, 4, 85.00, 130.00);
INSERT INTO GrafikZajec VALUES ('2025-04-06', 'niedziela', '18:00:00', 90, 5, 1, 8, 2, 80.00, 125.00);
INSERT INTO GrafikZajec VALUES ('2025-04-07', 'poniedzialek', '20:00:00', 90, 6, 1, 10, 1, 75.00, 120.00);
INSERT INTO GrafikZajec VALUES ('2025-04-08', 'wtorek', '10:00:00', 90, 7, 1, 10, 3, 90.00, 140.00);
INSERT INTO GrafikZajec VALUES ('2025-04-08', 'wtorek', '12:00:00', 90, 8, 1, 10, 4, 85.00, 135.00);
INSERT INTO GrafikZajec VALUES ('2025-04-08', 'wtorek', '14:00:00', 90, 9, 1, 12, 1, 80.00, 130.00);

-- InstancjaZajec
INSERT INTO InstancjaZajec VALUES ('2025-04-05', 0, 1);
INSERT INTO InstancjaZajec VALUES ('2025-04-05', 0, 2);
INSERT INTO InstancjaZajec VALUES ('2025-04-05', 0, 3);
INSERT INTO InstancjaZajec VALUES ('2025-04-06', 0, 4);
INSERT INTO InstancjaZajec VALUES ('2025-04-06', 0, 5);
INSERT INTO InstancjaZajec VALUES ('2025-04-07', 0, 6);
INSERT INTO InstancjaZajec VALUES ('2025-04-08', 0, 7);
INSERT INTO InstancjaZajec VALUES ('2025-04-08', 0, 8);
INSERT INTO InstancjaZajec VALUES ('2025-04-08', 0, 9);

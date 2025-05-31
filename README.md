# SportsCenter

## Uruchomienie aplikacji
Aby uruchomić aplikację lokalnie, wykonaj poniższe kroki:

1. Sklonuj repozytorium:
```bash
git clone https://github.com/Chest3rHub/SportsCenter.git
```
2. Utwórz bazę danych w SQL Server Management Studio (nazwa SportsCenter). W folderze SportsCenter/backend/database-data znajdziesz skrypt tworzący bazę: createDbScript.sql
3. Zasiej dane do bazy. W tym samym folderze znajdziesz plik z przykładowymi danymi: seedData.sql
4. Zainstaluj zależności frontendu. Przejdź do folderu SportsCenter/frontend i w konsoli wpisz:
```bash
npm i
```
5. W razie potrzeby zmień connectionString. Znajduje się on na backendzie w pliku appSettings.json.

### Dodatkowe informacje 
1. Backend działa domyślnie na porcie 5277 - można to zmienić w pliku Program.cs i launchsettings.json na backendzie oraz w appConfig.js na frontendzie.
2. Fronted domyślnie działa na porcie 3000. Jeśli port ten będzie zajęty, aplikacja frontendowa uruchomi się na innym porcie, który należy wtedy dodać do konfiguracji CORS — można to zrobić w pliku Program.cs.

Po wykonaniu powyższych kroków można uruchomić aplikację. 
1. Z poziomu backendu przy użyciu np. Visual Studio.
2. Z poziomu frontendu korzystając z komendy 
```bash
npm start
```
w folderze SportsCenter/frontend.

### Testowi użytkownicy 
Każdy z użytkowników ma hasło 'Qwe123@@'
- klient@wp.pl
- wlasciciel@wp.pl
- pracownik@wp.pl
- trener@wp.pl
- sprzataczka@wp.pl

## Backend
Backend został zaimplementowany zgodnie z podziałem określanym jako Clean Architecture.
Wyróżnione wartstwy:
- SportsCenter.Api - warstwa w której zdeifniowane są kontrolery. Jedyna warstwa stanowiąca uruchamialną aplikację. W tej warstwie następuję spięcie
wszystkich zależności razem.
- SportsCenter.Application - warstwa, która stanowi łącznik między warstwą Core i Api. W tej warstwie nas†ępuje obsłużenie komendy/kwerendy. W handlerach w większości
znajduej się logika biznesowa. Handlery dla komend możemy znaleźć w warstwie aplikacji. Handlery dla kwerend mogą być w całości zaimplementowane w warstwie infrastruktury - zazwyczaj
kwerendy nie mają żadnej logiki biznesowej i sprowadzają się do wykonania odpowiedniego zapytania do źródła danych.
- SportsCenter.Core - warstwa logiki biznesowej. W naszym wypadku warstwa core jest w większości anemiczna. Logika znajduje się w warstwie aplikacji. Z czasem czześć logiki może zostać przeniesiona do warstwy core.
- SportsCenter.Infrastructure - warstwa w której znajdują się implementacje wszystkich serwisów wymagających komunikacji z zewnętrznymi serwisami - baza danych, logowanie, serwis do wysyłania emaili itd.

- SportsCenter.Tests.Unit - projekt zawierający testy jednostkowe
- SportsCenter.Tests.Integration - projekt zawierający testy integracyjne

### Decyzje architektoniczne
1. Kontroler pozwala na wysłanie komendy lub kwerendy. Kwerendy nie zmieniają stanu aplikacji. Komenda lub kwerenda jest wysyłana z pomocą biblioteki Mediatr do warstwy aplikacji. Handler dla komend znajduje się w warstwie aplikacji. Handler dla kwerend znajduje się w warstwie infrastruktury.
2. Warstwa aplikacja powinna izolować modele domenowe. Zawsze zwracamy DTO do klienta. Nie chcemy udostępniać na zewnątrz struktury wewnętrznej bazy danych.

## Frontend
Frontend aplikacji został zbudowany w oparciu o bibliotekę React oraz komponenty interfejsu użytkownika z biblioteki MUI (Material UI). Aplikacja wspiera dwujęzyczność (polski i angielski), a jej kod jest uporządkowany według funkcjonalnych katalogów.
Struktura katalogów:
- api – zawiera funkcje do komunikacji z backendem. Każda funkcja reprezentuje jedno konkretne wywołanie HTTP (np. dodanie aktywności sportowej) i korzysta z fetch z odpowiednimi nagłówkami.
- components – zawiera wielokrotnie używane komponenty interfejsu użytkownika, takie jak przyciski, formularze, niestandardowe inputy, tła itp.
- context – zawiera kontekst SportsContext, który zarządza m.in. rolą użytkownika, aktywnym routerem oraz słownikiem językowym. Zapewnione zostało również podtrzymywanie sesji. Z ciasteczek pobierany jest token, który przy wykonaniu zapytania getAccountInfo ustawia z powrotem wszelkie parametry.
- dictionary – zawiera słowniki językowe (pl, en).
- hooks – niestandardowe hooki, np. useDebounce (do opóźniania zapytań) oraz useDictionary (do obsługi języków).
- layouts – struktury układu strony, np. panele boczne, różniące się w zależności od typu użytkownika (np. właściciel, trener). Został zaimplementowany mechanizm automatycznego odświeżania tokenu co 30 minut, który zapewnia utrzymanie aktywnej sesji bez konieczności ponownego logowania przez maksymalnie 24 godziny.
- pages – główne widoki aplikacji, przypisane do konkretnych adresów URL. Każda strona stanowi samodzielny komponent i korzysta z odpowiednich layoutów oraz kontekstu.
- router – konfiguracja trasowania w aplikacji, przypisująca komponenty stron do odpowiednich ścieżek.
- utils – funkcje pomocnicze i narzędzia

### Cechy i założenia
1. Komunikacja z backendem odbywa się wyłącznie poprzez funkcje z katalogu api, co umożliwia łatwe zarządzanie i testowanie zapytań HTTP.
2. Aplikacja obsługuje dwa języki (polski i angielski) dzięki własnemu mechanizmowi tłumaczeń opartemu na słownikach językowych. Struktura rozwiązania umożliwia łatwe dodanie kolejnych języków w przyszłości.
3. Uwierzytelnianie oparte jest na tokenie JWT przechowywanym w ciasteczkach.
4. UI został zbudowany z wykorzystaniem komponentów MUI, zapewniających spójny wygląd.
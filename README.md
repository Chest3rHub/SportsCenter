# SportsCenter

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

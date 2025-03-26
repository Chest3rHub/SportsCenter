export default {
    navbar: {
      changeLanguageLabel: 'EN',
      anonymousUser: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        registerLabel: 'Rejestracja',
        loginLabel: 'Logowanie',
      },
      client: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        accountLabel: 'Konto',
        logoutLabel: 'Wyloguj',
      },
      owner: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        accountLabel: 'Konto',
        logoutLabel: 'Wyloguj',
      },
      employee: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        accountLabel: 'Konto',
        logoutLabel: 'Wyloguj',
      },
      coach: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        accountLabel: 'Konto',
        logoutLabel: 'Wyloguj',
      },
      cleaner: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        accountLabel: 'Konto',
        logoutLabel: 'Wyloguj',
      }
    },
    registerPage: {
      title: 'Rejestracja',
      firstNameLabel: 'Imię',
      lastNameLabel: 'Nazwisko',
      addressLabel: 'Adres',
      dateOfBirthLabel: 'Data urodzenia',
      phoneNumberLabel: 'Numer telefonu',
      emailLabel: 'E-mail',
      passwordLabel: 'Hasło',
      confirmPasswordLabel: 'Powtórz hasło',
      signUpLabel: 'Utwórz konto',
      firstNameError: 'Imię musi mieć od 2 do 50 znaków.',
      lastNameError: 'Nazwisko musi mieć od 2 do 50 znaków.',
      addressError: 'Adres musi mieć od 5 do 100 znaków.',
      emailError: 'Podaj poprawny adres e-mail.',
      passwordError: 'Hasło musi mieć co najmniej 6 znaków.',
      confirmPasswordError: 'Hasła muszą być identyczne.',
      birthDateError: 'Musisz mieć co najmniej 18 lat.',
      phoneNumberError: 'Numer telefonu musi być w poprawnym formacie (np. 123456789).',
      emailTakenError: 'Podany adres e-mail jest już zajęty.',
    },
    loginPage: {
      title: 'Logowanie',
      emailLabel:'E-mail',
      passwordLabel: 'Hasło',
      forgotPasswordLabel: 'Nie pamiętam hasła',
      signInLabel: 'Zaloguj',
      incorrectLoginLabel: "Niepoprawny login lub hasło",
    },
    errorPage: {
        returnLabel: 'Wróć',
    },
    sidebar: {
      clientSidebar: {
        newsLabel: 'Aktualności',
        myTimetableLabel: 'Mój grafik',
        myReservationsLabel: 'Moje rezerwacje',
        balanceLabel: 'Saldo',
        shopLabel: 'Sklep',
        accountLabel: 'Konto',
      },
      employeeSidebar: {
        clientsLabel: 'Klienci',
        timetableLabel: 'Grafik',
        todoLabel: 'Do zrobienia',
        changePasswordLabel: 'Zmiana hasła',
        reservationsLabel: 'Rezerwacje',
        trainingsLabel: 'Zajęcia',
        productsLabel: 'Produkty',
        newsLabel: 'Aktualności',
      },
      ownerSidebar: {
        employeesLabel: 'Pracownicy',
        clientsLabel: 'Klienci',
        timetableLabel: 'Grafik',
        todoLabel: 'Do zrobienia',
        changePasswordLabel: 'Zmiana hasła',
        trainingsLabel: 'Zajęcia',
        reservationsLabel: 'Rezerwacje',
        opinionsLabel: 'Opinie',
        productsLabel: 'Produkty',
        gearLabel: 'Sprzęt',
        newsLabel: 'Aktualności',
      },
      coachSidebar: {
        newsLabel: 'Aktualności',
        timetableLabel: 'Grafik',
        changePasswordLabel: 'Zmiana hasła',
      }
    },
    newsPage: {
      newsLabel: 'Aktualności',
      showLabel:'Wyświetl',
      editLabel: 'Edytuj',
      removeLabel: 'Usuń',
      nothingHappenedLabel: 'Ups, nic się ostatnio nie wydarzyło.',
      checkAgainSoonLabel: 'Sprawdź ponownie za jakiś czas!',
      addNewsLabel: 'Utwórz',
      createNewsLabel: 'Utwórz aktualność',
    },
    addNewsPage: {
      newsLabel: 'Aktualności',
      titleLabel: 'Nazwa',
      contentLabel: 'Opis',
      validFromLabel: 'Ważne od',
      validUntilLabel: 'Ważne do',
      saveLabel: 'Zapisz',
      titleError: 'Tytuł może mieć maksymalnie 20 znaków',
      contentError: 'Opis nie może przekraczać 4000 znaków',
      validFromError: 'Błąd daty',
      validUntilError: 'Błąd daty',
      returnLabel:'Wróć',
      successLabel:'Sukces!',
      savedSuccessLabel: 'Udało się zapisać zmiany!',
      clickAnywhereLabel:'Kliknij w dowolne miejsce ekranu.',
      failureLabel: 'Błąd!',
      savedFailureLabel: 'Nie udało się zapisać zmian!',
      clickAnywhereFailureLabel: 'Kliknij w dowolne miejsce ekranu.',
    },
    editNewsPage: {
      newsLabel: 'Edytuj aktualność',
      titleLabel: 'Nazwa',
      contentLabel: 'Opis',
      validFromLabel: 'Ważne od',
      validUntilLabel: 'Ważne do',
      saveLabel: 'Zapisz',
      titleError: 'Tytuł może mieć maksymalnie 20 znaków',
      contentError: 'Opis nie może przekraczać 4000 znaków',
      validFromError: 'Błąd daty',
      validUntilError: 'Błąd daty',
      returnLabel:'Wróć',
      successLabel:'Sukces!',
      savedSuccessLabel: 'Udało się zapisać zmiany!',
      clickAnywhereLabel:'Kliknij w dowolne miejsce ekranu.',
      failureLabel: 'Błąd!',
      savedFailureLabel: 'Nie udało się zapisać zmian!',
      clickAnywhereFailureLabel: 'Kliknij w dowolne miejsce ekranu.',
    },
    newsDetailsPage: {
      returnLabel: 'Wróć',
    },
    changePasswordPage: {
      changePasswordLabel: 'Zmiana hasła',
      newPasswordLabel: 'Nowe hasło',
      confirmNewPasswordLabel: 'Powtórz hasło',
      newPasswordError:'Hasło musi mieć co najmniej 6 znaków.',
      confirmNewPasswordError:'Hasła muszą być identyczne.',
      saveLabel: 'Zapisz',
      returnLabel:'Wróć',
      successLabel:'Sukces!',
      savedSuccessLabel: 'Udało się zapisać zmiany!',
      clickAnywhereLabel:'Kliknij w dowolne miejsce ekranu.',
      failureLabel: 'Błąd!',
      savedFailureLabel: 'Nie udało się zapisać zmian!',
      clickAnywhereFailureLabel: 'Kliknij w dowolne miejsce ekranu.',
    },
    employeesPage: {
      employeesLabel: 'Pracownicy',
      employeeLabel: 'Pracownik',
      positionLabel:'Stanowisko',
      changePasswordLabel:'Zmień hasło',
      fireLabel:'Zwolnij',
      confirmLabel:'Potwierdź zwolnienie:',
      noLabel:'Nie',
      yesLabel:'Tak',
    },
    accountPage: {
      title: 'Moje konto',
      personalDataLabel: 'Dane osobowe',
      firstNameLabel: 'Imię',
      lastNameLabel: 'Nazwisko',
      addressLabel: 'Adres',
      dateOfBirthLabel: 'Data urodzenia',
      phoneNumberLabel: 'Numer telefonu',
      emailLabel: 'E-mail',
      role: 'Rola',
      balance: 'Saldo',
      addBalance: 'Doładuj saldo',
      changePassword: 'Zmień hasło',
    },
  };

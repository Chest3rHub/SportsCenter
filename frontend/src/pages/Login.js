import React, { useState } from 'react';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import '../styles/auth.css';

function Login() {
  //State do przechowywania maila i hasla
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  //Funkcja obsługi logowania
  const handleSubmit = async (event) => {
    event.preventDefault(); // Zapobiega przeładowaniu strony

    //Tworzenie obiektu z danymi logowania
    const loginData = {
      email: email,
      password: password
    };

    try {
      //Wysłanie żądania POST na endpoint /login
      const response = await fetch('https://localhost:7085/api/clients/login', {

        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData) //dane w formacie JSON
      });

      //odpowiedź jest OK
      if (response.ok) {
        const data = await response.json();
        console.log('Zalogowano pomyślnie:', data);

        //zapis tokena w localStorage 
        localStorage.setItem('token', data.token);
        window.location.href = '/dashboard'; // Przekierowanie na dashboard
      } else {
        //blad logowania
        const errorData = await response.json();
        console.error('Błąd logowania:', errorData.message);
        alert('Niepoprawne dane logowania');
      }
    } catch (error) {
      console.error('Wystąpił błąd:', error);
      alert('Wystąpił problem z logowaniem');
    }
  };

  return (
    <>
      <Navbar />
      <GreenBackground height={"50vh"} marginTop={"10vh"}>
        <Header>Logowanie</Header>
        <OrangeBackground width="70%">
          <form onSubmit={handleSubmit}>
            <table>
              <tr>
                <td className="right-align">
                  <label htmlFor="email">E-mail:</label>
                </td>
                <td className="center-align">
                  <input 
                    type="text" 
                    id="email" 
                    name="email" 
                    className='one-register-input' 
                    value={email} 
                    onChange={(e) => setEmail(e.target.value)} //Aktualizacja email w stanie
                    required 
                  />
                </td>
              </tr>
              <tr>
                <td className="right-align">
                  <label htmlFor="password">Hasło:</label>
                </td>
                <td className="center-align">
                  <input 
                    type="password" 
                    id="password" 
                    name="password" 
                    className='one-register-input' 
                    value={password} 
                    onChange={(e) => setPassword(e.target.value)} //Aktualizacja hasła w stanie
                    required 
                  />
                </td>
              </tr>
              <tr>
                <td className="right-align"></td>
                <td className="center-align">
                  <GreenButton type="submit">Zaloguj</GreenButton>
                </td>
              </tr>
              <tr>
                <td className="right-align"></td>
                <td className="center-align">
                  <a href="/reset-password" className="forgot-password">Nie pamiętam hasła</a>
                </td>
              </tr>
            </table>
          </form>
        </OrangeBackground>
      </GreenBackground>
    </>
  );
}

export default Login;

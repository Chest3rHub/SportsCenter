import React, { useState, useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import API_URL from '../appConfig';
import '../styles/auth.css';
import { SportsContext } from '../context/SportsContext';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const { dictionary, toggleLanguage } = useContext(SportsContext);

  const navigate = useNavigate();

  function handleError(textToDisplay) {
    navigate('/error', {
        state: { message: textToDisplay }
    });
}

  const handleSubmit = async (event) => {
    event.preventDefault(); 

    const loginData = {
      email: email,
      password: password
    };

    try {
      const response = await fetch(`${API_URL}/clients/login`, {

        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginData) 
      });

      if (response.ok) {
        const data = await response.json();
        console.log('Zalogowano pomyślnie:', data);

        localStorage.setItem('token', data.token);

        //////
        alert('Logowanie zakończone sukcesem!');

      } else {
        
        const errorData = await response.json();
        handleError(errorData.message);
      }
    } catch (error) {
      handleError(error);
    }
  };

  return (
    <>
      <Navbar />
      <GreenBackground height={"50vh"} marginTop={"10vh"}>
        <Header>{dictionary.loginPage.title}</Header>
        <OrangeBackground width="70%">
          <form onSubmit={handleSubmit}>
            <table>
              <tr>
                <td className="right-align">
                  <label htmlFor="email">{dictionary.loginPage.emailLabel}</label>
                </td>
                <td className="center-align">
                  <input 
                    type="text" 
                    id="email" 
                    name="email" 
                    className='one-register-input' 
                    value={email} 
                    onChange={(e) => setEmail(e.target.value)} 
                    required 
                  />
                </td>
              </tr>
              <tr>
                <td className="right-align">
                  <label htmlFor="password">{dictionary.loginPage.passwordLabel}</label>
                </td>
                <td className="center-align">
                  <input 
                    type="password" 
                    id="password" 
                    name="password" 
                    className='one-register-input' 
                    value={password} 
                    onChange={(e) => setPassword(e.target.value)} 
                    required 
                  />
                </td>
              </tr>
              <tr>
                <td className="right-align"></td>
                <td className="center-align">
                  <GreenButton type="submit">{dictionary.loginPage.signInLabel}</GreenButton>
                </td>
              </tr>
              <tr>
                <td className="right-align"></td>
                <td className="center-align">
                  <Link to="/reset-password" className="forgot-password">{dictionary.loginPage.forgotPasswordLabel}</Link>
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

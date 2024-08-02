import React from 'react';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import '../styles/auth.css';

function Login() {
  return (
    <>
    <Navbar/>
    <GreenBackground height={"50vh"} marginTop={"10vh"}>
        <Header>Logowanie</Header>
        <OrangeBackground>
        <form>
        <table>
        <tr>
            <td class="right-align">
              <label htmlFor="email">E-mail:</label>
            </td>
            <td class="center-align">
              <input type="text" id="email" name="email" className='one-register-input' required />
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="password">Hasło:</label>
            </td>
            <td class="center-align">
              <input type="text" id="password" name="password" className='one-register-input' required />
            </td>
        </tr>
        <tr>
            <td class="right-align"></td>
            <td class="center-align">
              <GreenButton type="submit" onClick={1+1} >Zaloguj</GreenButton>
            </td>
        </tr>
        <tr>
            <td class="right-align"></td>
            <td class="center-align">
              <a href="#" className="forgot-password">Nie pamiętam hasła</a>
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
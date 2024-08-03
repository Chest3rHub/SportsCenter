import React from 'react';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import '../styles/auth.css';

function Register() {
  return (
    <>
    <Navbar/>
    <GreenBackground height={"75vh"} marginTop={"5vh"}>
        <Header>Rejestracja</Header>
        <OrangeBackground width="70%">
        <form>
        <table>
        <tr>
            <td class="right-align">
              <label htmlFor="firstName">*Imię:</label>
            </td>
            <td class="center-align">
              <input type="text" id="firstName" name="firstName" className='one-register-input' required />
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="lastName">*Nazwisko:</label>
            </td>
            <td class="center-align">
              <input type="text" id="lastName" name="lastName" className='one-register-input' required />
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="address">Adres:</label>
            </td>
            <td class="center-align">
              <input type="text" id="address" name="address" className='one-register-input' />
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="birthDate">*Data urodzenia:</label>
            </td>
            <td>
              <input type="date" id="birthDate" name="birthDate" className='one-register-input' />
            </td>
        </tr>
        <tr>
            <td className="right-align">
              <label>*Płeć:</label>
            </td>
            <td>
            <div className="gender-options">
              <div>
              <input type="radio" id="genderFemale" name="gender" value="k" required />
              <label htmlFor="genderFemale">K</label>
              </div>
              <div>
              <input type="radio" id="genderMale" name="gender" value="m" required />
              <label htmlFor="genderMale">M</label>
              </div>
            </div>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="phoneNumber">*Numer telefonu:</label>
            </td>
            <td class="center-align">
              <input type="tel" id="phoneNumber" name="phoneNumber" className='one-register-input' />
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="email">*E-mail:</label>
            </td>
            <td class="center-align">
              <input type="email" id="email" name="email" className='one-register-input' required />
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="password">*Hasło:</label></td>
            <td class="center-align">
              <input type="password" id="password" name="password" className='one-register-input' required /></td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="confirmPassword">*Powtórz hasło:</label></td>
            <td class="center-align">
              <input type="password" id="confirmPassword" name="confirmPassword" className='one-register-input' required />
            </td>
        </tr>
        <tr>
            <td class="right-align"></td>
            <td class="center-align">
              <GreenButton type="submit" onClick={1+1} >Utwórz konto</GreenButton>
            </td>
        </tr>
    </table>
        </form>
        </OrangeBackground>
        
      </GreenBackground>
    </>
  );
}

export default Register;

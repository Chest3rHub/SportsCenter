import React from 'react';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import '../styles/auth.css';

function Register() {
  return (
    <GreenBackground>
        <Header>Rejestracja</Header>
        <OrangeBackground>
        <form>
          <div className="testClass">
          <div className="xlabels">
            <label htmlFor="firstName">*Imię:</label>
            <label htmlFor="lastName">*Nazwisko:</label>
            <label htmlFor="address">Adres:</label>
            <label htmlFor="birthDate">Data urodzenia:</label>
            <label>*Płeć:</label>
            <label htmlFor="phoneNumber">Numer telefonu:</label>
            <label htmlFor="email">*E-mail:</label>
            <label htmlFor="password">*Hasło:</label>
            <label htmlFor="confirmPassword">*Powtórz hasło:</label>
          </div>

          <div className="xinputs">
            <input type="text" id="firstName" name="firstName" required />
            <input type="text" id="lastName" name="lastName" required />
            <input type="text" id="address" name="address" />
            <input type="date" id="birthDate" name="birthDate" />
            <div className="gender-options">
              <input type="radio" id="genderFemale" name="gender" value="k" required />
              <label htmlFor="genderFemale">K</label>
              <input type="radio" id="genderMale" name="gender" value="m" required />
              <label htmlFor="genderMale">M</label>
            </div>
            <input type="tel" id="phoneNumber" name="phoneNumber" />
            <input type="email" id="email" name="email" required />
            <input type="password" id="password" name="password" required />
            <input type="password" id="confirmPassword" name="confirmPassword" required />
            <GreenButton type="submit" onClick={1+1} >Utwórz konto</GreenButton>
          </div>
          </div>
        
          
        </form>
        </OrangeBackground>
        
      </GreenBackground>
  );
}

export default Register;

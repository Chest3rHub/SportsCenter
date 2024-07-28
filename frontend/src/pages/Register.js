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
          <div className="register-content">
          <div className="labels">
            <div className='top-labels'>
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
            <div className='bottom-labels'>
            
            </div>
          </div>

          <div className="register-inputs">
            <div className='top-inputs'>
            <input type="text" id="firstName" name="firstName" className='one-register-input' required />
            <input type="text" id="lastName" name="lastName" className='one-register-input' required />
            <input type="text" id="address" name="address" className='one-register-input' />
            <input type="date" id="birthDate" name="birthDate" className='one-register-input' />
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
            <input type="tel" id="phoneNumber" name="phoneNumber" className='one-register-input' />
            <input type="email" id="email" name="email" className='one-register-input' required />
            <input type="password" id="password" name="password" className='one-register-input' required />
            <input type="password" id="confirmPassword" name="confirmPassword" className='one-register-input' required />
            </div>
           
            <div className='bottom-inputs'>
           
            </div>
            <GreenButton type="submit" onClick={1+1} >Utwórz konto</GreenButton>
          </div>
          </div>
        
          
        </form>
        </OrangeBackground>
        
      </GreenBackground>
  );
}

export default Register;

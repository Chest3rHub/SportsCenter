import React from 'react';
//import Header from './Header';
import '../styles/auth.css';

export default function Login() {
  return (
    <div className="auth-container">
        <Header>Logowanie</Header>
        <form className="auth-form">
            <div className="labels">
                <label htmlFor="email">*E-mail:</label>
                <label htmlFor="password">*Hasło:</label>
            </div>

            <div className="inputs">
                <input type="email" id="email" name="email" required />
                <input type="password" id="password" name="password" required />
                <button className="auth-button">Zaloguj</button>
                <a href="#" className="forgot-password">Nie pamiętam hasła</a>
            </div>
        </form>
    </div>
  );
}

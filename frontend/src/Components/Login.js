import React from 'react';
import '../Styles/Login.css';

function Login() {
  return (
    <div className="container">
        <div className="login-container">
            <div className="login-header">Logowanie</div>
            <div className="login-form">
                <div className="form-group">
                    <label htmlFor="email">*E-mail:</label>
                    <input type="email" id="email" name="email" required />
                </div>
                <div className="form-group">
                <label htmlFor="password">*Hasło:</label>
                <input type="password" id="password" name="password" required />
                </div>
                <button className="login-button">Zaloguj</button>
                <a href="#" className="forgot-password">Nie pamiętam hasła</a>
            </div>
        </div>
    </div>
  );
}

export default Login;

import React from 'react';
import '../Styles/Auth.css';

function Login() {
  return (
    <div className="container">
        <div className="auth-container">
            <div className="auth-header">Logowanie</div>
            <div className="auth-form">
                <div className="form-group">
                    <label htmlFor="email">*E-mail:</label>
                    <input type="email" id="email" name="email" required />
                </div>
                <div className="form-group">
                <label htmlFor="password">*Hasło:</label>
                <input type="password" id="password" name="password" required />
                </div>
                <button className="auth-button">Zaloguj</button>
                <a href="#" className="forgot-password">Nie pamiętam hasła</a>
            </div>
        </div>
    </div>
  );
}

export default Login;

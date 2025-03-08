import React, { useState, useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import Header from "../components/Header";
import GreenButton from "../components/GreenButton";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import Navbar from "../components/Navbar";
import "../styles/auth.css";
import { SportsContext } from "../context/SportsContext";
import loginRequest from "../api/loginRequest";
import CustomInput from "../components/CustomInput";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [emailError, setEmailError] = useState(false);
  const [passwordError, setPasswordError] = useState(false);
  const [serverError, setServerError] = useState(false);

  const { dictionary, toggleLanguage } = useContext(SportsContext);

  const navigate = useNavigate();

  function handleError(error) {
    const message = error instanceof Error ? error.message : error;
    navigate("/error", {
      state: { message },
    });
  }

  const handleSubmit = async (event) => {
    event.preventDefault();
    let valid = true;
    setServerError(false);
    if (email.trim() === "" || email.length < 4) {
      setEmailError(true);
      valid = false;
    } else {
      setEmailError(false);
    }

    if (password.trim() === "" || password.length < 5) {
      setPasswordError(true);
      valid = false;
    } else {
      setPasswordError(false);
    }

    if (!valid) return;

    const loginData = {
      email: email,
      password: password,
    };

    try {
      const response = await loginRequest(loginData);

      if (response.ok) {
        const data = await response.json();
        console.log("Zalogowano pomyślnie:", data);

        localStorage.setItem("token", data.token);
        setServerError(false);

        alert("Logowanie zakończone sukcesem!");
      } else {
        const errorData = await response.json();

        setServerError(true);
        setEmailError(true);
        setPasswordError(true);
      }
    } catch (error) {
      handleError(error);
    }
  };

  return (
    <>
    
      <GreenBackground height={"50vh"} marginTop={"10vh"}>
        <Header>{dictionary.loginPage.title}</Header>
        <OrangeBackground width="70%">
          <form onSubmit={handleSubmit}>
            <div style={{ display: "flex", flexDirection: "column", gap: "1rem" }}>
              <CustomInput
                label={dictionary.loginPage.emailLabel}
                type="email"
                id="email"
                name="email"
                fullWidth
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                error={emailError}
                helperText={emailError ? dictionary.loginPage.emailError : ""}
                required
              />
              <CustomInput
                label={dictionary.loginPage.passwordLabel}
                type="password"
                id="password"
                name="password"
                fullWidth
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                error={passwordError}
                helperText={
                  serverError ? dictionary.loginPage.incorrectLoginLabel : ""
                }
                required
              />
            <div style={{ textAlign: "center" }}>
              <GreenButton type="submit">
                {dictionary.loginPage.signInLabel}
              </GreenButton>
              <Link to="/reset-password" className="forgot-password">
                {dictionary.loginPage.forgotPasswordLabel}
              </Link>
              </div>
            </div>
          </form>
        </OrangeBackground>
      </GreenBackground>
    </>
  );
}

export default Login;

import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import OwnerSidebar from '../components/OwnerSidebar'
import API_URL from '../appConfig';
import '../styles/auth.css';
import { SportsContext } from '../context/SportsContext';
import registerRequest from '../api/registerRequest';
import CustomInput from '../components/CustomInput';
function Register() {

    const { dictionary, toggleLanguage } = useContext(SportsContext);
    
    const [formData, setFormData] = useState({
      firstName: '',
      lastName: '',
      address: '',
      birthDate: '',
      phoneNumber: '',
      email: '',
      password: '',
      confirmPassword: ''
    });

    const [firstNameError, setFirstNameError] = useState(false);
    const [lastNameError, setLastNameError] = useState(false);
    const [birthDateError, setBirthDateError] = useState(false);
    const [emailError, setEmailError] = useState(false);
    const [passwordError, setPasswordError] = useState(false);
    const [confirmPasswordError, setConfirmPasswordError] = useState(false);




    function handleError(textToDisplay) {
      navigate('/error', {
          state: { message: textToDisplay }
      });
  }
  
    const navigate = useNavigate();
  
    const handleChange = (e) => {
      const { name, value } = e.target;
      setFormData((prev) => ({
        ...prev,
        [name]: value
      }));
    };
  
    const handleSubmit = async (e) => {
      e.preventDefault();
  
      if (formData.password !== formData.confirmPassword) {
        alert("Hasła nie zgadzają się!");
        return;
      }
  
      try {
        const response = await registerRequest(formData);
  
        if (!response.ok) {
          const errorData = await response.json();
          console.log(errorData);
          handleError('Blad rejestracji... sprawdz konsole')

        } else {
          navigate('/login');
        }
        
        } catch (error) {
        console.error('Błąd rejestracji:', error);
      }
    };

  return (
    <>
    <Navbar/>
    <OwnerSidebar/>
    <GreenBackground height={"80vh"} marginTop={"2vh"}>
        <Header>{dictionary.registerPage.title}</Header>
        <OrangeBackground width="70%">
        <form onSubmit={handleSubmit}>
          <div style={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
          <CustomInput
                label={dictionary.registerPage.firstNameLabel}
                type="text"
                id="firstName"
                name="firstName"
                fullWidth
                value={formData.firstName}
                onChange={handleChange}
                error={firstNameError}
                helperText={firstNameError ? 'wpisac' : ""}
                required
                size="small"
              />
          <CustomInput
                label={dictionary.registerPage.lastNameLabel}
                type="text"
                id="lastName"
                name="lastName"
                fullWidth
                value={formData.lastName}
                onChange={handleChange}
                error={lastNameError}
                helperText={lastNameError ? 'wpisac' : ""}
                required
                size="small"
              />
          <CustomInput
                label={dictionary.registerPage.addressLabel}
                type="text"
                id="address"
                name="address"
                fullWidth
                value={formData.address}
                onChange={handleChange}
                size="small"
                
              />
          <CustomInput
                label={dictionary.registerPage.dateOfBirthLabel}
                type="date"
                id="birthDate"
                name="birthDate"
                fullWidth
                value={formData.birthDate}
                onChange={handleChange}
                error={birthDateError}
                helperText={birthDateError ? 'wpisac' : ""}
                required
                size="small"
                InputLabelProps={{
                  shrink: true
                }}
              />
          <CustomInput
                label={dictionary.registerPage.phoneNumberLabel}
                type="tel"
                id="phoneNumber"
                name="phoneNumber"
                fullWidth
                value={formData.phoneNumber}
                onChange={handleChange}
                size="small"
              />
          <CustomInput
                label={dictionary.registerPage.emailLabel}
                type="email"
                id="email"
                name="email"
                fullWidth
                value={formData.email}
                onChange={handleChange}
                error={emailError}
                helperText={emailError ? 'wpisac' : ""}
                required
                size="small"
              />
            <CustomInput
                label={dictionary.registerPage.passwordLabel}
                type="password"
                id="password"
                name="password"
                fullWidth
                value={formData.password}
                onChange={handleChange}
                error={passwordError}
                helperText={passwordError ? 'wpisac' : ""}
                required
                size="small"
              />
          <CustomInput
                label={dictionary.registerPage.confirmPasswordLabel}
                type="password"
                id="confirmPassword"
                name="confirmPassword"
                fullWidth
                value={formData.confirmPassword}
                onChange={handleChange}
                error={confirmPasswordError}
                helperText={confirmPasswordError ? 'wpisac' : ""}
                required
                size="small"
              />
              <GreenButton type="submit">{dictionary.registerPage.signUpLabel}</GreenButton>
          </div>
                </form>
        </OrangeBackground>
        
       </GreenBackground>
     </>
  );
}

export default Register;

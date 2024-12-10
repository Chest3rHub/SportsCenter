import React, { useState, useContext , useEffect} from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import OwnerSidebar from '../components/OwnerSidebar'
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

    
    const[firstNameError, setFirstNameError] = useState(false);
    const[lastNameError, setLastNameError] = useState(false);

    const[addressError, setAddressError] = useState(false);

    const[birthDateError, setBirthDateError] = useState(false);

    const[phoneNumberError, setPhoneNumberError] = useState(false);

    const[emailError, setEmailError] = useState(false);
    const[emailIsTakenError, setEmailIsTakenError] = useState(false);

    const[passwordError, setPasswordError] = useState(false);

    const[confirmPasswordError, setConfirmPasswordError] = useState(false);


    const validateForm = () => {
      let isValid = true;
  
      
      if (formData.firstName.length < 2 || formData.firstName.length > 50) {
        isValid = false;
        setFirstNameError(true);
      } else {
        setFirstNameError(false);
      }
  
      
      if (formData.lastName.length < 2 || formData.lastName.length > 50) {
        isValid = false;
        setLastNameError(true);
      } else {
        setLastNameError(false);
      }
  
      
      if (formData.address.length < 5 || formData.address.length > 100) {
        isValid = false;
        setAddressError(true);
      } else {
        setAddressError(false);
      }
  
      
      const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
      if (!emailRegex.test(formData.email)) {
        isValid = false;
        setEmailError(true);
        setEmailIsTakenError(false);
      } else {
        setEmailError(false);
        setEmailIsTakenError(false);
      }
  
      
      if (formData.password.length < 6) {
        isValid = false;
        setPasswordError(true);
      } else {
        setPasswordError(false);
      }
  
      
      if (formData.password !== formData.confirmPassword) {
        isValid = false;
        setConfirmPasswordError(true);
      } else {
        setConfirmPasswordError(false);
      }
  
      
      const birthDate = new Date(formData.birthDate);
      const age = new Date().getFullYear() - birthDate.getFullYear();
      const month = new Date().getMonth() - birthDate.getMonth();
      if (age < 18 || (age === 18 && month < 0)) {
        isValid = false;
        setBirthDateError(true);
      } else {
        setBirthDateError(false);
      }
  
      
      const phoneRegex = /^[0-9]{3}[0-9]{3}[0-9]{3}$/;
      if (!phoneRegex.test(formData.phoneNumber)) {
        isValid = false;
        setPhoneNumberError(true);
      } else {
        setPhoneNumberError(false);
      }
  
      
      return isValid;
    };



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

      if (!validateForm()) {
        return;
      }
  
  
      try {
        const response = await registerRequest(formData);
  
        if (!response.ok) {
          const errorData = await response.json();
          console.log(errorData);
          if (errorData.message && errorData.message.includes('Email:')) {
            setEmailIsTakenError(true);
          } else {
            handleError('Blad rejestracji... sprawdz konsole');
          }

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
                helperText={firstNameError ? dictionary.registerPage.firstNameError : ""}
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
                helperText={lastNameError ? dictionary.registerPage.lastNameError : ""}
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
                error={addressError}
                helperText={addressError ? dictionary.registerPage.addressError : ""}
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
                helperText={birthDateError ? dictionary.registerPage.birthDateError : ""}
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
                error={phoneNumberError}
                helperText={phoneNumberError ? dictionary.registerPage.phoneNumberError : ""}
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
                error={emailError || emailIsTakenError}
                helperText={emailIsTakenError ? dictionary.registerPage.emailTakenError : (emailError ? dictionary.registerPage.emailError : "")}

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
                helperText={passwordError ? dictionary.registerPage.passwordError : ""}
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
                helperText={confirmPasswordError ? dictionary.registerPage.confirmPasswordError : ""}
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

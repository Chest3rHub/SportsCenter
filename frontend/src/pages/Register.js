import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import Navbar from '../components/Navbar';
import OwnerSidebar from '../components/OwnerSidebar'
import API_URL from '../appConfig';
import '../styles/auth.css';

function Register() {

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
        const response = await fetch(`${API_URL}/clients`, { 
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            imie: formData.firstName,
            nazwisko: formData.lastName,
            email: formData.email,
            haslo: formData.password,
            dataUr: formData.birthDate,
            nrTel: formData.phoneNumber,
            adres: formData.address,
          }),
        });
  
        if (!response.ok) {
          const errorData = await response.json();
          return;
        }
        navigate('/login');
        } catch (error) {
        console.error('Błąd rejestracji:', error);
      }
    };

  return (
    <>
    <Navbar/>
    <OwnerSidebar/>
    <GreenBackground height={"75vh"} marginTop={"5vh"}>
        <Header>Rejestracja</Header>
        <OrangeBackground width="70%">
        <form onSubmit={handleSubmit}>
        <table>
        <tr>
            <td class="right-align">
              <label htmlFor="firstName">*Imię:</label>
            </td>
            <td class="center-align">
              <input type="text" id="firstName" name="firstName" className='one-register-input' required onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="lastName">*Nazwisko:</label>
            </td>
            <td class="center-align">
              <input type="text" id="lastName" name="lastName" className='one-register-input' required onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="address">Adres:</label>
            </td>
            <td class="center-align">
              <input type="text" id="address" name="address" className='one-register-input' onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="birthDate">*Data urodzenia:</label>
            </td>
            <td>
              <input type="date" id="birthDate" name="birthDate" className='one-register-input' onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="phoneNumber">*Numer telefonu:</label>
            </td>
            <td class="center-align">
              <input type="tel" id="phoneNumber" name="phoneNumber" className='one-register-input' onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="email">*E-mail:</label>
            </td>
            <td class="center-align">
              <input type="email" id="email" name="email" className='one-register-input' required onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="password">*Hasło:</label></td>
            <td class="center-align">
              <input type="password" id="password" name="password" className='one-register-input' required onChange={handleChange}/></td>
        </tr>
        <tr>
            <td class="right-align">
              <label htmlFor="confirmPassword">*Powtórz hasło:</label></td>
            <td class="center-align">
              <input type="password" id="confirmPassword" name="confirmPassword" className='one-register-input' required onChange={handleChange}/>
            </td>
        </tr>
        <tr>
            <td class="right-align"></td>
            <td class="center-align">
              <GreenButton type="submit">Utwórz konto</GreenButton>
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

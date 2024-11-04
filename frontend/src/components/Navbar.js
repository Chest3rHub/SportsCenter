import NavbarButton from './NavbarButton';
import '../styles/navbar.css';
import useDictionary from '../hooks/useDictionary';
import { SportsContext } from '../context/SportsContext';
import React, { useContext } from 'react';

export default function Navbar(){
    // tutaj trzeba bedzie tez dostosowac i jakos te button wyodrebnic zeby byly przekazywane jako parametr czy cos
    // poniewaz niektorzy beda mieli inny ten  navbar
    const { dictionary, toggleLanguage } = useContext(SportsContext);
    return(
    <div className="navbar">
        <NavbarButton>OFERTA KLUBU</NavbarButton>
        <NavbarButton>GRAFIK ZAJĘĆ</NavbarButton>
        <NavbarButton navigate="/login">LOGOWANIE</NavbarButton>
        <NavbarButton navigate="/register">REJESTRACJA</NavbarButton>
        <button onClick={toggleLanguage} style={{minWidth: "4vw", borderRadius: "40px",}}>{dictionary.navbar.changeLanguageLabel}</button>
      </div>);
}

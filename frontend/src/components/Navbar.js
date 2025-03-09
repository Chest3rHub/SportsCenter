import { Box, Button } from '@mui/material';
import NavbarButton from './NavbarButton';
import useDictionary from '../hooks/useDictionary';
import { SportsContext } from '../context/SportsContext';
import React, { useContext } from 'react';

export default function Navbar() {
  const { dictionary, toggleLanguage } = useContext(SportsContext);

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'space-around',
        background: 'linear-gradient(to right, #028174, #98dfc6)',
        padding: '10px',
        marginBottom: '30px',
      }}
    >
      <NavbarButton>OFERTA KLUBU</NavbarButton>
      <NavbarButton>GRAFIK ZAJĘĆ</NavbarButton>
      <NavbarButton navigate="/login">LOGOWANIE</NavbarButton>
      <NavbarButton navigate="/register">REJESTRACJA</NavbarButton>

      <button
        onClick={toggleLanguage}
        style={{ minWidth: '4vw', borderRadius: '40px' }}
      >
        {dictionary.navbar.changeLanguageLabel}
      </button>
    </Box>
  );
}

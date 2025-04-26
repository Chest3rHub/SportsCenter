import { Box } from '@mui/material';
import NavbarButton from './buttons/NavbarButton';
import { SportsContext } from '../context/SportsContext';
import React, { useContext } from 'react';

export default function Navbar({navbarItems}) {
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
      {navbarItems.map((item) => (
        <NavbarButton key={item.location} navigate={item.navigate}>
          {item.label}
        </NavbarButton>
      ))}

      <button
        onClick={toggleLanguage}
        style={{ minWidth: '4vw', borderRadius: '40px' }}
      >
        {dictionary.navbar.changeLanguageLabel}
      </button>
    </Box>
  );
}

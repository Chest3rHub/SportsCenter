import React from 'react';
import { Box } from '@mui/material';
import SidebarButton from './SidebarButton';

export default function OwnerSidebar() {
  const menuItems = [ 
    { label: 'Pracownicy', navigate: '/employees' },
    { label: 'Klienci', navigate: '/clients' },
    { label: 'Grafik', navigate: '/timetable' },
    { label: 'TODO', navigate: '/todo' },
    { label: 'Zmiana hasła', navigate: '/change-password' },
    { label: 'Zajęcia', navigate: '/trainings' },
    { label: 'Rezerwacje', navigate: '/reservations' },
    { label: 'Opinie', navigate: '/opinions' },
    { label: 'Produkty', navigate: '/products' },
    { label: 'Sprzęt', navigate: '/gear' },
    { label: 'Aktualności', navigate: '/news' }
  ];

  return (
    <Box
      sx={{
        backgroundColor: 'rgba(146, 222, 139, 0.14)',
        width: '15vw',
        marginLeft: 0,
        paddingTop: '1rem',
        boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.2)',
        position: 'absolute',
        left: 0,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        gap: '0.5rem',
        borderRadius: '10px',
        zIndex: 1,
        paddingBottom: '1rem',
      }}
    >
      {menuItems.map((item, index) => (
        <Box
          key={index}
          sx={{
            color: '#47C8A4',
            fontSize: '1.2rem',
            cursor: 'pointer',
            padding: '5px',
            textAlign: 'center',
            width: '100%',
            position: 'relative',
            textShadow: '2px 2px 4px rgba(0, 0, 0, 0.2)',
            transition: 'transform 0.3s ease, box-shadow 0.3s ease',
            '&:hover': {
              color: 'rgb(0, 255, 157)',
              transform: 'scale(1.1)',
            },
          }}
        >
          <SidebarButton navigate={item.navigate}>
            {item.label}
          </SidebarButton>
          <Box
            sx={{
              content: '""',
              display: 'block',
              width: '90%',
              height: '3px',
              backgroundColor: '#47C8A4',
              margin: '0.6rem auto',
            }}
          />
        </Box>
      ))}
    </Box>
  );
}

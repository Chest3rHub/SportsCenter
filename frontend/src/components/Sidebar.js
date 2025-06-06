import React from 'react';
import { Box } from '@mui/material';
import SidebarButton from './buttons/SidebarButton';

export default function Sidebar({ menuItems }) {

  return (
    <Box
      sx={{
        backgroundColor: 'rgba(146, 222, 139, 0.14)',
        width: '15vw',
        marginLeft: 0,
        paddingTop: '1rem',
        boxShadow: '0px 4px 10px rgba(0, 0, 0, 0.2)',
        position: 'fixed',
        left: 0,
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        gap: '0.8rem',
        borderRadius: '10px',
        zIndex: 1,
        paddingBottom: '0.3rem',
        minHeight: "80vh",
        marginTop:'1rem',
      }}
    >
      {menuItems.map((item, index) => (
        <Box
          key={index}
          sx={{
            color: '#47C8A4',
            fontSize: '1.2rem',
            cursor: 'pointer',
            
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

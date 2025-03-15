import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';
import Sidebar from '../components/Sidebar';

export default function OwnerLayout() {

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

  const {token, setToken} = useContext(SportsContext);

    useEffect(() => {
      const intervalId = setInterval(async () => {
        try {
          const response = await refreshTokenRequest(token); 
          if (response.ok) {
            const newToken = await response.json(); 
            setToken(newToken.token); 
          } else {
            console.error('Błąd podczas odświeżania tokena');
          }
        } catch (error) {
          console.error('Wystąpił błąd podczas zapytania o token:', error);
        }
      }, 30 * 60 * 1000); // 30 min
  
      return () => clearInterval(intervalId);
    }, []);

    // dostosowac navbar dla ownera 
  return (
    <Box>
        <Navbar />
        <Sidebar menuItems={menuItems} />
      <main>
        <Outlet />
      </main>
    </Box>
  );
}

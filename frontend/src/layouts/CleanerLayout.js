import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';

export default function CleanerLayout() {

  const {token, setToken, dictionary} = useContext(SportsContext);

  const navbarItems = [
    { label: dictionary.navbar.cleaner.newsLabel, navigate: '/news' },
    { label: dictionary.navbar.cleaner.timetableLabel, navigate: '/timetable' },
    { label: dictionary.navbar.cleaner.accountLabel, navigate: '/account' },
    { label: dictionary.navbar.cleaner.logoutLabel, navigate: '/logout' },
  ];

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

    // wedlug projektu pomoc sprzatajaca nie ma sidebara!! 
  return (
    <Box>
        <Navbar navbarItems={navbarItems}/>
      <main>
        <Outlet />
      </main>
    </Box>
  );
}

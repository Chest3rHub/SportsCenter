import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';
import Sidebar from '../components/Sidebar';

export default function ClientLayout() {

    const {token, setToken, dictionary, toggleLanguage} = useContext(SportsContext);

    const menuItems = [ 
        { label: dictionary.sidebar.clientSidebar.newsLabel, navigate: '/news' },
        { label: dictionary.sidebar.clientSidebar.myTimetableLabel, navigate: '/my-timetable' },
        { label: dictionary.sidebar.clientSidebar.myReservationsLabel, navigate: '/my-reservations' },
        { label: dictionary.sidebar.clientSidebar.balanceLabel, navigate: '/wallet' },
        { label: dictionary.sidebar.clientSidebar.shopLabel, navigate: '/shop' },
        { label: dictionary.sidebar.clientSidebar.accountLabel, navigate: '/account' },
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

    // dostosowac navbar dla pomocy sprzatajacej moze kazdy ma jeden taki sam z przyciskiem wyloguj np?
    // wedlug projektu pomoc sprzatajaca nie ma sidebara!! 
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

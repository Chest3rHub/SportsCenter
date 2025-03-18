import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';
import Sidebar from '../components/Sidebar';

export default function CoachLayout() {

  const {token, setToken, dictionary, toggleLanguage} = useContext(SportsContext);

    const menuItems = [ 
      { label: dictionary.sidebar.coachSidebar.newsLabel, navigate: '/news' },
      { label: dictionary.sidebar.coachSidebar.timetableLabel, navigate: '/timetable' },
      { label: dictionary.sidebar.coachSidebar.changePasswordLabel, navigate: '/change-password' },
      ];

      const navbarItems = [
        { label: dictionary.navbar.coach.newsLabel, navigate: '/news' },
        { label: dictionary.navbar.coach.timetableLabel, navigate: '/timetable' },
        { label: dictionary.navbar.coach.accountLabel, navigate: '/account' },
        { label: dictionary.navbar.coach.logoutLabel, navigate: '/logout' },
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

    // dostosowac navbar dla trenera moze kazdy ma jeden taki sam z przyciskiem wyloguj np
  return (
    <Box>
        <Navbar navbarItems={navbarItems}/>
        <Sidebar menuItems={menuItems} />
      <main>
        <Outlet />
      </main>
    </Box>
  );
}

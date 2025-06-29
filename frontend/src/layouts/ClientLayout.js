import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';
import Sidebar from '../components/Sidebar';
import getAccountInfo from '../api/getAccountInfo';

export default function ClientLayout() {

  const { token, setToken, dictionary, toggleLanguage, setRole } = useContext(SportsContext);

  const menuItems = [
    { label: dictionary.sidebar.clientSidebar.dashboardLabel, navigate: '/' },
    { label: dictionary.sidebar.clientSidebar.newsLabel, navigate: '/news' },
    { label: dictionary.sidebar.clientSidebar.myTimetableLabel, navigate: '/my-timetable' },
    { label: dictionary.sidebar.clientSidebar.myReservationsLabel, navigate: '/my-reservations' },
    //{ label: dictionary.sidebar.clientSidebar.balanceLabel, navigate: '/wallet' },
    // { label: dictionary.sidebar.clientSidebar.shopLabel, navigate: '/shop' },
    { label: dictionary.sidebar.clientSidebar.accountLabel, navigate: '/account' },
  ];

  const navbarItems = [
    { label: dictionary.navbar.client.newsLabel, navigate: '/news' },
    { label: dictionary.navbar.client.timetableLabel, navigate: '/timetable' },
    { label: dictionary.navbar.client.accountLabel, navigate: '/account' },
    { label: dictionary.navbar.client.logoutLabel, navigate: '/logout' },
  ];


  useEffect(() => {
    const intervalId = setInterval(async () => {
      try {
        const response = await refreshTokenRequest(token);
        if (response.ok) {
          const newToken = await response.json();
          setToken(newToken.token);
        } else {
      //    console.error('Błąd podczas odświeżania tokena');
        }
      } catch (error) {
      //  console.error('Wystąpił błąd podczas zapytania o token:', error);
      }
    }, 30 * 60 * 1000); // 30 min

    return () => clearInterval(intervalId);
  }, []);

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await getAccountInfo(token);
        const data = await response.json();
        if (response.ok) {
          setRole(data.role);
        } else {
          setRole('Anonim');
        }
      } catch (error) {
      //  console.error("Błąd podczas pobierania danych użytkownika:", error);
      }
    };
    fetchUserData();
  }, []);
  return (
    <Box>
      <Navbar navbarItems={navbarItems} />
      <Sidebar menuItems={menuItems} />
      <main>
        <Outlet />
      </main>
    </Box>
  );
}

import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';
import Sidebar from '../components/Sidebar';
import getAccountInfo from '../api/getAccountInfo';

export default function OwnerLayout() {

  const { token, setToken, dictionary, toggleLanguage, setRole } = useContext(SportsContext);

  const menuItems = [
    { label: dictionary.sidebar.ownerSidebar.employeesLabel, navigate: '/employees' },
    { label: dictionary.sidebar.ownerSidebar.clientsLabel, navigate: '/clients' },
    { label: dictionary.sidebar.ownerSidebar.timetableLabel, navigate: '/timetable' },
    { label: dictionary.sidebar.ownerSidebar.todoLabel, navigate: '/todo' },
    { label: dictionary.sidebar.ownerSidebar.changePasswordLabel, navigate: '/change-password' },
    { label: dictionary.sidebar.ownerSidebar.trainingsLabel, navigate: '/trainings' },
    { label: dictionary.sidebar.ownerSidebar.reservationsLabel, navigate: '/reservations' },
    { label: dictionary.sidebar.ownerSidebar.opinionsLabel, navigate: '/opinions' },
    { label: dictionary.sidebar.ownerSidebar.productsLabel, navigate: '/products' },
    // { label: dictionary.sidebar.ownerSidebar.gearLabel, navigate: '/gear' },
    { label: dictionary.sidebar.ownerSidebar.newsLabel, navigate: '/news' },
  ];

  const navbarItems = [
    { label: dictionary.navbar.owner.newsLabel, navigate: '/news' },
    { label: dictionary.navbar.owner.timetableLabel, navigate: '/timetable' },
    { label: dictionary.navbar.owner.accountLabel, navigate: '/account' },
    { label: dictionary.navbar.owner.logoutLabel, navigate: '/logout' },
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
        console.error("Błąd podczas pobierania danych użytkownika:", error);
      }
    };
    fetchUserData();
  }, []);

  // dostosowac navbar dla ownera 
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

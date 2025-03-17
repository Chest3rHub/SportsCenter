import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { Box } from '@mui/material';
import { useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";
import refreshTokenRequest from '../api/refreshTokenRequest';
import Sidebar from '../components/Sidebar';

export default function EmployeeLayout() {

  const {token, setToken, dictionary} = useContext(SportsContext);

  const menuItems = [ 
    { label: dictionary.sidebar.employeeSidebar.clientsLabel, navigate: '/clients' },
    { label: dictionary.sidebar.employeeSidebar.timetableLabel, navigate: '/timetable' },
    { label: dictionary.sidebar.employeeSidebar.todoLabel, navigate: '/todo' },
    { label: dictionary.sidebar.employeeSidebar.changePasswordLabel, navigate: '/change-password' },
    { label: dictionary.sidebar.employeeSidebar.trainingsLabel, navigate: '/trainings' },
    { label: dictionary.sidebar.employeeSidebar.reservationsLabel, navigate: '/reservations' },
    
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

    // dostosowac navbar dla pracownika adm. moze kazdy ma jeden taki sam z przyciskiem wyloguj np? 
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

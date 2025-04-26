import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { useContext, useEffect } from 'react';
import { SportsContext } from '../context/SportsContext';
import getAccountInfo from '../api/getAccountInfo';
export default function BaseLayout() {
  const { dictionary, toggleLanguage, setRole, } = useContext(SportsContext);

  const navbarItems = [
    { label: dictionary.navbar.anonymousUser.newsLabel, navigate: '/news' },
    { label: dictionary.navbar.anonymousUser.timetableLabel, navigate: '/timetable' },
    { label: dictionary.navbar.anonymousUser.registerLabel, navigate: '/register' },
    { label: dictionary.navbar.anonymousUser.loginLabel, navigate: '/login' },
  ];

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const response = await getAccountInfo();
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
  return (
    <div>
      <Navbar navbarItems={navbarItems} />
      <main>
        <Outlet />
      </main>
    </div>
  );
}

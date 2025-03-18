import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';
import { useContext } from 'react';
import { SportsContext } from '../context/SportsContext';

export default function BaseLayout() {
    const {dictionary, toggleLanguage} = useContext(SportsContext);

    const navbarItems = [
      { label: dictionary.navbar.anonymousUser.newsLabel, navigate: '/news' },
      { label: dictionary.navbar.anonymousUser.timetableLabel, navigate: '/timetable' },
      { label: dictionary.navbar.anonymousUser.registerLabel, navigate: '/register' },
      { label: dictionary.navbar.anonymousUser.loginLabel, navigate: '/login' },
    ];
  return (
    <div>
        <Navbar navbarItems={navbarItems}/>
      <main>
        <Outlet />
      </main>
    </div>
  );
}

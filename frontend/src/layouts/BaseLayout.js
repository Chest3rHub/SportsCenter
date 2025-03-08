import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';

export default function BaseLayout() {
    // dopracowac ten navbar, taki ma byc tylko dla niezalogowanych
    // dla zalogowanych inny z przyciskiem wyloguj itd
  return (
    <div>
        <Navbar />
      <main>
        <Outlet />
      </main>
    </div>
  );
}

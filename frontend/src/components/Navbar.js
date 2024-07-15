import NavbarButton from './NavbarButton';
import '../styles/navbar.css';

export default function Navbar(){
    // tutaj trzeba bedzie tez dostosowac i jakos te button wyodrebnic zeby byly przekazywane jako parametr czy cos
    // poniewaz niektorzy beda mieli inny ten  navbar
    return(
    <div className="navbar">
        <NavbarButton>OFERTA KLUBU</NavbarButton>
        <NavbarButton>GRAFIK ZAJĘĆ</NavbarButton>
        <NavbarButton>LOGOWANIE</NavbarButton>
        <NavbarButton>REJESTRACJA</NavbarButton>
      </div>);
}

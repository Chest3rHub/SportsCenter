import '../styles/navbar.css';

export default function NavbarButton(props){
    return (
    <button className="nav-button">{props.children}</button>);
}
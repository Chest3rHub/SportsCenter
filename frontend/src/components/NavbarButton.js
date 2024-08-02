import '../styles/navbar.css';
import React from 'react';
import { useNavigate } from 'react-router-dom';

export default function NavbarButton(props){
    const navigate = useNavigate();

  const handleClick = () => {
    navigate(props.navigate);
  };
    return (
    <button className="nav-button" onClick={handleClick}>{props.children}</button>);
}

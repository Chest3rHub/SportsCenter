import '../styles/sidebarButton.css';
import React from 'react';
import { useNavigate } from 'react-router-dom';

export default function SidebarButton(props){
    const navigate = useNavigate();

  const handleClick = () => {
    navigate(props.navigate);
  };
    return (
    <button className="sidebar-button" onClick={handleClick}>{props.children}</button>);
}
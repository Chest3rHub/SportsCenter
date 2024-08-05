import React from 'react';
import '../styles/sidebar.css'

export default function OwnerSidebar() {
  const menuItems = [
    'Pracownicy', 'Klienci', 'Grafik', 'TODO', 
    'Zmiana hasła', 'Zajęcia', 'Rezerwacje', 
    'Opinie', 'Produkty', 'Sprzęt', 'Aktualności'
  ];

  return (
    <div className="sidebar">
      {menuItems.map((item, index) => (
        <div key={index} className="sidebar-item">
          {item}
        </div>
      ))}
    </div>
  );
}

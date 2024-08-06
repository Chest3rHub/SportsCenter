import React from 'react';
import SidebarButton from './SidebarButton';
import '../styles/sidebar.css'

export default function OwnerSidebar() {
  const menuItems = [ 
  {
    label: 'Pracownicy',
    navigate: '/employees'
  },
  {
    label: 'Klienci',
    navigate: '/clients'
  },
  {
    label: 'Grafik',
    navigate: '/timetable'
  },
  {
    label: 'TODO',
    navigate: '/todo'
  },
  {
    label: 'Zmiana hasła',
    navigate: '/change-password'
  },
  {
    label: 'Zajęcia',
    navigate: '/trainings'
  },
  {
    label: 'Rezerwacje',
    navigate: '/reservations'
  },
  {
    label: 'Opinie',
    navigate: '/opinions'
  },
  {
    label: 'Produkty',
    navigate: '/products'
  },
  {
    label: 'Sprzęt',
    navigate: '/gear'
  },
  {
    label: 'Aktualności',
    navigate: '/news'
  }
 
  ];

  return (
    <div className="sidebar">
      {menuItems.map((item, index) => (
        <div key={index} className="sidebar-item">
          <SidebarButton navigate={item.navigate}>
            {item.label}
          </SidebarButton>
        </div>
      ))}
    </div>
  );
}

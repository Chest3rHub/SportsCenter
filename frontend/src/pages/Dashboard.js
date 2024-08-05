import React from 'react';
import Navbar from '../components/Navbar';
import Sidebar from '../components/OwnerSidebar';

export default function Dashboard() {
  return (
    <>
      <Navbar />
      <div style={{ display: 'flex', marginTop: '20px' }}>
        <Sidebar />
        <div style={{ flex: 1, padding: '20px' }}>
          <h1>panel wlasicciela</h1>
        </div>
      </div>
    </>
  );
}

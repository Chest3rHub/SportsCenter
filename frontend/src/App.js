import React from 'react';
import './App.css';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import SportsCenterRouter from './router/SportsCenterRouter';
import { SportsProvider } from './context/SportsContext';

function App() {
  return (
    <div className="App">
      <SportsProvider>
        <SportsCenterRouter/>
      </SportsProvider>
    </div>
  );
}

export default App;

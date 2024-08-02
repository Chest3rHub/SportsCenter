import React from 'react';
import './App.css';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Register from './pages/Register';
import SportsCenterRouter from './router/SportsCenterRouter';

function App() {
  return (
    <div className="App">
      <SportsCenterRouter/>
    </div>
  );
}

export default App;

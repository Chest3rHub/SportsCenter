import React from 'react';
import './App.css';
import Login from './Components/Login';
import Register from './Components/Register';

function App() {
  return (
    <div className="App">
      <div className="container">
        <div className="navbar">
          <button className="nav-button">OFERTA KLUBU</button>
          <button className="nav-button">GRAFIK ZAJĘĆ</button>
          <button className="nav-button">LOGOWANIE</button>
          <button className="nav-button">REJESTRACJA</button>
        </div>
        <Register />
      </div>
    </div>
  );
}

export default App;

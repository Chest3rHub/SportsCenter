import React from 'react';
import './App.css';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Register from './pages/Register';

function App() {
  return (
    <div className="App">
      <div className="container">
        <Navbar/>
        <Login/>
      </div>
    </div>
  );
}

export default App;

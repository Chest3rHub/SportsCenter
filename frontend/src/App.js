import React from 'react';
import './App.css';
import Navbar from './components/Navbar';
// import Login from './components/Login';
import Register from './components/Register';

function App() {
  return (
    <div className="App">
      <div className="container">
        <Navbar/>
        <Register />
      </div>
    </div>
  );
}

export default App;

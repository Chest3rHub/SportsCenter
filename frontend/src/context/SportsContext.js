import React, { createContext, useState, useEffect } from 'react';
import useDictionary from '../hooks/useDictionary'; 
import Cookies from 'js-cookie';

export const SportsContext = createContext();

export const SportsProvider = ({ children }) => {
  const [dictionary, toggleLanguage] = useDictionary();
  const [token, setToken] = useState(null);
  const [role, setRole] = useState(null);
  const [router, setRouter] = useState(null);

  useEffect(() => {
    const savedToken = Cookies.get('token'); 
    if (savedToken) {
      setToken(savedToken);
    }
  }, []);

  return (
    <SportsContext.Provider value={{ dictionary, toggleLanguage, token, setToken, router, setRouter, role, setRole }}>
      {children}
    </SportsContext.Provider>
  );
};

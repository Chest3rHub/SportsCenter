import React, { createContext } from 'react';
import useDictionary from '../hooks/useDictionary'; 

export const SportsContext = createContext();

export const SportsProvider = ({ children }) => {
  const [dictionary, toggleLanguage] = useDictionary();

  return (
    <SportsContext.Provider value={{ dictionary, toggleLanguage }}>
      {children}
    </SportsContext.Provider>
  );
};

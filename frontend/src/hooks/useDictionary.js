import { useState } from 'react';
import pl from '../dictionary/pl';
import en from '../dictionary/en';

export default function useDictionary() {
  // domyslnie polski
  const [language, setLanguage] = useState('pl');
  
  const dictionaries = { pl, en };
    const dictionary = dictionaries[language];
  
  const toggleLanguage = () => {
    setLanguage(prevLanguage => (prevLanguage === 'pl' ? 'en' : 'pl'));
  };

  return [dictionary, toggleLanguage];
}

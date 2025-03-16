import { useEffect, useState, useContext } from 'react';
import getNews from '../api/getNews';
import { Typography } from '@mui/material';
import SingleNews from '../components/SingleNews';
import GreenBackground from '../components/GreenBackground';
import Header from '../components/Header';
import { SportsContext } from '../context/SportsContext';


export default function News() {
    const { dictionary, toggleLanguage } = useContext(SportsContext);

    const [news, setNews] = useState([]);
    useEffect(() => {
        getNews()
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data); 
                setNews(data); 
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getNews:', error);
            });
    }, []);

  console.log(news);

  // max 3 newsy na stronie poki co
  const limitedNews = news.slice(0, 3);
  return (
      
    <GreenBackground gap={"4vh"}>
        <Header >{dictionary.newsPage.newsLabel}</Header>

{limitedNews.map((oneNews) => (
        <SingleNews oneNewsDetails={oneNews}/>       
      ))}
    </GreenBackground>
  );
}

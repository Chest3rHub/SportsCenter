import { useEffect, useState, useContext } from 'react';
import getNews from '../api/getNews';
import { Typography, Box, Avatar } from '@mui/material';
import SingleNews from '../components/SingleNews';
import GreenBackground from '../components/GreenBackground';
import Header from '../components/Header';
import { SportsContext } from '../context/SportsContext';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';


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
                // linijka set news ustawia na te pobrane z api, jak wykomentujecie ją to pojawi się ta smutna buzka xd
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

            {news.length === 0 && (<>
                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                    <Avatar sx={{ width: "20rem", height: "20rem" }}>
                        <SentimentDissatisfiedIcon sx={{ fontSize: "20rem", color: '#F46C63', stroke: "#ebf9ea", strokeWidth: 1.1, backgroundColor: "#ebf9ea" }} />
                    </Avatar>

                </Box><Box>
                    <Typography sx={{
                        marginTop: "-4vh",
                        fontSize: "2rem",
                        fontWeight: "bold",
                        color: "#F46C63",
                        textShadow: '0 5px 5px rgba(0, 0, 0, 0.1)',

                    }}>{dictionary.newsPage.nothingHappenedLabel}</Typography>
                    <Typography sx={{
                        fontSize: "2rem",
                        fontWeight: "bold",
                        color: "#F46C63",
                        textShadow: '0 5px 5px rgba(0, 0, 0, 0.1)',
                    }}>{dictionary.newsPage.checkAgainSoonLabel}</Typography>
                </Box>
            </>


            )}

            {limitedNews.map((oneNews) => (
                <SingleNews oneNewsDetails={oneNews} />
            ))}
        </GreenBackground>
    );
}

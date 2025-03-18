import { useEffect, useState, useContext } from 'react';
import getNews from '../api/getNews';
import { Typography, Box, Avatar, CircularProgress } from '@mui/material';
import SingleNews from '../components/SingleNews';
import GreenBackground from '../components/GreenBackground';
import Header from '../components/Header';
import { SportsContext } from '../context/SportsContext';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import NewsButton from '../components/NewsButton';
import { useNavigate } from 'react-router-dom';

export default function News() {
    const { dictionary, toggleLanguage, role } = useContext(SportsContext);

    const navigate = useNavigate();

    const [news, setNews] = useState([]);
    const [loading, setLoading] = useState(true);
    useEffect(() => {
        getNews()
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                setNews(data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getNews:', error);
            });
    }, []);

    function handleCreateNews() {
        navigate('/add-news');
    }
    const handleDeleteNews = (id) => {
        setNews((prevNews) => prevNews.filter(newsItem => newsItem.id !== id));
    };

    console.log(news);

    // max 3 newsy na stronie poki co
    const limitedNews = news.slice(0, 3);
    return (
        <GreenBackground gap={"4vh"} height={"76.5vh"}>
            <Header>{dictionary.newsPage.newsLabel}</Header>
            {(role === "Wlasciciel" || role === "Pracownik administracyjny") &&
                <Box sx={{
                    position: "fixed",
                    top: "12vh",
                    right: "3vw",
                    minWidth: "7vw"
                }}>
                    <NewsButton backgroundColor={"#8edfb4"} minWidth={"14vw"} onClick={handleCreateNews}>{dictionary.newsPage.createNewsLabel}</NewsButton>
                </Box>}

            {loading ? (
                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                    <CircularProgress sx={{ fontSize: '5rem', color: "#4caf50" }} />
                </Box>
            ) : (
                <>
                    {news.length === 0 && (
                        <>
                            <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                                <Avatar sx={{ width: "20rem", height: "20rem" }}>
                                    <SentimentDissatisfiedIcon sx={{ fontSize: "20rem", color: '#F46C63', stroke: "#ebf9ea", strokeWidth: 1.1, backgroundColor: "#ebf9ea" }} />
                                </Avatar>
                            </Box>
                            <Box>
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
                        <SingleNews key={oneNews.id} oneNewsDetails={oneNews} onNewsDeleted={handleDeleteNews} />
                    ))}
                </>
            )}
        </GreenBackground>
    );
}

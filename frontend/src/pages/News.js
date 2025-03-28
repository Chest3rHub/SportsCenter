import { useEffect, useState, useContext } from 'react';
import getNews from '../api/getNews';
import { Typography, Box, Avatar, CircularProgress } from '@mui/material';
import SingleNews from '../components/SingleNews';
import GreenBackground from '../components/GreenBackground';
import Header from '../components/Header';
import { SportsContext } from '../context/SportsContext';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import NewsButton from '../components/NewsButton';
import { useNavigate, useLocation } from 'react-router-dom';
import ChangePageButton from '../components/ChangePageButton';

export default function News() {
    const location = useLocation();
    const { dictionary, toggleLanguage, role } = useContext(SportsContext);


    // max 3 newsy na stronie poki co
    const maxNewsPerPage = 3;

    const navigate = useNavigate();

    const [news, setNews] = useState([]);
    const [loading, setLoading] = useState(true);
    const [ stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);
    //offset przekazany przy edycji lub wyswietlaniu zeby wracac na te sama strone
    const [offset, setOffset] = useState(location.state?.offsetFromLocation ? location.state.offsetFromLocation : 0);
    useEffect(() => {
        getNews(offset)
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                if (Array.isArray(data)) {
                    setNews(data);
                } else {
                    console.error('Otrzymane dane nie są tablicą:', data);
                }
                setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getNews:', error);
            });
    }, [offset, stateToTriggerUseEffectAfterDeleting]);

    function handleCreateNews(){
        navigate('/add-news');
    }
    function handleDeleteNews(id){
        setNews((prevNews) => prevNews.filter(newsItem => newsItem.id !== id));
        recalculateOffsetAfterDelete();
        setStateToTriggerUseEffectAfterDeleting((prev) => !prev);
    };

    function handleNextPage(){
        if (news.length < 3) {
            return;
        }
        setOffset(prevOffset => prevOffset + 1);
    };

    function handlePreviousPage(){
        if (offset === 0) {
            return;
        }
        setOffset(prevOffset => prevOffset - 1);
    };

    function recalculateOffsetAfterDelete(){
        // po usunieciu ostatniego newsa na stronie zeby nie bylo bledow z offsetem
        // niepotrzebne poki co bo odswiezam newsy co kazde usuniecie
        // if (news.length === 0 && offset > 0) {
        //     setOffset(prevOffset => prevOffset - 1); 
        // }
    }

   // console.log(news);

    
   // const limitedNews = news.slice(0, maxNewsPerPage);
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
                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center", minHeight: '57vh', }}>
                    <CircularProgress sx={{ fontSize: '5rem', color: "#4caf50" }} />
                </Box>
            ) : (
                <>

                    <Box sx={{
                        minHeight: '57vh',
                        display: 'flex',
                        flexDirection: 'column',
                        rowGap:'3vh',
                        marginTop:'-3vh',
                        }}>
                    {news.map((oneNews) => (
                        <SingleNews key={oneNews.id} oneNewsDetails={oneNews} onNewsDeleted={handleDeleteNews} offset={offset}/>
                    ))}
                    {news.length === 0 && (
                        <>
                            <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center", marginTop:'4vh', }}>
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
                    </Box>
                </>
            )}
            {<Box sx={{
                display: "flex",
                flexDirection: "row",
                justifyContent: 'center',
                columnGap: "4vw",
                
        
            }}>
                <ChangePageButton disabled={offset === 0} onClick={handlePreviousPage} backgroundColor={"#F46C63"} minWidth={"10vw"}>{dictionary.newsPage.previousLabel}</ChangePageButton>
                <ChangePageButton disabled={news.length<3} onClick={handleNextPage} backgroundColor={"#8edfb4"} minWidth={"10vw"}>{dictionary.newsPage.nextLabel}</ChangePageButton>
            </Box>}

        </GreenBackground>
    );
}

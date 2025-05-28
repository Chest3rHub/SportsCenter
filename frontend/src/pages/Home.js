import { useState, useEffect, useContext } from "react";
import { Box, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { SportsContext } from "../context/SportsContext";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import Header from "../components/Header";
import SingleNews from "../components/SingleNews";
import getNews from "../api/getNews";
import getCourts from "../api/getCourts";
import CircularProgress from '@mui/material/CircularProgress';

export default function Home() {
  const { dictionary } = useContext(SportsContext);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [courts, setCourts] = useState([]);
  const [courtsLoading, setCourtsLoading] = useState(true);
  const [courtsError, setCourtsError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    
    const fetchData = async () => {
      try {
        const newsResponse = await getNews(0);
        if (!newsResponse.ok) throw new Error('Failed to fetch news');
        const newsData = await newsResponse.json();
        
        if (newsData?.length > 0) {
          setLatestNews([...newsData].sort((a, b) => new Date(b.date) - new Date(a.date))[0]);
        }
        const courtsData = await getCourts();
        if (Array.isArray(courtsData)) {
          setCourts(courtsData);
        } else {
          setCourtsError('Failed to fetch courts!');
        }

      } catch (error) {
        console.error('Error fetching data!', error);
        setCourtsError(error.message);
      } finally {
        setLoading(false);
        setCourtsLoading(false);
      }
    };

     fetchData();
  }, []);

  const handleRegisterClick = () => {
    navigate('/register');
  };

  return (
    <>
      <GreenBackground height={"auto"} marginTop={"5vh"} gap={"2vh"}>
        <Header>{dictionary.homePage.titleLabel}</Header>
        <Box sx={{
          display: 'flex',
          flexDirection: { xs: 'column', md: 'row' },
          width: '90%',
          margin: '0 auto',
          gap: '4vw',
          minHeight: '40vh'
        }}>
          <Box sx={{
            width: { xs: '100%', md: '50%' },
            display: 'flex',
            flexDirection: 'column'
          }}>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '1.5rem',
              marginBottom: '2vh',
              textAlign: 'left'
            }}>
              {dictionary.homePage.whatIsNewLabel}
            </Typography>

            <Box sx={{ marginLeft: '-30px' }}>
              {loading ?(
                <Box sx={{ 
                  height: '140px', 
                  display: 'grid', 
                  placeItems: 'center' 
                }}>
                  <CircularProgress sx={{ color: '#4caf50', fontSize: '3.5rem' }} />
                </Box>
              ) : latestNews ? (
                <SingleNews 
                  oneNewsDetails={latestNews} 
                  onNewsDeleted={()=>{}} 
                />
              ) : (
                <Typography sx={{ 
                  color: 'black', 
                  fontSize: '1rem',
                  textAlign: 'center'
                }}>
                  {dictionary.empMainPage.noNewsLabel}
                </Typography>
              )}
            </Box>

            <Typography sx={{ 
              color: 'black', 
              fontSize: '1.25rem',
              fontWeight: 'bold',
              textAlign: 'left',
              marginTop: '4vh',
            }}>
              {dictionary.homePage.firstCommercial} 🎾🥇
            </Typography>
          </Box>

          <Box sx={{ 
            width: { xs: '100%', md: '50%' },
            display: 'flex',
            flexDirection: 'column'
          }}>
            <Typography 
              sx={{ 
                color: 'black', 
                fontSize: '1.5rem',
                fontWeight: 'bold',
                textAlign: 'left',
                marginBottom: '2vh'
              }}
            >
              <span 
                onClick={handleRegisterClick}
                style={{ 
                  cursor: 'pointer',
                  textDecoration: 'underline',
                  color: '#0AB68B'
                }}
              >
                {dictionary.homePage.registerLabel}
              </span>
               {dictionary.homePage.secondCommercial}
            </Typography>

            <Box sx={{ marginTop: '2vh' }}>
              <Typography sx={{ 
                color: 'black', 
                fontWeight: 'bold', 
                fontSize: '1.5rem',
                marginBottom: '1vh',
                textAlign: 'left'
              }}>
                {dictionary.homePage.ourCourtsLabel}
              </Typography>
              <OrangeBackground 
                width="92%" 
                height="20vh" 
                maxHeight="20vh" 
                overflow="auto"
                minHeight="20vh"
              >
                {courtsLoading ? (
                  <Box sx={{ height: '100%', display: 'grid', placeItems: 'center' }}>
                    <CircularProgress sx={{ color: '#4caf50' }} />
                  </Box>
                ) : courtsError ? (
                  <Typography sx={{ 
                    color: 'black', 
                    height: '100%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    fontSize: '1.1rem'
                  }}>
                    Error while fetching courts: {courtsError}
                  </Typography>
                ) : courts.length > 0 ? (
                  <Box sx={{ 
                    display: 'flex', 
                    flexDirection: 'column',
                    gap: '1vh',
                    padding: '0 16px'
                  }}>
                    {courts.map((court) => (
                      <Typography 
                        key={court.id}
                        sx={{ 
                          color: 'black', 
                          fontSize: '1.2rem',
                          textAlign: 'left',
                          padding: '0.5vh 0'
                        }}
                      >
                        • {court.name}
                      </Typography>
                    ))}
                  </Box>
                ) : (
                  <Typography sx={{ 
                    color: 'black', 
                    height: '100%',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    fontSize: '1.1rem'
                  }}>
                    {dictionary.homePage.noCourtsLabel}
                  </Typography>
                )}
              </OrangeBackground>
            </Box>
          </Box>
        </Box>
      </GreenBackground>
    </>
  );
}
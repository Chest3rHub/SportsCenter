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
import GreenButton from "../components/buttons/GreenButton";
import { keyframes } from "@mui/material";

export default function Home() {
  const { dictionary } = useContext(SportsContext);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [courts, setCourts] = useState([]);
  const [courtsLoading, setCourtsLoading] = useState(true);
  const [courtsError, setCourtsError] = useState(null);
  const navigate = useNavigate();

  const pulse = keyframes`
  0% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.25);
  }
  100% {
    transform: scale(1);
  }
`;

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
        <Box
          sx={{
            width: '100%',             // pełna szerokość kontenera
            display: 'flex',
            justifyContent: 'center',  // wyśrodkowanie poziome
            minHeight: '5vh',
          }}
        ><Header>{dictionary.homePage.titleLabel}</Header>

        </Box>

        <Box sx={{
          display: 'flex',
          flexDirection: 'row',
          columnGap: '3vw',
          //  justifyContent:'space-between',

        }}>
          <Box sx={{
            // box z co nowego
            width: '50%',
            display: 'flex',
            flexDirection: 'column',
            rowGap: '2vh',
          }}>
            <Typography sx={{
              color: 'black',
              fontWeight: 'bold',
              fontSize: '1.5rem',

              textAlign: 'left',
              paddingLeft: '2vw'
            }}>
              {dictionary.homePage.whatIsNewLabel}
            </Typography>
            {loading ? (
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
                onNewsDeleted={() => { }}
                height={'16vh'}
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
          <Box sx={{
            // box z kortami
            width: '50%',
            display: 'flex',
            flexDirection: 'column',
            rowGap: '2vh',
          }}>
            <Typography sx={{
              color: 'black',
              fontWeight: 'bold',
              fontSize: '1.5rem',
              marginLeft: '1.5vw',
              textAlign: 'left'
            }}>
              {dictionary.homePage.ourCourtsLabel}
            </Typography>

            {courtsLoading ? (
              <Box sx={{ height: '100%', display: 'grid', placeItems: 'center' }}>
                <CircularProgress sx={{ color: '#4caf50' }} />
              </Box>
            ) : courts.length > 0 ? (
              <OrangeBackground
                width={"80%"}
                height="16vh"
                maxHeight="16vh"
                overflow="hidden"
                minHeight="16vh"
              >
                <Box sx={{
                  display: 'flex',
                  flexDirection: 'column',
                  gap: '1vh',

                }}>
                  {courts.map((court) => (
                    <Typography
                      key={court.id}
                      sx={{
                        color: 'black',
                        fontSize: '1.2rem',
                        textAlign: 'left',

                      }}
                    >
                      • {court.name}
                    </Typography>
                  ))}
                </Box>
              </OrangeBackground>
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
          </Box>
        </Box>
        <Typography sx={{
          color: 'black',
          fontWeight: 'bold',
          fontSize: '2rem',
          textAlign: 'center',
          marginTop: '2vh'
        }}>
          {dictionary.homePage.joinNowLabel}
        </Typography>
        <Box
          sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            marginTop: '1vh',
            marginBottom: '1vh',
          }}

        >
          <Box
            sx={{
              animation: `${pulse} 3.5s infinite`,
              display: 'inline-block'
            }}
          >
            <GreenButton style={{ paddingLeft: '2vw', paddingRight: '2vw', minWidth:'14vw' }} onClick={handleRegisterClick}>{dictionary.homePage.registerLabel}</GreenButton>
          </Box>
        </Box>
      </GreenBackground>
    </>
  );
}
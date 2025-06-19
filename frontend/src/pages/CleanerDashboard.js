import { useContext, useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import Header from "../components/Header";
import SingleNews from "../components/SingleNews";
import getNews from "../api/getNews";
import { SportsContext } from "../context/SportsContext";
import CircularProgress from '@mui/material/CircularProgress';
import getClubWorkingHours from "../api/getClubWorkingHours";
import { useNavigate } from 'react-router-dom';

export default function CleanerDashboard() {
  const { dictionary } = useContext(SportsContext);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [openingHours, setOpeningHours] = useState([]);
  const [hoursLoading, setHoursLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchNews = async () => {
      try {
        const newsResponse = await getNews(0);
        if (!newsResponse.ok) throw new Error('Failed to fetch news');
        const newsData = await newsResponse.json();
        
        if (newsData?.length > 0) {
          setLatestNews([...newsData].sort((a, b) => new Date(b.date) - new Date(a.date))[0]);
        }
      } catch (error) {
      //  console.error('Error fetching news:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchNews();
  }, []);

  useEffect(() => {
    const fetchWorkingHours = async () => {
      try {
        const hoursResponse = await getClubWorkingHours(0);
        if (!hoursResponse.ok) throw new Error('Failed to fetch working hours');

        const hoursData = await hoursResponse.json();
        
        if (Array.isArray(hoursData)) {
          setOpeningHours(hoursData);
        } else {
          setOpeningHours([]);
        }
      } catch (error) {
      //  console.error('Error fetching working hours:', error);
        setOpeningHours([]);
      } finally {
        setHoursLoading(false);
      }
    };

    fetchWorkingHours();
  }, []);

  const getDayName = (dayOfWeek) => {
    const dayNames = {
      'poniedzialek': dictionary.days.monday,
      'wtorek': dictionary.days.tuesday,
      'sroda': dictionary.days.wednesday,
      'czwartek': dictionary.days.thursday,
      'piatek': dictionary.days.friday,
      'sobota': dictionary.days.saturday,
      'niedziela': dictionary.days.sunday
    };
    return dayNames[dayOfWeek] || dayOfWeek;
  };

  const formatTime = (timeString) => {
    if (!timeString) return '';
    return timeString.substring(0, 5);
  };

  return (
    <Box sx={{
      maxWidth: '1300px',
      margin: '0 auto',
      padding: '24px',
      height: '85vh',
      display: 'flex',
      flexDirection: 'column',
      alignItems: 'center',
      width: '100%'
    }}>
      <Box sx={{ 
        width: '100%',
      }}>
        <Header>{dictionary.empMainPage.title}</Header>
      </Box>
  
      <GreenBackground>
        <Box sx={{ width: '100%' }}>
          <Typography sx={{ 
            color: 'black', 
            fontWeight: 'bold', 
            fontSize: '2rem',
            marginBottom: '3vh',
            marginTop: '-3vh',
            textAlign: 'center'
          }}>
            {dictionary.empMainPage.whatIsNewLabel}
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
            <Box sx={{ 
              width: '90%',
              margin: '0 auto',
              maxWidth: '800px'
            }}>
              <SingleNews 
                oneNewsDetails={latestNews} 
                onNewsDeleted={()=>{}} 
              />
            </Box>
          ) : (
            <Typography sx={{ 
              color: 'black', 
              fontSize: '1.1rem',
              textAlign: 'center'
            }}>
              {dictionary.empMainPage.noNewsLabel}
            </Typography>
          )}
        </Box>

        <Box sx={{ width: '100%',  
          // godziny klubu
          marginTop:'3vh'
        }}>
          <Typography sx={{ 
                color: 'black', 
                fontWeight: 'bold', 
                fontSize: '2rem',
              }}>
                {dictionary.ownerMainPage.openingHoursLabel}
              </Typography>
              <Typography sx={{ 
                color: 'black', 
                fontSize: '1.2rem',
                fontStyle: 'italic',
                marginBottom:'2vh'
              }}>
                {dictionary.ownerMainPage.thisWeekLabel}
              </Typography>
    {hoursLoading ? (
      <Box sx={{ display: 'grid', placeItems: 'center', flex: 1 }}>
        <CircularProgress sx={{ color: '#4caf50' }} />
      </Box>
    ) : openingHours.length > 0 ? (
      <>
        <OrangeBackground width={'90%'}>
          <Box sx={{
            display: 'grid',
            gridTemplateColumns: 'repeat(7, 1fr)',
            gap: '6px',
            textAlign: 'center',
          }}>
            {openingHours.map((day, index) => (
              <Typography key={`day-${index}`} sx={{ fontWeight: 'bold', color: 'black', fontSize: '0.9rem' }}>
                {getDayName(day.dayOfWeek)}
              </Typography>
            ))}
          </Box>

          <Box sx={{
            display: 'grid',
            gridTemplateColumns: 'repeat(7, 1fr)',
            gap: '8px',
            textAlign: 'center',
            marginTop: '12px'
          }}>
            {openingHours.map((day, index) => (
              <Box key={`hour-${index}`} sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
                <Typography sx={{ color: 'black', fontSize: '1rem' }}>
                  {formatTime(day.openHour)}
                </Typography>
                <Typography sx={{ color: 'black', fontSize: '1rem' }}>
                  -
                </Typography>
                <Typography sx={{ color: 'black', fontSize: '1rem' }}>
                  {formatTime(day.closeHour)}
                </Typography>
              </Box>
            ))}
          </Box>
        </OrangeBackground>
      </>
    ) : (
      <>
        <OrangeBackground>
          <Typography sx={{
            color: 'black',
            textAlign: 'center',
            fontSize: '1.1rem'
          }}>
            {dictionary.ownerMainPage.noHoursLabel}
          </Typography>
        </OrangeBackground>
      </>

    )}
  </Box>
      </GreenBackground>
    </Box>
  );
}
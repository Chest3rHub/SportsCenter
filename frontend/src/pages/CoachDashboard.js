import { useContext, useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import Header from "../components/Header";
import SingleNews from "../components/SingleNews";
import getNews from "../api/getNews";
import { SportsContext } from "../context/SportsContext";
import CircularProgress from '@mui/material/CircularProgress';
import getTrainerActivities from "../api/getTrainerActivities";
import { useNavigate } from 'react-router-dom';

export default function CoachDashboard() {
  const { dictionary } = useContext(SportsContext);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [activities, setActivities] = useState([]);
  const [activitiesLoading, setActivitiesLoading] = useState(true);
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

        const activitiesResponse = await getTrainerActivities(0);
        if (!activitiesResponse.ok) throw new Error('Failed to fetch activities');
        const activitiesData = await activitiesResponse.json();
        
        if (Array.isArray(activitiesData)) {
          const sortedActivities = activitiesData.sort((a, b) => {
            const dateA = new Date(`${a.dateOfActivity}T${a.startTime}`);
            const dateB = new Date(`${b.dateOfActivity}T${b.startTime}`);
            return dateA - dateB; });
          
          const now = new Date();
          const upcomingActivities = sortedActivities.filter(activity => {
            const activityStart = new Date(`${activity.dateOfActivity}T${activity.startTime}`);
            return activityStart > now;
          });
          
          setActivities(upcomingActivities);
        } else {
          setActivities([]);
        }

      } catch (error) {
       // console.error('Error fetching data:', error);
        setActivities([]);
      } finally {
        setLoading(false);
        setActivitiesLoading(false);
      }
    };

    fetchData();
  }, []);

  const formatActivityTime = (dateString, startTime, endTime) => {
    if (!dateString || !startTime || !endTime) return 'No data avaible';
    const formattedDate = dateString;
    const formattedStartTime = startTime.substring(0, 5);
    const formattedEndTime = endTime.substring(0, 5);
    return `${formattedDate}, ${formattedStartTime} - ${formattedEndTime}`;
  };

  return (
    <Box sx={{
      maxWidth: '1100px',
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
        marginBottom: '16px',
        textAlign: 'center'
      }}>
        <Header>{dictionary.coachMainPage.titleLabel}</Header>
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
            {dictionary.coachMainPage.whatIsNewLabel}
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
              {dictionary.coachMainPage.noNewsLabel}
            </Typography>
          )}
        </Box>

        <Box sx={{ 
          width: '100%',
          marginTop: '24px'
        }}>
          <Typography sx={{ 
            color: 'black', 
            fontWeight: 'bold', 
            fontSize: '2rem',
            marginBottom: '3vh',
            textAlign: 'center'
          }}>
            {dictionary.coachMainPage.upcomingEventsLabel}
          </Typography>
  
          <Box sx={{ 
            width: '90%',
            margin: '0 auto',
            maxWidth: '800px'
          }}>
            <OrangeBackground sx={{
              width: '100%',
              minHeight: '20vh',
              padding: '16px',
              position: 'relative',
            }}>
              {activitiesLoading ? (
                <Box sx={{ height: '100%', display: 'grid', placeItems: 'center' }}>
                  <CircularProgress sx={{ color: '#4caf50' }} />
                </Box>
              ) : activities.length > 0 ? (
                  <Box sx={{ 
                  display: 'flex', 
                  flexDirection: 'column', 
                  gap: '16px',
                  textAlign: 'left',
                  padding: '8px'
                }}>
                  {activities[0].type === "Rezerwacja" ? (
                    <>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.typeLabel} <span style={{fontWeight: 'bold'}}>{dictionary.coachMainPage.reservationLabel}</span>
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.dateLabel} <span style={{fontWeight: 'bold'}}>{formatActivityTime(
                          activities[0].dateOfActivity, 
                          activities[0].startTime, 
                          activities[0].endTime
                        )}</span>
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.courtLabel} <span style={{fontWeight: 'bold'}}>{activities[0].courtName}</span>
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.clientLabel} <span style={{fontWeight: 'bold'}}>{activities[0].clientName} {activities[0].clientSurname}</span>
                      </Typography>
                    </>
                  ) : (
                    <>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.typeLabel} <span style={{fontWeight: 'bold'}}>{dictionary.coachMainPage.sportActivityLabel}{activities[0].sportActivityName}</span>
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.dateLabel} <span style={{fontWeight: 'bold'}}>{formatActivityTime(
                          activities[0].dateOfActivity, 
                          activities[0].startTime, 
                          activities[0].endTime
                        )}</span>
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.courtLabel} <span style={{fontWeight: 'bold'}}>{activities[0].courtName}</span>
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.coachMainPage.levelLabel} <span style={{fontWeight: 'bold'}}>{activities[0].levelName}</span>
                      </Typography>
                    </>
                  )}
                </Box>
              ) : (
                <Typography sx={{ 
                  color: 'black', 
                  height: '100%',
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  fontSize: '1.1rem',
                  fontWeight: 'bold'
                }}>
                  {dictionary.coachMainPage.noUpcomingEventsLabel}
                </Typography>
              )}
            </OrangeBackground>
          </Box>
        </Box>
      </GreenBackground>
    </Box>
  );
}

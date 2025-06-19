import { useContext, useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import Header from "../components/Header";
import SingleNews from "../components/SingleNews";
import getNews from "../api/getNews";
import { SportsContext } from "../context/SportsContext";
import CircularProgress from '@mui/material/CircularProgress';
import getYourTasks from "../api/getYourTasks";
import getClubWorkingHours from '../api/getClubWorkingHours';
import GreenButton from "../components/buttons/GreenButton";
import { useNavigate } from 'react-router-dom';

export default function OwnerDashboard() {
  const { dictionary } = useContext(SportsContext);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [tasks, setTasks] = useState([]);
  const [tasksLoading, setTasksLoading] = useState(true);
  const [workingHours, setWorkingHours] = useState([]);
  const [hoursLoading, setHoursLoading] = useState(true);
  const navigate = useNavigate();


  useEffect(() => {
    const fetchNews = async () => {
      try {
        const newsResponse = await getNews(0);
        if (!newsResponse.ok) throw new Error("Failed to fetch news");

        const newsData = await newsResponse.json();

        if (newsData?.length > 0) {
          setLatestNews([...newsData].sort((a, b) => new Date(b.date) - new Date(a.date))[0]);
        }
      } catch (error) {
       // console.error("Error fetching news:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchNews();
  }, []);

  useEffect(() => {
    const fetchTasks = async () => {
      try {
        const tasksResponse = await getYourTasks(0);
        if (tasksResponse.status === 404) {
          setTasks([]);
          return;
        }
        if (!tasksResponse.ok) throw new Error("Failed to fetch tasks");

        const tasksData = await tasksResponse.json();

        if (Array.isArray(tasksData)) {
          const sortedTasks = tasksData.sort((a, b) => new Date(a.dateTo) - new Date(b.dateTo));
          setTasks(sortedTasks);
        } else {
          setTasks([]);
        }
      } catch (error) {
      //  console.error("Error fetching tasks:", error);
        setTasks([]);
      } finally {
        setTasksLoading(false);
      }
    };

    fetchTasks();
  }, []);

  useEffect(() => {
    const fetchWorkingHours = async () => {
      try {
        const hoursResponse = await getClubWorkingHours(0);
        if (!hoursResponse.ok) throw new Error("Failed to fetch working hours");

        const hoursData = await hoursResponse.json();

        if (Array.isArray(hoursData)) {
          setWorkingHours(hoursData);
        } else {
          setWorkingHours([]);
        }
      } catch (error) {
      //error("Error fetching working hours:", error);
        setWorkingHours([]);
      } finally {
        setHoursLoading(false);
      }
    };

    fetchWorkingHours();
  }, []);


  const formatDeadline = (dateString) => {
    if (!dateString) return dictionary.empMainPage.noDateLabel || 'No date';
    const date = new Date(dateString);
    return `${date.getDate().toString().padStart(2, '0')}.${(date.getMonth()+1).toString().padStart(2, '0')}.${date.getFullYear()}`;
  };

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

  const handleTodoListClick = () => {
    navigate('/todo');
  };

  const handleModifyHoursClick = () => {
    navigate('/working-hours');
  };

  return (<>
    <GreenBackground height={"55vh"} marginTop={"2vh"}>
      <Header>{dictionary.empMainPage.title}</Header>

      <Box sx={{
        display:'flex',
        flexDirection:'row',
        maxHeight:'32vh',
      }}>
        <Box sx={{
          // pierwszy box ten z newsami
          width:'50%'
        }}>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '24px',
              paddingLeft: '24px',
              textAlign: 'left'
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
                height:'20vh'
              }}>
                <SingleNews 
                  oneNewsDetails={latestNews} 
                  onNewsDeleted={()=>{}} 
                  height={'15.7vh'}
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

          <Box sx={{ 
            // drugi box ten z to do 
            width:'50%'
          }}>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '24px',
              paddingLeft: '24px',
              textAlign: 'left'
            }}>
              {dictionary.empMainPage.todoLabel}
            </Typography>
  
            <Box sx={{ 
              width: '90%',
              margin: '0 auto'
            }}>
              <OrangeBackground>
                {tasksLoading ? (
                  <Box sx={{ height: '100%', display: 'grid', placeItems: 'center' }}>
                    <CircularProgress sx={{ color: '#4caf50' }} />
                  </Box>
                ) : tasks.length > 0 ? (
                  <Box sx={{ display: 'flex', flexDirection: 'column', rowGap:'0.7vh' }}>
                    <Box sx={{ 
                      display: 'flex', 
                      justifyContent: 'space-between',
                      
                    }}>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.empMainPage.contentLabel}
                      </Typography>
                      <Typography sx={{ color: 'black', fontSize: '1.1rem', fontWeight: 'bold' }}>
                        {dictionary.empMainPage.deadlineLabel}
                      </Typography>
                    </Box>

                    {tasks.slice(0, 3).map((task, index) => (
                      <Box 
                        key={index}
                        sx={{ 
                          display: 'flex',
                          justifyContent: 'space-between',
                          alignItems: 'center',
                          pb: '4px',
                          px: '8px'
                        }}
                      >
                        <Typography sx={{ 
                          flex: 2, 
                          textAlign: 'left',
                          overflow: 'hidden',
                          textOverflow: 'ellipsis',
                          whiteSpace: 'nowrap',
                        }}>
                          ● {task.description.length > 32 ? `${task.description.substring(0, 29)}...` : task.description}
                        </Typography>
                        <Typography sx={{ flex: 1, textAlign: 'right' }}>
                          {formatDeadline(task.dateTo)}
                        </Typography>
                      </Box>
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
                    {dictionary.empMainPage.noTasksLabel}
                  </Typography>
                )}
              </OrangeBackground>
            </Box>
          </Box>

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
  ) : workingHours.length > 0 ? (
    <>
      <OrangeBackground width={'90%'}>
        <Box sx={{
          display: 'grid',
          gridTemplateColumns: 'repeat(7, 1fr)',
          gap: '6px',
          textAlign: 'center',
        }}>
          {workingHours.map((day, index) => (
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
          {workingHours.map((day, index) => (
            <Typography key={`hour-${index}`} sx={{ color: 'black', fontSize: '1rem' }}>
              {formatTime(day.openHour)} - {formatTime(day.closeHour)}
            </Typography>
          ))}
        </Box>
      </OrangeBackground>

      <Box sx={{ marginTop: '20px', display: 'flex', justifyContent: 'center' }}>
        <GreenButton 
          onClick={handleModifyHoursClick}
          style={{ width: 'auto', paddingX: '24px' }}
        >
          {dictionary.ownerMainPage.modifyHoursButtonLabel}
        </GreenButton>
      </Box>
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

      <Box sx={{ marginTop: '20px', display: 'flex', justifyContent: 'center' }}>
        <GreenButton 
          onClick={handleModifyHoursClick}
          style={{ width: 'auto', paddingX: '24px' }}
        >
          {dictionary.ownerMainPage.modifyHoursButtonLabel}
        </GreenButton>
      </Box>
    </>
  )}
</Box>

    </GreenBackground>
  </>
  );
}
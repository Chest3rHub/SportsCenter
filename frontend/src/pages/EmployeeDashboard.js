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
import getOrdersToProcess from "../api/getOrdersToProcess";
import { useNavigate } from 'react-router-dom';

export default function EmployeeDashboard() {
  const { dictionary } = useContext(SportsContext);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [tasks, setTasks] = useState([]);
  const [tasksLoading, setTasksLoading] = useState(true);
  const [orders, setOrders] = useState([]);
  const [ordersLoading, setOrdersLoading] = useState(true);
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

        const tasksResponse = await getYourTasks(0);
        if (!tasksResponse.ok) throw new Error('Failed to fetch tasks');
        const tasksData = await tasksResponse.json();
        
        if (Array.isArray(tasksData)) {
          const sortedTasks = tasksData.sort((a, b) => new Date(a.dateTo) - new Date(b.dateTo));
          setTasks(sortedTasks);
        } else {
          setTasks([]);
        }

        const ordersResponse = await getOrdersToProcess();
        if (!ordersResponse.ok) throw new Error('Failed to fetch orders');
        const ordersData = await ordersResponse.json();
        
        if (Array.isArray(ordersData)) {
          const sortedOrders = [...ordersData]
            .sort((a, b) => new Date(a.orderDate) - new Date(b.orderDate))
            .slice(0, 3);
          setOrders(sortedOrders);
        } else {
          setOrders([]);
        }

      } catch (error) {
      //  console.error('Error fetching data:', error);
        setTasks([]);
        setOrders([]);
      } finally {
        setLoading(false);
        setTasksLoading(false);
        setOrdersLoading(false);
      }
    };

    fetchData();
  }, []);

  const formatDeadline = (dateString) => {
    if (!dateString) return dictionary.empMainPage.noDateLabel || 'No date';
    const date = new Date(dateString);
    return `${date.getDate().toString().padStart(2, '0')}.${(date.getMonth()+1).toString().padStart(2, '0')}.${date.getFullYear()}`;
  };

  const formatOrderDate = (dateString) => {
    if (!dateString) return dictionary.empMainPage.noDateLabel || 'No date';
    const date = new Date(dateString);
    return `${date.getFullYear()}-${(date.getMonth()+1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
  };

  const handleTodoListClick = () => {
    navigate('/todo');
  };

  const handleMoreOrdersClick = () => {
    alert("Nie zaimplementowano.");
  };

  return (
    <GreenBackground height={"55vh"} marginTop={"12vh"} gap={"2vh"}>
      <Box sx={{
            width: '100%',             // pełna szerokość kontenera
            display: 'flex',
            justifyContent: 'center',  // wyśrodkowanie poziome
            minHeight: '5vh',
            marginBottom:'5vh',
          }}>
        <Header>{dictionary.empMainPage.title}</Header>
      </Box>
  
      <Box sx={{
        display: 'flex',    
        height: 'calc(100% - 60px)',
        gap: '50px'
      }}>
        
          <Box>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '24px',
              paddingLeft: '38px',
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
                width: '90%',
                margin: '0 auto',
                maxWidth:'30vw'
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

          <Box sx={{ 
            flex: 1,
            
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
              <OrangeBackground sx={{
                width: '100%',
                minHeight: '20vh',
                padding: '16px',
                position: 'relative'
              }} height={'12vh'}>
                {tasksLoading ? (
                  <Box sx={{ height: '90%', display: 'grid', placeItems: 'center' }}>
                    <CircularProgress sx={{ color: '#4caf50' }} />
                  </Box>
                ) : tasks.length > 0 ? (
                  <Box sx={{ display: 'flex', flexDirection: 'column', gap: '6px' }}>
                    <Box sx={{ 
                      display: 'flex', 
                      justifyContent: 'space-between',
                      mb: '6px',
                      px: '6px'
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
        

        <Box sx={{
          display:'none'
        }}>
        
          <Typography sx={{ 
            color: 'black', 
            fontWeight: 'bold', 
            fontSize: '2rem',
            marginBottom: '16px',
            textAlign: 'center'
          }}>
            {dictionary.empMainPage.ordersLabel}
          </Typography>

          {ordersLoading ? (
            <Box sx={{ display: 'grid', placeItems: 'center', flex: 1 }}>
              <CircularProgress sx={{ color: '#4caf50' }} />
            </Box>
          ) : orders.length > 0 ? (
            <>
              <Box sx={{
                display: 'flex',
                flexDirection: 'column',
                gap: '24px',
                overflowY: 'auto',
                flex: 1,
                padding: '0 4px'
              }}>
                {orders.map((order, index) => (
                  <OrangeBackground key={index} sx={{ 
                    padding: '12px 16px',
                    borderRadius: '8px',
                    width: '100%',
                    minWidth: '300px'
                  }}>
                    <Box sx={{ 
                      display: 'flex', 
                      flexDirection: 'column', 
                      gap: '6px',
                      textAlign: 'left'
                    }}>
                      <Box sx={{ 
                        display: 'flex', 
                        justifyContent: 'space-between',
                        alignItems: 'center'
                      }}>
                        <Typography sx={{ color: 'black', fontWeight: 'bold' }}>
                          {dictionary.empMainPage.orderIdLabel} {order.orderId}
                        </Typography>
                        <Typography sx={{ color: 'black', fontWeight: 'bold' }}>
                          {dictionary.empMainPage.dateLabel}: {formatOrderDate(order.orderDate)}
                        </Typography>
                      </Box>

                      <Typography sx={{ color: 'black' }}>
                        <Box component="span" sx={{ fontWeight: 'bold' }}>{dictionary.empMainPage.clientLabel}:</Box> {order.clientFirstName} {order.clientLastName}
                      </Typography>

                      <Typography sx={{ color: 'black' }}>
                        <Box component="span" sx={{ fontWeight: 'bold' }}>{dictionary.empMainPage.productLabel}:</Box> {order.productName} x{order.quantity}
                      </Typography>
                    </Box>
                  </OrangeBackground>
                ))}
              </Box>

              {orders.length > 2 && (
                <Typography
                  sx={{
                    color: '#0AB68B',
                    textAlign: 'center',
                    cursor: 'pointer',
                    textDecoration: 'underline',
                    '&:hover': { opacity: 0.8 },
                    marginTop: '24px'
                  }}
                  onClick={handleMoreOrdersClick}
                >
                  {dictionary.empMainPage.moreOrdersLabel}
                </Typography>
              )}
            </>
          ) : (
            <Typography sx={{ 
              color: 'black', 
              textAlign: 'center',
              flex: 1,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              fontSize: '1.1rem'
            }}>
              {dictionary.empMainPage.noOrdersLabel}
            </Typography>
          )}
        
        </Box>
      </Box>
      </GreenBackground>
  );
}
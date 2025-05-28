import { useContext, useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import GreenButton from '../components/buttons/GreenButton';
import Header from "../components/Header";
import SingleNews from "../components/SingleNews";
import getNews from "../api/getNews";
import { SportsContext } from "../context/SportsContext";
import CircularProgress from '@mui/material/CircularProgress';
import getAccountInfo from "../api/getAccountInfo";
import getYourUpcomingActivities from "../api/getYourUpcomingActivities";

export default function ClientDashboard() {
  const { dictionary, token, user } = useContext(SportsContext);
  const navigate = useNavigate();
  const [userData, setUserData] = useState(null);
  const [latestNews, setLatestNews] = useState(null);
  const [loading, setLoading] = useState(true);
  const [upcomingClasses, setUpcomingClasses] = useState([]);
  const [classesLoading, setClassesLoading] = useState(true);
  const [balance, setBalance] = useState(0);
  const [balanceLoading, setBalanceLoading] = useState(true);
  const [discounts, setDiscounts] = useState({
    classes: 0,
    products: 0,
  });

  useEffect(() => {
    const fetchUserAndData = async () => {

      try {
        const accountResponse = await getAccountInfo(token);
        
        if (!accountResponse.ok) {
          throw new Error(`Account info failed: ${accountResponse.status}`);
        }
        
        const accountData = await accountResponse.json();
        
        setUserData(accountData);
        setBalance(accountData.balance);
        setDiscounts({
          classes: accountData.classesDiscount || 0,
          products: accountData.productsDiscount || 0
        });
        
        const newsResponse = await getNews(0);
        const newsData = await newsResponse.json();
        if (newsData?.length > 0) {
          setLatestNews([...newsData].sort((a, b) => new Date(b.date) - new Date(a.date))[0]);
        }

        const activitiesResponse = await getYourUpcomingActivities(token);
        if (!activitiesResponse.ok) {
          throw new Error(`Activities request failed: ${activitiesResponse.status}`);
        }
        
        const activitiesData = await activitiesResponse.json();
        const now = new Date();
        const futureActivities = activitiesData
          .filter(activity => new Date(activity.dateOfActivity) >= now)
          .sort((a, b) => new Date(a.dateOfActivity) - new Date(b.dateOfActivity))
          .slice(0, 5);

        setUpcomingClasses(futureActivities);
      } catch (error) {
        console.error('Error in fetchUserAndData:', error);
      } finally {
        setLoading(false);
        setBalanceLoading(false);
        setClassesLoading(false);
      }
    };

    fetchUserAndData();
  },[]);

  function handleAddBalance() {
    alert("Funkcja doładowania salda jeszcze niezaimplementowana.");
  }

  function handleHowToGetDiscounts() {
    alert("Strona informacyjna o zniżkach nie jest jeszcze dostępna.");
  }

  return (
    <Box sx={{
      maxWidth: '1100px',
      margin: '0 auto',
      padding: '24px',
      height: '85vh',
      boxSizing: 'border-box',
      marginLeft:'17vw',
    }}>
      <Box sx={{ 
        width: '100%',
        marginBottom: '16px',
        textAlign: 'center'
      }}>
        <Header>{dictionary.mainPage.title}</Header>
      </Box>
  
      <Box sx={{
        display: 'flex',
        width: '100%',
        height: 'calc(100% - 60px)',
        gap: '50px'
      }}>
        <GreenBackground sx={{
          flex: '0 0 55%',
          height: '100%',
          padding: '24px',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'flex-start',
          gap: '32px'
        }}>
          <Box>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '24px',
              paddingLeft: '24px',
              textAlign: 'left'
            }}>
              {dictionary.mainPage.whatIsNewLabel}
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
                margin: '0 auto'
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
                {dictionary.mainPage.noNewsLabel}
              </Typography>
            )}
          </Box>
  
          <Box sx={{ 
            flex: 1,
            marginTop: '24px'
          }}>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '20px',
              paddingLeft: '24px',
              textAlign: 'left'
            }}>
              {dictionary.mainPage.upcomingClassesLabel}
            </Typography>
  
            <Box sx={{ 
              width: '90%',
              margin: '0 auto'
            }}>
              <OrangeBackground sx={{
                width: '100%',
                minHeight: '12vh',
                maxHeight: '12vh',
                padding: '16px',
                overflow: 'hidden',
                position: 'relative'
              }}>
                {classesLoading ? (
                  <Box sx={{ height: '100%', display: 'grid', placeItems: 'center' }}>
                    <CircularProgress sx={{ color: '#4caf50' }} />
                  </Box>
                ) : upcomingClasses.length > 0 ? (
                  <Box sx={{ 
                    display: 'flex',
                    gap: '4px',
                    height: '100%',
                    alignItems: 'center',
                    overflowX: 'auto',
                    justifyContent: 'center',
                    padding: '0 8px'
                  }}>
                    {upcomingClasses.map((activity, index) => {
                      const activityDate = new Date(activity.dateOfActivity);
                      const [hours, minutes] = activity.startHour.split(':').map(Number);
                      const startTime = new Date(activityDate);
                      startTime.setHours(hours, minutes, 0);
                      
                      const endTime = new Date(startTime);
                      endTime.setMinutes(endTime.getMinutes() + activity.durationInMinutes);
                      
                      return (
                        <Box key={index} sx={{ 
                          minWidth: '110px',
                          textAlign: 'center',
                          padding: '6px',
                          flexShrink: 0,
                          margin: '0 2px'
                        }}>
                          <Typography sx={{ 
                            color: 'black', 
                            fontWeight: 'bold', 
                            fontSize: '1.1rem'
                          }}>
                            {activityDate.getDate()}
                          </Typography>
                          <Typography sx={{ color: '#0AB68B', fontSize: '0.9rem' }}>
                            {dictionary.daysOfWeekShort?.[activity.dayOfWeek] || activity.dayOfWeek}
                          </Typography>
                          <Typography sx={{ 
                            color: 'black', 
                            fontWeight: 'bold',
                            margin: '4px 0',
                            overflow: 'hidden',
                            textOverflow: 'ellipsis'
                          }}>
                            {activity.sportActivityName.length > 14 
                              ? `${activity.sportActivityName.substring(0, 11)}...` 
                              : activity.sportActivityName}
                          </Typography>
                          <Typography sx={{ color: 'black', fontSize: '0.85rem' }}>
                            {startTime.toTimeString().substring(0, 5)}-{endTime.toTimeString().substring(0, 5)}
                          </Typography>
                        </Box>
                      );
                    })}
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
                    {dictionary.mainPage.noClassesLabel}
                  </Typography>
                )}
              </OrangeBackground>
            </Box>
          </Box>
        </GreenBackground>
  
        <GreenBackground sx={{
          flex: '0 0 45%',
          height: '100%',
          padding: '24px',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'space-between'
        }}>
          <Box>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '12px'
            }}>
              {dictionary.mainPage.currentBalanceLabel}
            </Typography>
            
            {balanceLoading ? (
              <CircularProgress size={36} sx={{ color: '#4caf50' }} />
            ) : (
              <>
                <Typography sx={{ 
                  color: '#0AB68B',
                  fontWeight: 'bold',
                  fontSize: '2rem',
                  margin: '16px 0'
                }}>
                  {balance.toFixed(2)} zł
                </Typography>
                <Box sx={{ width: '50%', margin: '0 auto 32px' }}>
                  {/* <GreenButton 
                    fullWidth
                    onClick={handleAddBalance}
                    sx={{ 
                      fontSize: '1.1rem',
                      padding: '10px'
                    }}
                  >
                    {dictionary.mainPage.addBalanceLabel}
                  </GreenButton> */}
                </Box>
              </>
            )}
          </Box>
  
          <Box>
            <Typography sx={{ 
              color: 'black', 
              fontWeight: 'bold', 
              fontSize: '2rem',
              marginBottom: '24px'
            }}>
              {dictionary.mainPage.activeDiscountsLabel}
            </Typography>
  
            <Box sx={{ width: '80%', margin: '0 auto 16px' }}>
              <OrangeBackground sx={{ 
                padding: '16px',
                width: '100%'
              }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Typography sx={{ color: 'black', fontSize: '1.4rem' }}>
                    {dictionary.mainPage.classesDiscountLabel}
                  </Typography>
                  <Typography sx={{ 
                    color: discounts.classes > 0 ? '#0AB68B' : 'red',
                    fontWeight: 'bold',
                    fontSize: '1.4rem'
                  }}>
                    {discounts.classes > 0 ? `-${discounts.classes}%` : dictionary.mainPage.noDiscountsLabel}
                  </Typography>
                </Box>
              </OrangeBackground>
            </Box>
  
            <Box sx={{ width: '80%', margin: '0 auto' }}>
              <OrangeBackground sx={{ 
                padding: '16px',
                width: '100%'
              }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between' }}>
                  <Typography sx={{ color: 'black', fontSize: '1.4rem' }}>
                    {dictionary.mainPage.productsDiscountLabel}
                  </Typography>
                  <Typography sx={{ 
                    color: discounts.products > 0 ? '#0AB68B' : 'red',
                    fontWeight: 'bold',
                    fontSize: '1.4rem'
                  }}>
                    {discounts.products > 0 ? `-${discounts.products}%` : dictionary.mainPage.noDiscountsLabel}
                  </Typography>
                </Box>
              </OrangeBackground>
            </Box>
          </Box>
  
          <Typography
            sx={{
              color: 'black',
              textDecoration: 'underline',
              cursor: 'pointer',
              fontSize: '1.05rem',
              marginTop: '24px',
              textAlign: 'center',
              '&:hover': { color: '#0AB68B' }
            }}
            onClick={handleHowToGetDiscounts}
          >
            {dictionary.mainPage.howToGetDiscountsLabel}
          </Typography>
        </GreenBackground>
      </Box>
    </Box>
  );
}
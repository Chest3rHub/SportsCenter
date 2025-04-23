import React, { useState, useContext, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import addClientDiscount from '../api/addClientDiscount';
import CustomInput from '../components/CustomInput';
import { Box, Typography, Avatar } from '@mui/material';
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

function AddClientDiscount() {

    const navigate = useNavigate();
    const { dictionary, toggleLanguage, token } = useContext(SportsContext);

    const location = useLocation();
    const { email } = location.state || {};
  
    const [formData, setFormData] = useState({
      clientEmail: email || '',
      activityDiscount: '',
      productDiscount: ''
    });
  
    const [activityDiscountError, setActivityDiscountError] = useState(false);
  
    const [productDiscountError, setProductDiscountError] = useState(false);
  
  
    const [openSuccessBackdrop, setOpenSuccessBackdrop] = React.useState(false);
    const [openFailureBackdrop, setOpenFailureBackdrop] = React.useState(false);
  
    useEffect(() => {
      if (email) {
          setFormData((prev) => ({
              ...prev,
              clientEmail: email
          }));
      }
  }, [email]);
  
    const handleCloseSuccess = () => {
      setOpenSuccessBackdrop(false);
    };
    const handleCloseFailure = () => {
      setOpenFailureBackdrop(false);
    };
    const handleOpenSuccess = () => {
      setOpenSuccessBackdrop(true);
    };
    const handleOpenFailure = () => {
      setOpenFailureBackdrop(true);
    };
  
    const validateForm = () => {

      const activityDiscountValue = parseFloat(formData.activityDiscount);
      if (isNaN(activityDiscountValue) || activityDiscountValue < 0) {
        setActivityDiscountError(true);
        return false;
      }
      setActivityDiscountError(false);
    

      const productDiscountValue = parseFloat(formData.productDiscount);
      if (isNaN(productDiscountValue) || productDiscountValue < 0) {
        setProductDiscountError(true);
        return false;
      }
      setProductDiscountError(false);
    
      return true;
    };
    
    const handleChange = (e) => {
      const { name, value } = e.target;
      setFormData((prev) => ({
        ...prev,
        [name]: value
      }));
    };
  
    const handleSubmit = async (e) => {
      e.preventDefault();
  
      if (!validateForm()) return;
  
      try {
  
        const response = await addClientDiscount(formData, token);
        if (!response.ok) {
          const errorData = await response.json();
          console.log('Błąd odpowiedzi z API:', errorData);
          if (errorData.errors) {
            
            console.error('Szczegóły błędów walidacji:', errorData.errors);
        }
        console.log(formData.clientEmail);
          handleOpenFailure();
        } else {
          setFormData({
            clientEmail: '',
            activityDiscount: '',
            productDiscount: ''
          });
          handleOpenSuccess();
        }
      } catch (error) {
        console.error('Błąd dodawania zniżki:', error);
        handleOpenFailure();
      }
    };

    function handleCancel() {
      navigate(-1);
  }

    return (
      <>
        <GreenBackground height="55vh" marginTop="2vh">
          <Header>{dictionary.addClientDiscountPage.discountLabel}</Header>
          <OrangeBackground width="70%">
            <form onSubmit={handleSubmit}>
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginBottom: '2vh' }}>
                <CustomInput
                  label={dictionary.addClientDiscountPage.clientEmailLabel}
                  type="email"
                  id="clientEmail"
                  name="clientEmail"
                  fullWidth
                  value={formData.clientEmail}
                  disabled
                  size="small"
                />
                <CustomInput
                  label={dictionary.addClientDiscountPage.activityDiscountLabel}
                  type="number"
                  id="activityDiscount"
                  name="activityDiscount"
                  fullWidth
                  value={formData.activityDiscount}
                  onChange={handleChange}
                  error={activityDiscountError}
                  helperText={activityDiscountError ? dictionary.addClientDiscountPage.activityDiscountError : ''}
                  required
                  size="small"
                />
                <CustomInput
                  label={dictionary.addClientDiscountPage.productDiscountLabel}
                  type="number"
                  id="productDiscount"
                  name="productDiscount"
                  fullWidth
                  value={formData.productDiscount}
                  onChange={handleChange}
                  error={productDiscountError}
                  helperText={productDiscountError ? dictionary.addClientDiscountPage.productDiscountError : ''}
                  required
                  size="small"
                />
                <Box sx={{ 
                  display: 'flex', 
                  flexDirection: 'row', 
                  justifyContent: 'center', 
                  columnGap: '4vw' 
                }}>
                  <GreenButton onClick={handleCancel} style={{ maxWidth: '10vw', backgroundColor: '#F46C63' }} hoverBackgroundColor="#c3564f">{dictionary.addClientDiscountPage.returnLabel}</GreenButton>
                  <GreenButton type="submit" style={{ maxWidth: '10vw' }}>{dictionary.addClientDiscountPage.saveLabel}</GreenButton>
                </Box>
              </Box>
              <Backdrop 
                sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })} 
                open={openSuccessBackdrop} 
                onClick={handleCloseSuccess}
              >
                <Box sx={{ 
                  backgroundColor: 'white', 
                  margin: 'auto', 
                  minWidth: '30vw', 
                  minHeight: '30vh', 
                  borderRadius: '20px', 
                  boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)' 

                }}>
                  <Box>
                    <Typography sx={{ 
                      color: 'green', 
                      fontWeight: 'bold', 
                      fontSize: '3rem', 
                      marginTop: '2vh' 
                    }}>
                      {dictionary.addClientDiscountPage.successLabel}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography sx={{ 
                      color: 'black', 
                      fontSize: '1.5rem' 
                    }}>
                      {dictionary.addClientDiscountPage.savedSuccessLabel}
                  </Typography>
                  </Box>
                  <Box>
                    <Typography sx={{ 
                      color: 'black', 
                      fontSize: '1.5rem' 
                      }}>
                      {dictionary.addClientDiscountPage.clickAnywhereLabel}
                    </Typography>
                  </Box>
                  <Box sx={{ textAlign: 'center', display: 'flex', justifyContent: 'center' }}>
                    <Avatar sx={{ width: '7rem', height: '7rem' }}>
                      <SentimentSatisfiedIcon sx={{ fontSize: '7rem', color: 'green', stroke: 'white', strokeWidth: 1.1, backgroundColor: 'white' }} />
                    </Avatar>
                  </Box>
                </Box>
              </Backdrop>
              <Backdrop 
                sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })} 
                open={openFailureBackdrop} 
                onClick={handleCloseFailure}
              >
                <Box sx={{ 
                  backgroundColor: 'white', 
                  margin: 'auto', 
                  minWidth: '30vw', 
                  minHeight: '30vh', 
                  borderRadius: '20px', 
                  boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)' 

                }}>
                  <Box>
                    <Typography sx={{ 
                      color: 'red', 
                      fontWeight: 'bold', 
                      fontSize: '3rem', 
                      marginTop: '2vh' }}>
                      {dictionary.addClientDiscountPage.failureLabel}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography sx={{ 
                      color: 'black', 
                      fontSize: '1.5rem' }}>
                      {dictionary.addClientDiscountPage.savedFailureLabel}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography sx={{ 
                      color: 'black', 
                      fontSize: '1.5rem' }}>
                      {dictionary.addClientDiscountPage.clickAnywhereFailureLabel}
                    </Typography>
                  </Box>
                  <Box sx={{ textAlign: 'center', display: 'flex', justifyContent: 'center' }}>
                    <Avatar sx={{ width: '7rem', height: '7rem' }}>
                      <SentimentDissatisfiedIcon sx={{ fontSize: '7rem', color: 'red', stroke: 'white', strokeWidth: 1.1, backgroundColor: 'white' }} />
                    </Avatar>
                  </Box>
                </Box>
              </Backdrop>
            </form>
          </OrangeBackground>
        </GreenBackground>
      </>
    );
  }
  
  export default AddClientDiscount;
import React, { useState, useContext, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import updateClientDeposit from '../api/updateClientDeposit';
import CustomInput from '../components/CustomInput';
import { Box, Typography, Avatar } from '@mui/material';
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

function UpdateClientDeposit() {
    const navigate = useNavigate();
    const { dictionary, toggleLanguage, token } = useContext(SportsContext);

    const location = useLocation();
    const { email } = location.state || {};
  
    const [formData, setFormData] = useState({
      deposit: '',
      email: email || ''
    });
  
    const [depositError, setDepositError] = useState(false);
  
    const [openSuccessBackdrop, setOpenSuccessBackdrop] = React.useState(false);
    const [openFailureBackdrop, setOpenFailureBackdrop] = React.useState(false);
  
    useEffect(() => {
      if (email) {
          setFormData((prev) => ({
              ...prev,
              email: email
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

    const depositValue= parseFloat(formData.deposit);
    if (isNaN(depositValue) || depositValue <= 0 || depositValue > 5000) {
      setDepositError(true);
      return false;
    }
    setDepositError(false);
  
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
      const response = await updateClientDeposit(formData, token);
      if (!response.ok) {
        const errorData = await response.json();
        console.error('Błąd odpowiedzi z API:', errorData);
        handleOpenFailure();
      } else {
        setFormData({
          deposit: '',
          email: ''
        });
        handleOpenSuccess();
      }
    } catch (error) {
      console.error('Błąd aktualizacji salda:', error);
      handleOpenFailure();
    }
  };

  const handleCancel = () => {
    navigate(-1);
  };

  return (
    <GreenBackground height="55vh" marginTop="2vh">
      <Header>{dictionary.updateClientDeposit.depositLabel}</Header>
      <OrangeBackground width="70%">
        <form onSubmit={handleSubmit}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginBottom: '2vh' }}>
            <CustomInput
              label={dictionary.updateClientDeposit.clientEmailLabel}
              type="email"
              id="email"
              name="email"
              fullWidth
              value={formData.email}
              disabled
              size="small"
            />
            <CustomInput
              label={dictionary.updateClientDeposit.depositValueLabel}
              type="number"
              id="deposit"
              name="deposit"
              fullWidth
              value={formData.deposit}
              onChange={handleChange}
              error={depositError}
              helperText={depositError ? dictionary.updateClientDeposit.depositError : ''}
              required
              size="small"
            />
            <Box sx={{ 
                display: 'flex', 
                justifyContent: 'center', 
                columnGap: '4vw' 
            }}>
              <GreenButton onClick={handleCancel} style={{ maxWidth: '10vw', backgroundColor: '#F46C63' }} hoverBackgroundColor="#c3564f">{dictionary.updateClientDeposit.returnLabel}</GreenButton>
              <GreenButton type="submit" style={{ maxWidth: '10vw' }}>{dictionary.updateClientDeposit.saveLabel}</GreenButton>
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
                        mt: '2vh' 
                    }}>
                        {dictionary.updateClientDeposit.successLabel}
                    </Typography>
              </Box>
              <Box>
                <Typography sx={{ 
                    color: 'black', 
                    fontSize: '1.5rem' 
                    }}>
                        {dictionary.updateClientDeposit.savedSuccessLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ 
                    color: 'black', 
                    fontSize: '1.5rem' 
                    }}>
                        {dictionary.updateClientDeposit.clickAnywhereLabel}
                </Typography>
              </Box>
              <Box sx={{textAlign: 'center', display: 'flex', justifyContent: 'center'  }}>
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
                    mt: '2vh' 
                    }}>
                        {dictionary.updateClientDeposit.failureLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ 
                    color: 'black', 
                    fontSize: '1.5rem' 
                    }}>
                        {dictionary.updateClientDeposit.savedFailureLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ 
                    color: 'black', 
                    fontSize: '1.5rem' 
                    }}>
                        {dictionary.updateClientDeposit.clickAnywhereFailureLabel}
                </Typography>
              </Box>
              <Box sx={{  textAlign: 'center', display: 'flex', justifyContent: 'center' }}>
                <Avatar sx={{ width: '7rem', height: '7rem' }}>
                  <SentimentDissatisfiedIcon sx={{ fontSize: '7rem', color: 'red', stroke: 'white', strokeWidth: 1.1, backgroundColor: 'white'  }} />
                </Avatar>
              </Box>
            </Box>
          </Backdrop>
        </form>
      </OrangeBackground>
    </GreenBackground>
  );

}
export default updateClientDeposit;
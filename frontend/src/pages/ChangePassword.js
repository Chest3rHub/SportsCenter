import React, { useState, useContext } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import CustomInput from '../components/CustomInput';
import { Box, Typography, Avatar } from '@mui/material';
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import changePasswordRequest from '../api/changePasswordRequest';
import changeSomeonePassword from '../api/changeSomeonePassword';
function ChangePassword() {

  const location = useLocation();

  const { id } = location.state || {};

  const { dictionary, toggleLanguage, token } = useContext(SportsContext);

  const [formData, setFormData] = useState({
    newPassword: '',
    confirmNewPassword: ''
  });

  const [newPasswordError, setNewPasswordError] = useState(false);

  const [confirmNewPasswordError, setConfirmNewPasswordError] = useState(false);

  const [openSuccessBackdrop, setOpenSuccessBackdrop] = useState(false);
  const [openFailureBackdrop, setOpenFailureBackdrop] = useState(false);
  
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
    let isValid = true;

    const passwordRegex = /^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]).{6,}$/;

    if (!passwordRegex.test(formData.newPassword)) {
      isValid = false;
      setNewPasswordError(true);
    } else {
      setNewPasswordError(false);
    }

    if (formData.newPassword !== formData.confirmNewPassword) {
      isValid = false;
      setConfirmNewPasswordError(true);
    } else {
      setConfirmNewPasswordError(false);
    }

    return isValid;
  };


  function handleError(textToDisplay) {
    // navigate('/error', {
    //   state: { message: textToDisplay }
    // });
    handleOpenFailure();
  }


  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      return;
    }


    try {
      let response;
      if(id){
        response = await changeSomeonePassword(formData, id, token);
      } else {
        response = await changePasswordRequest(formData, token);
      }

      if (!response.ok) {
        const errorData = await response.json();
        console.log(errorData);
        handleError('Blad zmiany hasła... sprawdz konsole');
      } else {
        setFormData({
            newPassword: '',
            confirmNewPassword: '',
        });
        handleOpenSuccess();
      }

    } catch (error) {
        handleError('Blad zmiany hasła... sprawdz konsole');
    }
  };

  function handleCancel() {
    navigate(-1);
}

  return (
    <>
      <GreenBackground height={"55vh"} marginTop={"2vh"}>
        <Header>{dictionary.changePasswordPage.changePasswordLabel}</Header>
        <OrangeBackground width="70%">
          <form onSubmit={handleSubmit}>
            <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
              <CustomInput
                label={dictionary.changePasswordPage.newPasswordLabel}
                type="password"
                id="newPassword"
                name="newPassword"
                fullWidth
                value={formData.newPassword}
                onChange={handleChange}
                error={newPasswordError}
                helperText={newPasswordError ? dictionary.changePasswordPage.newPasswordError : ""}
                required
                size="small"
              />
              <CustomInput
                label={dictionary.changePasswordPage.confirmNewPasswordLabel}
                type="password"
                id="confirmNewPassword"
                name="confirmNewPassword"
                fullWidth
                value={formData.confirmNewPassword}
                onChange={handleChange}
                error={confirmNewPasswordError}
                helperText={confirmNewPasswordError ? dictionary.changePasswordPage.confirmNewPasswordError : ""}
                required
                size="small"
              />
              <Box sx={{
                display: "flex",
                flexDirection: "row",
                justifyContent: 'center',
                columnGap: "4vw"
              }}>
                <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.changePasswordPage.returnLabel}</GreenButton>
                <GreenButton type="submit" style={{ maxWidth: "10vw" }}>{dictionary.changePasswordPage.saveLabel}</GreenButton>
              </Box>
            </Box>
            <Backdrop
              sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
              open={openSuccessBackdrop}
              onClick={handleCloseSuccess}
            >
              <Box sx={{
                backgroundColor: "white",
                margin: 'auto',
                minWidth: '30vw',
                minHeight: '30vh',
                borderRadius: '20px',
                boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',

              }}>
                <Box>
                  <Typography sx={{
                    color: 'green',
                    fontWeight: 'Bold',
                    fontSize: '3rem',
                    marginTop: '2vh',

                  }}>
                    {dictionary.changePasswordPage.successLabel}
                  </Typography>
                </Box>
                <Box>
                  <Typography sx={{
                    color: 'black',
                    fontSize: '1.5rem',
                  }}>
                    {dictionary.changePasswordPage.savedSuccessLabel}
                  </Typography>
                </Box>
                <Box>
                  <Typography sx={{
                    color: 'black',
                    fontSize: '1.5rem',
                  }}>
                    {dictionary.changePasswordPage.clickAnywhereLabel}
                  </Typography>
                </Box>
                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                  <Avatar sx={{ width: "7rem", height: "7rem" }}>
                    <SentimentSatisfiedIcon sx={{ fontSize: "7rem", color: 'green', stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }} />
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
                backgroundColor: "white",
                margin: 'auto',
                minWidth: '30vw',
                minHeight: '30vh',
                borderRadius: '20px',
                boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',

              }}>
                <Box>
                  <Typography sx={{
                    color: 'red',
                    fontWeight: 'Bold',
                    fontSize: '3rem',
                    marginTop: '2vh',

                  }}>
                    {dictionary.changePasswordPage.failureLabel}
                  </Typography>
                </Box>
                <Box>
                  <Typography sx={{
                    color: 'black',
                    fontSize: '1.5rem',
                  }}>
                    {dictionary.changePasswordPage.savedFailureLabel}
                  </Typography>
                </Box>
                <Box>
                  <Typography sx={{
                    color: 'black',
                    fontSize: '1.5rem',
                  }}>
                    {dictionary.changePasswordPage.clickAnywhereFailureLabel}
                  </Typography>
                </Box>
                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                  <Avatar sx={{ width: "7rem", height: "7rem" }}>
                    <SentimentDissatisfiedIcon sx={{ fontSize: "7rem", color: 'red', stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }} />
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

export default ChangePassword;

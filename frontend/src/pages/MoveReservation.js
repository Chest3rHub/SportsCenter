import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import { SportsContext } from "../context/SportsContext";
import moveReservation from "../api/moveReservation";
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import CustomInput from '../components/CustomInput';
import { Box, Typography, Avatar } from '@mui/material';
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

function MoveReservation() {
    const location = useLocation();
    const { id, offsetFromLocation } = location.state || {};
    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        newStartTime: '',
        newEndTime: ''
    });

    const [newStartTimeError, setNewStartTimeError] = useState(false);
    const [newEndTimeError, setNewEndTimeError] = useState(false); 
    const [newTimeDurationError, setNewTimeDurationError] = useState(false); 

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

        const now = new Date();
        const start = new Date(formData.newStartTime);
        const end = new Date(formData.newEndTime);

        if (start < now) {
            isValid = false;
            setNewStartTimeError(true);
        } else {
            setNewStartTimeError(false);
        }

        if (end <= start) {
            setNewEndTimeError(true);
            isValid = false;
        } else {
            setNewEndTimeError(false);
        }

        const hoursDiff = (end - start) / (1000 * 60 * 60);
        if (hoursDiff < 1 || hoursDiff > 5) {
            setNewTimeDurationError(true);
            isValid = false;
        } else {
            setNewTimeDurationError(false);
        }

        return isValid;
    }

    function handleError(textToDisplay) {
        handleOpenFailure();
    }

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
          response = await moveReservation(formData, id);
    
          if (!response.ok) {
            const errorData = await response.json();
            console.log(errorData);
            handleError('Blad przeniesienia rezerwacji... sprawdz konsole');
          } else {
            setFormData({
                newStartTime: '',
                newEndTime: ''
            });
            handleOpenSuccess();
          }
    
        } catch (error) {
            handleError('Blad przeniesienia rezerwacji... sprawdz konsole');
        }
    };

    function handleCancel() {
        navigate('/my-reservations', {
            state: { offsetFromLocation }  
          });
    }

    return (
        <>
        <GreenBackground height={"55vh"} marginTop={"2vh"}>
            <Header>{dictionary.moveReservation.title}</Header>
            <OrangeBackground width="70%">
                <form onSubmit={handleSubmit}>
                <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
                    <CustomInput
                        label={dictionary.moveReservation.newStartTimeLabel}
                        type="datetime-local"
                        id="newStartTime"
                        name="newStartTime"
                        fullWidth
                        value={formData.newStartTime}
                        onChange={handleChange}
                        error={newStartTimeError}
                        helperText={newStartTimeError ? dictionary.moveReservation.newStartTimeError : ""}
                        required
                        size="small"
                        InputLabelProps={{
                            shrink: true
                        }}
                    />
                    <CustomInput
                        label={dictionary.moveReservation.newEndTimeLabel}
                        type="datetime-local"
                        id="newEndTime"
                        name="newEndTime"
                        fullWidth
                        value={formData.newEndTime}
                        onChange={handleChange}
                        error={newEndTimeError}
                        helperText={newEndTimeError ? dictionary.moveReservation.newEndTimeError : ""}
                        size="small"
                        required
                        InputLabelProps={{
                            shrink: true
                        }}
                    />

                    {newTimeDurationError && (
                    <Typography sx={{ color: 'red', fontSize: '0.9rem', marginTop: '-0.5rem' }}>
                        {dictionary.moveReservation.newTimeDurationError}
                    </Typography>
                    )}

                    <Box sx={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: 'center',
                        columnGap: "4vw"
                    }}>
                    <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.moveReservation.returnLabel}</GreenButton>
                    <GreenButton type="submit" style={{ maxWidth: "10vw" }}>{dictionary.moveReservation.saveLabel}</GreenButton>
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
                    {dictionary.moveReservation.successLabel}
                    </Typography>
                    </Box>
                    <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                    {dictionary.moveReservation.savedSuccessLabel}
                    </Typography>
                    </Box>
                    <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                    {dictionary.moveReservation.clickAnywhereLabel}
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
                    {dictionary.moveReservation.failureLabel}
                    </Typography>
                    </Box>
                    <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                    {dictionary.moveReservation.savedFailureLabel}
                    </Typography>
                    </Box>
                    <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                    {dictionary.moveReservation.clickAnywhereFailureLabel}
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
    )
}

export default MoveReservation;
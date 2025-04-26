import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import addReservationYourself from '../api/addReservationYourself';
import CustomInput from '../components/CustomInput';
import { Box } from '@mui/material';
import { Checkbox, FormControlLabel } from '@mui/material';
function AddReservationYourself() {

    const { dictionary, toggleLanguage } = useContext(SportsContext);
    
    const [formData, setFormData] = useState({
        courtId: '',
        startTime: '',
        endTime: '',
        creationDate: '',
        trainerId: '',
        participantsCount: '',
        isEquipmentReserved: '',
    });

    const [courtIdError, setCourtIdError] = useState(false);
    
    const [startTimeError, setStartTimeError] = useState(false); 

    const [endTimeRequiredError, setEndTimeRequiredError] = useState(false);
    const [endTimeBeforeStartError, setEndTimeBeforeStartError] = useState(false);
    const [endTimeDurationError, setEndTimeDurationError] = useState(false);


    const [trainerIdError, setTrainerIdError] = useState(false);
      
    const [participantsCountError, setParticipantsCountError] = useState(false);
      
    const [isEquipmentReservedError, setIsEquipmentReservedError] = useState(false);

   
    const validateForm = () => {
        let isValid = true;
    
        if (!formData.courtId || isNaN(parseInt(formData.courtId))) {
            isValid = false;
            setCourtIdError(true);
        } else {
            setCourtIdError(false);
        }
    
        const start = new Date(formData.startTime);
        const end = new Date(formData.endTime);
    
        if (!formData.startTime) {
            isValid = false;
            setStartTimeError(true);
        } else {
            setStartTimeError(false);
        }

        if (!formData.endTime) {
            isValid = false;
            setEndTimeRequiredError(true);
        } else {
            setEndTimeRequiredError(false);
        }
    

        if (formData.endTime && end <= start) {
            isValid = false;
            setEndTimeBeforeStartError(true);
        } else {
            setEndTimeBeforeStartError(false);
        }
    
        //czas trwania rezerwacji min 1h max 5h
        if (formData.startTime && formData.endTime) {
            const diffInHours = (end - start) / (1000 * 60 * 60);
            if (diffInHours < 1 || diffInHours > 5) {
                isValid = false;
                setEndTimeDurationError(true);
            } else {
                setEndTimeDurationError(false);
            }
        }

        if (!formData.trainerId || isNaN(parseInt(formData.trainerId))) {
            isValid = false;
            setTrainerIdError(true);
        } else {
            setTrainerIdError(false);
        }
    
        //ilosc uczestnikow od 1 do 8
        const participants = parseInt(formData.participantsCount);
        if (!participants || participants < 1 || participants > 8) {
            isValid = false;
            setParticipantsCountError(true);
        } else {
            setParticipantsCountError(false);
        }

       
        return isValid;
    };
    



    function handleError(textToDisplay) {
        navigate('/error', {
          state: { message: textToDisplay }
        });
    }
    
    
    const navigate = useNavigate();
    
    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData((prev) => ({
          ...prev,
           [name]: type === "checkbox" ? checked : value,
        }));
    };
    
    const handleSubmit = async (e) => {
        e.preventDefault();
    
        if (!validateForm()) {
          return;
        }

    
         try {
              const response = await addReservationYourself(formData);
        
              if (!response.ok) {
                const errorData = await response.json();
                console.log(errorData);        
                  handleError('Blad rezerwacji... sprawdz konsole');
              } else {
                navigate('/my-reservations');
              }
        
            } catch (error) {
              console.error('Błąd rezerwacji:', error);
            }
          };

          const endTimeE = endTimeRequiredError || endTimeBeforeStartError || endTimeDurationError;
          let endTimeErrorMessage = "";
          if (endTimeRequiredError) {
            endTimeErrorMessage = dictionary.addReservationYourselfPage.endTimeRequiredError;
          } else if (endTimeBeforeStartError) {
            endTimeErrorMessage = dictionary.addReservationYourselfPage.endTimeBeforeStartError;
          } else if (endTimeDurationError) {
            endTimeErrorMessage = dictionary.addReservationYourselfPage.endTimeDurationError;
          }
        
  return (
    <>
      <GreenBackground height={"80vh"} marginTop={"2vh"}>
        <Header>{dictionary.addReservationYourselfPage.title}</Header>
        <OrangeBackground width="70%">
          <form onSubmit={handleSubmit}>
            <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
              <CustomInput
                label={dictionary.addReservationYourselfPage.courtIdLabel}
                type="number"
                id="courtId"
                name="courtId"
                fullWidth
                value={formData.courtId}
                onChange={handleChange}
                error={courtIdError}
                helperText={courtIdError ? dictionary.addReservationYourselfPage.courtIdError : ""}
                required
                size="small"
              />
              <CustomInput
                label={dictionary.addReservationYourselfPage.startTimeLabel}
                type="datetime-local"
                id="startTime"
                name="startTime"
                fullWidth
                value={formData.startTime}
                onChange={handleChange}
                error={startTimeError}
                helperText={startTimeError ? dictionary.addReservationYourselfPage.startTimeError : ""}
                required
                size="small"
                InputLabelProps={{
                    shrink: true
                }}
              />
              <CustomInput
                label={dictionary.addReservationYourselfPage.endTimeLabel}
                type="datetime-local"
                id="endTime"
                name="endTime"
                fullWidth
                value={formData.endTime}
                onChange={handleChange}
                error={endTimeE}
                helperText={endTimeErrorMessage}
                size="small"
                required
                InputLabelProps={{
                    shrink: true
                }}
              />
              <CustomInput
                label={dictionary.addReservationYourselfPage.trainerIdLabel}
                type="number"
                id="trainerId"
                name="trainerId"
                fullWidth
                value={formData.trainerId}
                onChange={handleChange}
                error={trainerIdError}
                helperText={trainerIdError ? dictionary.addReservationYourselfPage.trainerIdError : ""}
                required
                size="small"
              />
              <CustomInput
                label={dictionary.addReservationYourselfPage.participantsCountLabel}
                type="number"
                id="participantsCount"
                name="participantsCount"
                fullWidth
                value={formData.participantsCount}
                onChange={handleChange}
                error={participantsCountError}
                helperText={participantsCountError ? dictionary.addReservationYourselfPage.participantsCountError : ""}
                size="small"
                required
              />
              <FormControlLabel
                control={
                <Checkbox
                    id="isEquipmentReserved"
                    name="isEquipmentReserved"
                    checked={formData.isEquipmentReserved}
                    onChange={handleChange}
                    sx={{
                        color: "#8edfb4",
                        '&.Mui-checked': {
                        color: "#8edfb4",
                        },
                    }}
                />
             }
                label={dictionary.addReservationYourselfPage.isEquipmentReservedLabel}
            />
                {isEquipmentReservedError && (
            <div style={{ color: 'red', fontSize: '0.8rem', marginTop: '4px' }}>
                {dictionary.addReservationYourselfPage.isEquipmentReservedError}
             </div>
            )}
              <GreenButton type="submit">{dictionary.addReservationYourselfPage.confirmLabel}</GreenButton>
            </Box>
          </form>
        </OrangeBackground>
      </GreenBackground>
    </>
  );

    
}

export default AddReservationYourself;
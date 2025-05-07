import React, { useState, useContext, useEffect   } from 'react';
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
import getAvailableTrainers from '../api/getAvailableTrainers';
import getAvailableCourts from '../api/getAvailableCourts';
import MenuItem from '@mui/material/MenuItem';

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
    
    const [startTimeError, setStartTimeError] = useState(false); 

    const [endTimeRequiredError, setEndTimeRequiredError] = useState(false);
    const [endTimeBeforeStartError, setEndTimeBeforeStartError] = useState(false);
    const [endTimeDurationError, setEndTimeDurationError] = useState(false);
      
    const [participantsCountError, setParticipantsCountError] = useState(false);
      
    const [isEquipmentReservedError, setIsEquipmentReservedError] = useState(false);

    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [availableCourts, setAvailableCourts] = useState([]);

    const now = new Date();
    const localISOTime = new Date(now.getTime() - now.getTimezoneOffset() * 60000)
        .toISOString()
        .slice(0, 16);
  
  function getStartAndEndTime(startTime, endTime) {

    const startDate = new Date(startTime);

    const startHours = startDate.getHours();
    const startMinutes = startDate.getMinutes();

    startDate.setUTCHours(startHours, startMinutes, 0, 0);

    const endDate = new Date(endTime);

    const endHours = endDate.getHours(); 
    const endMinutes = endDate.getMinutes(); 

    endDate.setUTCHours(endHours, endMinutes, 0, 0);
  
    // Funkcja do konwersji daty do formatu UTC: "YYYY-MM-DDTHH:MM:SSZ"
    const toUTCDateTimeString = (d) => {
        const year = d.getUTCFullYear();
        const month = String(d.getUTCMonth() + 1).padStart(2, '0');
        const day = String(d.getUTCDate()).padStart(2, '0');
        const hours = String(d.getUTCHours()).padStart(2, '0');
        const minutes = String(d.getUTCMinutes()).padStart(2, '0');
        const seconds = String(d.getUTCSeconds()).padStart(2, '0');
        return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}Z`; //dodanie "Z"
    };

    // Zwrócenie dat w formacie UTC
    return {
        startTimeUTC: toUTCDateTimeString(startDate),
        endTimeUTC: toUTCDateTimeString(endDate)
    };
}

    const validateForm = () => {
        let isValid = true;

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

 
     useEffect(() => {
         const fetchAvailability = async () => {
             if (!formData.startTime || !formData.endTime) return;
         
             const { startTimeUTC, endTimeUTC } = getStartAndEndTime(
                 formData.startTime,
                 formData.endTime,
             );
         
             try {
                 const [trainersResponse, courtsResponse] = await Promise.all([
                     getAvailableTrainers(startTimeUTC, endTimeUTC),
                     getAvailableCourts(startTimeUTC, endTimeUTC),
                 ]);
 
                 setAvailableTrainers(trainersResponse.length ? trainersResponse : []);
                 setAvailableCourts(courtsResponse.length ? courtsResponse : []);
 
             } catch (error) {
                 console.error('Error fetching availability:', error);
                 setAvailableTrainers([]);
                 setAvailableCourts([]);
             }
         };
         
         fetchAvailability();
     }, [formData.startTime, formData.endTime]);


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
              inputProps={{
                min: localISOTime
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
                inputProps={{
                  min: localISOTime
                }}
              />
              <CustomInput
                select
                label={dictionary.addReservationYourselfPage.trainerNameLabel}
                id="trainerName"
                name="trainerId"
                value={formData.trainerId}
                fullWidth
                onChange={handleChange}
                required
                size="small"
                SelectProps={{
                  sx: { textAlign: 'left' }
                }}
              >
              <MenuItem value="">
              </MenuItem>
                  {availableTrainers.map(trainer => (
              <MenuItem key={trainer.id} value={trainer.id}>
                  {trainer.fullName}
              </MenuItem>
              ))}
              </CustomInput>
              <CustomInput
                select
                label={dictionary.addReservationYourselfPage.courtNameLabel}
                id="courtName"
                name="courtId"
                value={formData.courtId}
                fullWidth
                onChange={handleChange}
                required
                size="small"
                SelectProps={{
                  sx: { textAlign: 'left' }
                  }}
                >
                <MenuItem value="">
                </MenuItem>
                    {availableCourts.map(court => (
                <MenuItem key={court.id} value={court.id}>
                    {court.name}
                </MenuItem>
                ))}
              </CustomInput>
              <CustomInput
                select
                label={dictionary.addReservationYourselfPage.participantsCountLabel}
                id="participantsCount"
                name="participantsCount"
                value={formData.participantsCount}
                fullWidth
                onChange={handleChange}
                error={participantsCountError}
                helperText={participantsCountError ? dictionary.addReservationYourselfPage.participantsCountError : ""}
                size="small"
                required
                SelectProps={{
                  sx: { textAlign: 'left' }
                }}
              >
              <MenuItem value=""></MenuItem>
                {[...Array(8)].map((_, i) => (
                  <MenuItem key={i + 1} value={i + 1}>
                    {i + 1}
                  </MenuItem>
              ))}
              </CustomInput>
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
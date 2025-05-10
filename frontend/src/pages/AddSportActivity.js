import React, { useState, useContext, useEffect  } from 'react';
import { useNavigate} from "react-router-dom";
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import addActivity from '../api/addActivity';
import CustomInput from '../components/CustomInput';
import { Box } from '@mui/material';
import MenuItem from '@mui/material/MenuItem';
import getAvailableTrainers from '../api/getAvailableTrainers';
import getAvailableCourts from '../api/getAvailableCourts';
import getActivityLevelNames from '../api/getActivityLevelName';
import ErrorModal from '../components/ErrorModal';
import getSportsCenterWorkingHours from '../api/getSportsCenterWorkingHours';

function AddSportActivity() {

    const { dictionary, toggleLanguage } = useContext(SportsContext);
    
    const [formData, setFormData] = useState({
        sportActivityName: '',
        startDate: '',
        dayOfWeek: '',
        startHour: '',
        durationInMinutes: '',
        levelName: '',
        employeeId: '',
        participantLimit: '',
        courtName: '',
        costWithoutEquipment: '',
        costWithEquipment: '',
    });

    const [activityNameError, setActivityNameError] = useState(false);
   
    const [startDateError, setStartDateError] = useState(false); 

    const [startHourError, setStartHourError] = useState(false); 

    const [durationInMinutesError, setDurationInMinutesError] = useState(false); 

    const [levelNameError, setLevelNameError] = useState(false);

    const [trainerIdError, setTrainerIdError] = useState(false);

    const [participantsCountError, setParticipantsCountError] = useState(false);

    const [courtNameError, setCourtNameError] = useState(false);

    const [costWithoutEquipmentError, setCostWithoutEquipmentError] = useState(false);

    const [costWithEquipmentError, setCostWithEquipmentError] = useState(false);

    const [openModal, setOpenModal] = useState(false);
    const [errorMessage, setErrorMessage] = useState('');

    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [availableCourts, setAvailableCourts] = useState([]);
    const [levels, setLevels] = useState([]);

    const [workingHoursError, setWorkingHoursError] = useState(false);
    const [clubHoursMessage, setClubHoursMessage] = useState('');

    function getStartAndEndTime(dateString, hourString, durationInMinutes) {
        const [hours, minutes] = hourString.split(':').map(Number);
        const date = new Date(dateString);
        
        date.setUTCHours(hours, minutes, 0, 0);
    
        const startTime = new Date(date);
        const endTime = new Date(startTime.getTime() + durationInMinutes * 60000);
        
        //Konwersja na format backendowy: "YYYY-MM-DDTHH:MM:SSZ" z UTC
        const toUTCDateTimeString = (d) => {
            const year = d.getUTCFullYear();
            const month = String(d.getUTCMonth() + 1).padStart(2, '0');
            const day = String(d.getUTCDate()).padStart(2, '0');
            const hours = String(d.getUTCHours()).padStart(2, '0');
            const minutes = String(d.getUTCMinutes()).padStart(2, '0');
            const seconds = String(d.getUTCSeconds()).padStart(2, '0');
            return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}Z`; //dodanie "Z"
        };
    
        return {
            startTimeUTC: toUTCDateTimeString(startTime),
            endTimeUTC: toUTCDateTimeString(endTime)
        };
    }

    useEffect(() => {
        const fetchLevels = async () => {
            try {
                const levelResponse = await getActivityLevelNames();
                setLevels(levelResponse || []);
            } catch (error) {
                console.error('Błąd pobierania poziomów trudności:', error);
                setLevels([]);
            }
        };
    
        fetchLevels();
    }, []);
    
    
    useEffect(() => {
        const fetchAvailability = async () => {
            if (!formData.startDate || !formData.startHour || !formData.durationInMinutes) return;
    
            const { startTimeUTC, endTimeUTC } = getStartAndEndTime(
                formData.startDate,
                formData.startHour,
                formData.durationInMinutes
            );
    
            try {
                const workingHours = await getSportsCenterWorkingHours(formData.startDate);
    
                const [startHourH, startHourM] = formData.startHour.split(":").map(Number);
                const startLocal = new Date(formData.startDate);
                startLocal.setHours(startHourH, startHourM, 0, 0);
    
                const endLocal = new Date(startLocal.getTime() + parseInt(formData.durationInMinutes) * 60000);
    
                const [openH, openM] = workingHours.openHour.split(":").map(Number);
                const [closeH, closeM] = workingHours.closeHour.split(":").map(Number);
    
                const openTime = new Date(formData.startDate);
                openTime.setHours(openH, openM, 0, 0);
    
                const closeTime = new Date(formData.startDate);
                closeTime.setHours(closeH, closeM, 0, 0)
        
                if (startLocal < openTime || endLocal > closeTime) {
                    setWorkingHoursError(true);
                    const messageTemplate = dictionary.addActivityPage.workingHoursMessage;
                    const translatedMessage = messageTemplate
                    .replace('{{open}}', workingHours.openHour.slice(0, 5))
                    .replace('{{close}}', workingHours.closeHour.slice(0, 5));
                    setClubHoursMessage(translatedMessage);
                } else {
                    setWorkingHoursError(false);
                    setClubHoursMessage('');
                }
    
                const [trainersResponse, courtsResponse] = await Promise.all([
                    getAvailableTrainers(startTimeUTC, endTimeUTC),
                    getAvailableCourts(startTimeUTC, endTimeUTC),
                ]);
    
                setAvailableTrainers(trainersResponse || []);
                setAvailableCourts(courtsResponse || []);
    
            } catch (error) {
                console.error('Error fetching availability or working hours:', error);
                setAvailableTrainers([]);
                setAvailableCourts([]);
                setWorkingHoursError(false);
                setClubHoursMessage('');
            }
        };
    
        fetchAvailability();
    }, [formData.startDate, formData.startHour, formData.durationInMinutes]);
    
    
    const validateForm = () => {
        let isValid = true;
    
        if (!formData.sportActivityName || formData.sportActivityName.trim() === "" || formData.sportActivityName.length > 100) {
            isValid = false;
            setActivityNameError(true);
        } else {
            setActivityNameError(false);
        }
    
        if (!formData.startDate) {
            isValid = false;
            setStartDateError(true);
        } else {
            const selectedDate = new Date(formData.startDate);
            const today = new Date();
            today.setHours(0, 0, 0, 0);
        
            if (selectedDate <= today) {
                isValid = false;
                setStartDateError(true);
            } else {
                setStartDateError(false);
            }
        }
    
        if (!formData.startHour || formData.startHour.trim() === "") {
            isValid = false;
            setStartHourError(true);
        } else {
            setStartHourError(false);
        }
    
        const duration = parseInt(formData.durationInMinutes);
        if (isNaN(duration) || duration <= 0) {
            isValid = false;
            setDurationInMinutesError(true);
        } else {
            setDurationInMinutesError(false);
        }
    
        if (!formData.levelName) {
            isValid = false;
            setLevelNameError(true);
        } else {
            setLevelNameError(false);
        }
    
        const trainerId = parseInt(formData.employeeId);
        if (isNaN(trainerId)) {
            isValid = false;
            setTrainerIdError(true);
        } else {
            setTrainerIdError(false);
        }
    
        const participants = parseInt(formData.participantLimit);
        if (isNaN(participants) || participants <= 0) {
            isValid = false;
            setParticipantsCountError(true);
        } else {
            setParticipantsCountError(false);
        }
    
        if (!formData.courtName) {
            isValid = false;
            setCourtNameError(true);
        } else {
            setCourtNameError(false);
        }
    
        const costWithout = parseFloat(formData.costWithoutEquipment);
        if (isNaN(costWithout) || costWithout <= 0) {
            isValid = false;
            setCostWithoutEquipmentError(true);
        } else {
            setCostWithoutEquipmentError(false);
        }
    
        const costWith = parseFloat(formData.costWithEquipment);
        if (isNaN(costWith) || costWith <= 0) {
            isValid = false;
            setCostWithEquipmentError(true);
        } else {
            setCostWithEquipmentError(false);
        }
    
        return isValid;
    };

    const handleError = (message) => {
        setErrorMessage(message);
        setOpenModal(true);
    };
    
    const navigate = useNavigate();

    function handleCancel() {
        navigate(-1);
    }

    function getWeekdayName(dateString) {
        const days = ['niedziela', 'poniedzialek', 'wtorek', 'sroda', 'czwartek', 'piatek', 'sobota'];
        const [year, month, day] = dateString.split('-').map(Number);
        const date = new Date(year, month - 1, day);
        return days[date.getDay()];
      }
      
      const handleChange = (e) => {
        const { name, value } = e.target;
      
        setFormData((prev) => {
          const updatedData = {
            ...prev,
            [name]: value
          };
      
          if (name === "startDate" && value) {
            const weekday = getWeekdayName(value);
            console.log(weekday);
            updatedData.dayOfWeek = weekday;
          }
      
          return updatedData;
        });
      };
      

      function getErrorMessage(errorCode, dictionary) {
        return dictionary.addActivityPage.errors?.[errorCode] || dictionary.errors?.GENERIC_ERROR;
      }      
    
    const handleSubmit = async (e) => {
        e.preventDefault();
    
        if (!validateForm()) {
          return;
        }

         try {
              const response = await addActivity(formData);
        
              if (!response.ok) {
                const errorData = await response.json();                    
                const message = getErrorMessage(errorData.errorCode, dictionary);
                handleError(message);
              } else {
                navigate('/trainings');
              }
        
            } catch (error) {
                console.error('Błąd dodawania:', error);
                handleError(getErrorMessage('GENERIC_ERROR', dictionary));
            }
          };
        
  return (
    <>
      <GreenBackground height={"80vh"} marginTop={"2vh"}>
        <Header>{dictionary.addActivityPage.title}</Header>
        <OrangeBackground width="70%">
          <form onSubmit={handleSubmit}>
            <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
            <CustomInput
                label={dictionary.addActivityPage.sportActivityNameLabel}
                type="text"
                id="sportActivityName"
                name="sportActivityName"
                fullWidth
                value={formData.sportActivityName}
                onChange={handleChange}
                error={activityNameError}
                helperText={activityNameError ? dictionary.addActivityPage.sportActivityNameError : ""}
                required
                size="small"
            />
            <CustomInput
                label={dictionary.addActivityPage.startDateLabel}
                type="date"
                id="startDate"
                name="startDate"
                fullWidth
                value={formData.startDate}
                onChange={handleChange}
                error={startDateError}
                helperText={startDateError ? dictionary.addActivityPage.startDateError : ""}
                inputProps={{
                    min: new Date().toISOString().split("T")[0],
                  }}
                required
                size="small"
                InputLabelProps={{ shrink: true }}
            />
            <CustomInput
                label={dictionary.addActivityPage.startHourLabel}
                id="startHour"
                name="startHour"
                value={formData.startHour}
                onChange={handleChange}
                error={startHourError}
                helperText={startHourError ? dictionary.addActivityPage.startHourError : ""}
                required
                size="small"
                InputLabelProps={{ shrink: true }}
            />
            {workingHoursError && (
                <p style={{ color: 'red' }}>
                    {clubHoursMessage}
                </p>
            )}
            <CustomInput
                label={dictionary.addActivityPage.durationInMinutesLabel}
                type="number"
                id="durationInMinutes"
                name="durationInMinutes" 
                fullWidth
                value={formData.durationInMinutes}
                onChange={handleChange}
                error={durationInMinutesError}
                helperText={durationInMinutesError ? dictionary.addActivityPage.durationInMinutesError : ""}
                required
                size="small"
                inputProps={{ min: 1 }} 
            />
            <CustomInput
                select
                label={dictionary.addActivityPage.levelNameLabel}
                type="text"
                id="levelName"
                name="levelName"
                fullWidth
                value={formData.levelName}
                onChange={handleChange}
                error={levelNameError}
                helperText={levelNameError ? dictionary.addActivityPage.levelNameError : ""}
                required
                size="small"
                SelectProps={{
                    sx: { textAlign: 'left' }
                  }}
                >
                <MenuItem value="">
                </MenuItem>
                    {levels.map(lvl => (
                <MenuItem key={lvl.levelId} value={lvl.levelName}>
                    {lvl.levelName}
                </MenuItem>
                ))}
            </CustomInput>
            <CustomInput
                select
                label={dictionary.addActivityPage.employeeIdLabel}
                id="employeeId"
                name="employeeId"
                value={formData.employeeId}
                onChange={handleChange}
                error={trainerIdError}
                helperText={trainerIdError ? dictionary.addActivityPage.employeeIdError : ""}
                required
                size="small"
                fullWidth
                SelectProps={{
                    sx: { textAlign: 'left' }
                }}
            >
            <MenuItem value=""></MenuItem>
                {availableTrainers.length === 0 ? (
            <MenuItem disabled>
                {dictionary.addActivityPage.noAvailableTrainers}
            </MenuItem>
            ) : (
                availableTrainers.map(emp => (
                <MenuItem key={emp.id} value={emp.id}>
                    {emp.fullName}
                 </MenuItem>
                ))
            )}
            </CustomInput>
            <CustomInput
                label={dictionary.addActivityPage.participantLimitLabel}
                type="number"
                id="participantLimit"
                name="participantLimit"
                fullWidth
                value={formData.participantLimit}
                onChange={handleChange}
                error={participantsCountError}
                helperText={participantsCountError ? dictionary.addActivityPage.participantLimitError : ""}
                required
                size="small"
                inputProps={{ min: 1 }} 
            />
            <CustomInput
                select
                label={dictionary.addActivityPage.courtNameLabel}
                type="text"
                id="courtName"
                name="courtName"
                fullWidth
                value={formData.courtName}
                onChange={handleChange}
                error={courtNameError}
                helperText={courtNameError ? dictionary.addActivityPage.courtNameError : ""}
                required
                size="small"
                SelectProps={{
                    sx: { textAlign: 'left' }
                  }}
            >
                <MenuItem value=""></MenuItem>
                    {availableCourts.length === 0 ? (
                <MenuItem disabled>
                    {dictionary.addActivityPage.noAvailableCourts}
                </MenuItem>
                ) : (
                    availableCourts.map(court => (
                    <MenuItem key={court.id} value={court.name}>
                        {court.name}
                    </MenuItem>
                    ))
                )}
            </CustomInput>
            <CustomInput
                label={dictionary.addActivityPage.costWithoutEquipmentLabel}
                type="number"
                id="costWithoutEquipment"
                name="costWithoutEquipment"
                fullWidth
                value={formData.costWithoutEquipment}
                onChange={handleChange}
                error={costWithoutEquipmentError}
                helperText={costWithoutEquipmentError ? dictionary.addActivityPage.costWithoutEquipmentError : ""}
                required
                size="small"
                inputProps={{ min: 1 }}
            />
            <CustomInput
                label={dictionary.addActivityPage.costWithEquipmentLabel}
                type="number"
                id="costWithEquipment"
                name="costWithEquipment"
                fullWidth
                value={formData.costWithEquipment}
                onChange={handleChange}
                error={costWithEquipmentError}
                helperText={costWithEquipmentError ? dictionary.addActivityPage.costWithEquipmentError : ""}
                required
                size="small"
                inputProps={{ min: 1 }}
            />
            <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'center', gap: '1rem' }}>
                <GreenButton
                    onClick={handleCancel}
                    style={{ backgroundColor: '#F46C63' }}
                    hoverBackgroundColor="#c3564f"
                >
                {dictionary.addActivityPage.returnLabel}
                </GreenButton>
                <GreenButton type="submit">{dictionary.addActivityPage.confirmLabel}</GreenButton>
            </Box>
            </Box>
          </form>
        </OrangeBackground>
      </GreenBackground>
      <ErrorModal open={openModal} onClose={() => setOpenModal(false)} errorMessage={errorMessage} />
    </>
  );
}

export default AddSportActivity;
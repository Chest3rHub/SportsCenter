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
import getTrainers from '../api/getTrainers';
import getCourts from '../api/getCourts';


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
      
    const [dayOfWeekError, setDayOfWeekError] = useState(false);

    const [startHourError, setStartHourError] = useState(false); 

    const [durationInMinutesError, setDurationInMinutesError] = useState(false); 

    const [levelNameError, setLevelNameError] = useState(false);

    const [trainerIdError, setTrainerIdError] = useState(false);

    const [participantsCountError, setParticipantsCountError] = useState(false);

    const [courtNameError, setCourtNameError] = useState(false);

    const [costWithoutEquipmentError, setCostWithoutEquipmentError] = useState(false);

    const [costWithEquipmentError, setCostWithEquipmentError] = useState(false);

    const [trainers, setTrainers] = useState([]);
    const [courts, setCourts] = useState([]);

    useEffect(() => {
        async function fetchData() {
          try {
            const [trainersData, courtsData, levelsData] = await Promise.all([
              getTrainers(),
              getCourts(),
              //getLevels()
            ]);
            setTrainers(trainersData);
            setCourts(courtsData);
            //setLevels(levelsData);
          } catch (error) {
            console.error('Error loading data:', error);
          }
        }
      
        fetchData();
      }, []);
    
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
            setStartDateError(false);
        }
    
        if (!formData.dayOfWeek) {
            isValid = false;
            setDayOfWeekError(true);
        } else {
            setDayOfWeekError(false);
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
    
        if (!formData.levelName || formData.levelName.trim() === "") {
            isValid = false;
            setLevelNameError(true);
        } else {
            setLevelNameError(false);
        }
    
        const trainerId = parseInt(formData.trainerId);
        if (isNaN(trainerId) || trainerId <= 0) {
            isValid = false;
            setTrainerIdError(true);
        } else {
            setTrainerIdError(false);
        }
    
        const participants = parseInt(formData.participantsCount);
        if (isNaN(participants) || participants <= 0) {
            isValid = false;
            setParticipantsCountError(true);
        } else {
            setParticipantsCountError(false);
        }
    
        if (!formData.courtName || formData.courtName.trim() === "") {
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
    

    function handleError(textToDisplay) {
        navigate('/error', {
          state: { message: textToDisplay }
        });
    }
    
    const navigate = useNavigate();

    function handleCancel() {
        navigate(-1);
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
              const response = await addActivity(formData);
        
              if (!response.ok) {
                const errorData = await response.json();
                console.log(errorData);        
                  handleError('Blad dodawnia zajęć... sprawdz konsole');
              } else {
                navigate('/trainings');
              }
        
            } catch (error) {
              console.error('Błąd dodawania:', error);
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
                required
                size="small"
                InputLabelProps={{ shrink: true }}
            />
           <CustomInput
                select
                label={dictionary.addActivityPage.dayOfWeekLabel}
                id="dayOfWeek"
                name="dayOfWeek"
                value={formData.dayOfWeek}
                onChange={handleChange}
                error={dayOfWeekError}
                helperText={dayOfWeekError ? dictionary.addActivityPage.dayOfWeekError : ""}
                required
                size="small"
                fullWidth
                SelectProps={{
                    sx: { textAlign: 'left' }
                  }}
            >
                <MenuItem value="Monday">{dictionary.addActivityPage.monday}</MenuItem>
                <MenuItem value="Tuesday">{dictionary.addActivityPage.tuesday}</MenuItem>
                <MenuItem value="Wednesday">{dictionary.addActivityPage.wednesday}</MenuItem>
                <MenuItem value="Thursday">{dictionary.addActivityPage.thursday}</MenuItem>
                <MenuItem value="Friday">{dictionary.addActivityPage.friday}</MenuItem>
                <MenuItem value="Saturday">{dictionary.addActivityPage.saturday}</MenuItem>
                <MenuItem value="Sunday">{dictionary.addActivityPage.sunday}</MenuItem>
            </CustomInput>
            <CustomInput
                label={dictionary.addActivityPage.startHourLabel}
                type="time"
                id="startHour"
                name="startHour"
                fullWidth
                value={formData.startHour}
                onChange={handleChange}
                error={startHourError}
                helperText={startHourError ? dictionary.addActivityPage.startHourError : ""}
                required
                size="small"
                InputLabelProps={{ shrink: true }}
            />
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
            />
             <CustomInput
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
            />
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
            <MenuItem value="">
            </MenuItem>
                {trainers.map(emp => (
            <MenuItem key={emp.id} value={emp.id}>
                {emp.fullName}
            </MenuItem>
            ))}
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
                <MenuItem value="">
                </MenuItem>
                    {courts.map(court => (
                <MenuItem key={court.id} value={court.id}>
                    {court.name}
                </MenuItem>
                ))}
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
    </>
  );
}

export default AddSportActivity;
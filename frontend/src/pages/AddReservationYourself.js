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
import getTrainers from '../api/getTrainers';
import getAvailableTrainers from '../api/getAvailableTrainers';
import getAvailableCourts from '../api/getAvailableCourts';
import getCourts from '../api/getCourts';
import MenuItem from '@mui/material/MenuItem';
import DatePicker, { registerLocale } from "react-datepicker"; // npm install react-datepicker date-fns
import 'react-datepicker/dist/react-datepicker.css';   
import { 
  format, 
  startOfDay, 
  endOfDay, 
  addHours, 
  isBefore, 
  isAfter, 
  eachHourOfInterval, 
  differenceInWeeks, 
  startOfWeek, 
  isSameDay,
  isSameMinute,
  setHours,
  setMinutes,
  addMinutes,
} from 'date-fns';
import getClubWorkingHours from '../api/getClubWorkingHours';
import getCourtEvents from '../api/getCourtEvents';
import { pl } from 'date-fns/locale';

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

    const [trainers, setTrainers] = useState([]);
    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [courts, setCourts] = useState([]);
    const [workingHours, setWorkingHours] = useState([]);
    const [existingReservations, setExistingReservations] = useState([]);
    const [excludedTimes, setExcludedTimes] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [workingHoursError, setWorkingHoursError] = useState(false);
    const [clubHoursMessage, setClubHoursMessage] = useState('');

    useEffect(() => {
        registerLocale('pl', pl);
    }, []);
    
    useEffect(() => {
        async function fetchData() {
            setIsLoading(true);
            try {
                const [trainersData, courtsData] = await Promise.all([
                    getTrainers(),
                    getCourts()
                ]);
                setTrainers(trainersData);
                setAvailableTrainers(trainersData);
                setCourts(courtsData);
                await fetchWorkingHours(0);
            } catch (error) {
                console.error('Błąd podczas pobierania trenerów lub kortów:', error);
            } finally {
                setIsLoading(false);
            }
        }
        fetchData();
    }, []);

    useEffect(() => {
        if (formData.courtId && formData.startTime) {
            const startDate = new Date(formData.startTime);
            fetchReservations(formData.courtId, startDate);
            fetchWorkingHoursForDate(startDate);
        }
    }, [formData.courtId, formData.startTime]);

    useEffect(() => {
        if (workingHours.length > 0 && formData.startTime) {
            calculateExcludedTimes();
        }
    }, [workingHours, existingReservations, formData.startTime]);

    useEffect(() => {
        const fetchAvailableTrainers = async () => {
            if (!formData.startTime || !formData.endTime) return;

            try {
                const localStart = new Date(formData.startTime);
                const localEnd = new Date(formData.endTime);
                const toUTCTimeStringSameHours = (date) => {
                    const year = date.getFullYear();
                    const month = String(date.getMonth() + 1).padStart(2, '0');
                    const day = String(date.getDate()).padStart(2, '0');
                    const hours = String(date.getHours()).padStart(2, '0');
                    const minutes = String(date.getMinutes()).padStart(2, '0');
                    const seconds = String(date.getSeconds()).padStart(2, '0');
                    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}Z`;
                };
                const startUTC = toUTCTimeStringSameHours(localStart);
                const endUTC = toUTCTimeStringSameHours(localEnd);
                const trainersResponse = await getAvailableTrainers(startUTC, endUTC);
                setAvailableTrainers(trainersResponse.length ? trainersResponse : []);
            } catch (error) {
                console.error('Error fetching available trainers:', error);
                setAvailableTrainers([]);
            }
        };

        fetchAvailableTrainers();
    }, [formData.startTime, formData.endTime]);
     

    const fetchWorkingHours = async (weekOffset) => {
        try {
            const response = await getClubWorkingHours(weekOffset);
            if (response.ok) {
                const data = await response.json();
                
                const enhancedData = data.map(day => ({
                    ...day,
                    dayOfWeek: format(new Date(day.date), 'EEEE', { locale: pl }).toLowerCase()
                }));
                
                setWorkingHours(enhancedData);
            }
        } catch (error) {
            console.error('Error fetching working hours:', error);
        }
    };

    const fetchWorkingHoursForDate = async (date) => {
        const today = new Date();
        const selected = new Date(date);
        const diff = Math.floor((selected - today) / (1000 * 60 * 60 * 24));
        const weekOffset = Math.floor(diff / 7);
        await fetchWorkingHours(weekOffset);
    };

    const fetchReservations = async (courtId, date) => {
        try {
            const response = await getCourtEvents(courtId, date);
            if (response.ok) {
                const data = await response.json();
                setExistingReservations(data);
            }
        } catch (error) {
            console.error('Error fetching resrvations:', error);
        }
    };

    const calculateExcludedTimes = () => {
        if (!formData.startTime || !workingHours.length) return;
    
        const selectedDate = new Date(formData.startTime);
        const dateStr = format(selectedDate, 'yyyy-MM-dd');
        const dayWorkingHours = workingHours.find(day => day.date === dateStr);
    
        if (!dayWorkingHours) {
            setExcludedTimes([]);
            return;
        }
    
        const [openHour, openMinute] = dayWorkingHours.openHour.split(':').map(Number);
        const [closeHour, closeMinute] = dayWorkingHours.closeHour.split(':').map(Number);
        const openTime = setMinutes(setHours(startOfDay(selectedDate), openHour), openMinute);
        const closeTime = setMinutes(setHours(startOfDay(selectedDate), closeHour), closeMinute);
    
        const allSlots = [];
        let currentTime = startOfDay(selectedDate);
        while (currentTime <= endOfDay(selectedDate)) {
            allSlots.push(new Date(currentTime));
            currentTime = addMinutes(currentTime, 30);
        }
    
        const excludedWorkingHours = allSlots.filter(slot => 
            isBefore(slot, openTime) || isAfter(slot, closeTime)
        );
    
        const excludedReservationTimes = existingReservations.flatMap(reservation => {
            const start = new Date(reservation.startTime);
            const end = new Date(reservation.endTime);
            
            const slots = [];
            let time = start;
            while (time < end) {
                slots.push(new Date(time));
                time = addMinutes(time, 30);
            }
            return slots;
        });
    
        setExcludedTimes([...new Set([...excludedWorkingHours, ...excludedReservationTimes])]);
    };

    const getValidEndTimes = () => {
        if (!formData.startTime) return [];
      
        const start = new Date(formData.startTime);
        const dateStr = format(start, 'yyyy-MM-dd');
        const dayWorkingHours = workingHours.find(day => day.date === dateStr);
        
        if (!dayWorkingHours) return [];

        const [openHour, openMinute] = dayWorkingHours.openHour.split(':').map(Number);
        const [closeHour, closeMinute] = dayWorkingHours.closeHour.split(':').map(Number);
        const openTime = setMinutes(setHours(startOfDay(start), openHour), openMinute);
        const closeTime = setMinutes(setHours(startOfDay(start), closeHour), closeMinute);

        const nextReservation = existingReservations
          .filter(res => {
            const resStart = new Date(res.startTime);
            return format(resStart, 'yyyy-MM-dd') === dateStr && 
                   isAfter(resStart, start);
          })
          .sort((a, b) => new Date(a.startTime) - new Date(b.startTime))[0];
      
        const maxEndTime = nextReservation 
          ? new Date(nextReservation.startTime)
          : closeTime;

        const validTimes = [];
        let currentTime = addMinutes(start, 30);
        const endTime = isBefore(maxEndTime, closeTime) ? maxEndTime : closeTime;
      
        while (isBefore(currentTime, endTime)) {
          validTimes.push(new Date(currentTime));
          currentTime = addMinutes(currentTime, 30);
        }
      
        return validTimes;
      };


    const filterPassedTime = (time) => {
        const currentDate = new Date();
        
        if (isSameDay(time, currentDate)) {
            if (isBefore(time, currentDate)) return false;
        }
    
        const dateStr = format(time, 'yyyy-MM-dd');
        const dayWorkingHours = workingHours.find(day => day.date === dateStr);
        
        if (dayWorkingHours) {
            const [openHour, openMinute] = dayWorkingHours.openHour.split(':').map(Number);
            const [closeHour, closeMinute] = dayWorkingHours.closeHour.split(':').map(Number);
            const openTime = setMinutes(setHours(startOfDay(time), openHour), openMinute);
            const closeTime = setMinutes(setHours(startOfDay(time), closeHour), closeMinute);
            
            if (isBefore(time, openTime) || isAfter(time, closeTime)) {
                return false;
            }
        }
        return true;
    };

    const handleStartTimeChange = (date) => {
        if (!date) return;
        const dateStr = format(date, 'yyyy-MM-dd');
        const dayWorkingHours = workingHours.find(day => day.date === dateStr);
        
        if (dayWorkingHours) {
            const [openHour, openMinute] = dayWorkingHours.openHour.split(':').map(Number);
            const [closeHour, closeMinute] = dayWorkingHours.closeHour.split(':').map(Number);
            const openTime = setMinutes(setHours(startOfDay(date), openHour), openMinute);
            const closeTime = setMinutes(setHours(startOfDay(date), closeHour), closeMinute);
            
            if (isBefore(date, openTime) || isAfter(date, closeTime)) {
                if (isBefore(date, openTime)) {
                    date = new Date(openTime);
                } else {
                    date = new Date(closeTime);
                }
            }
        }
        
        setFormData(prev => ({
            ...prev,
            startTime: date,
            endTime: date,
            trainerId: ''
        }));
    };

    const handleEndTimeChange = (date) => {
        if (!date) return;
        const minutes = date.getMinutes();
        const roundedDate = new Date(date);
        roundedDate.setMinutes(minutes < 30 ? 0 : 30, 0, 0);
        setFormData(prev => ({
          ...prev,
          endTime: roundedDate,
          trainerId: ''
        }));
      };

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));
    };

    const navigate = useNavigate();
    
    function handleError(textToDisplay) {
      navigate('/error', {
        state: { message: textToDisplay }
      });
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

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateForm()) return;
    
        const dataToSend = {
            ...formData,
            startTime: format(formData.startTime, "yyyy-MM-dd'T'HH:mm:ss"),
            endTime: format(formData.endTime, "yyyy-MM-dd'T'HH:mm:ss"),
            creationDate: format(new Date(), "yyyy-MM-dd'T'HH:mm:ss")
        };
    
    
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
        <style>
            {`
                .custom-datepicker-input {
                    width: 100%;
                    padding: 10px 14px;
                    border: 1px solid #ccc;
                    border-radius: 16px;
                    font-size: 16px;
                    background-color: #fff;
                    transition: border-color 0.3s;
                    font-family: inherit;
                    height: 40px;
                    box-sizing: border-box;
                }
                .custom-datepicker-input:focus {
                    border-color:#1976D2;
                    outline: none;
                }
                .custom-datepicker-input:disabled {
                    background-color: #f5f5f5;
                    cursor: not-allowed;
                }
                .custom-datepicker-calendar {
                    font-family: inherit;
                    border: 1px solid #ccc;
                    border-radius: 16px;
                    box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                }
                .custom-datepicker-calendar .react-datepicker__header {
                    background-color: #f5f5f5;
                    border-bottom: 1px solid #ccc;
                    border-radius: 16px 16px 0 0;
                }
                .custom-time-slot {
                    padding: 10px 15px;
                    font-size: 15px;
                }
                .custom-time-slot:hover {
                    background-color: #f5f5f5 !important;
                }
                .react-datepicker__time-list-item--selected {
                    background-color:#AFEBBC !important;
                    color: white !important;
                }
                .react-datepicker__time-container {
                    width: 120px;
                }
                .react-datepicker__day--selected,
                .react-datepicker__day--in-selecting-range,
                .react-datepicker__day--in-range,
                .react-datepicker__time-list-item--selected {
                    background-color: #AFEBBC !important;
                    color: #000 !important;
                }
            `}
        </style>

        <GreenBackground height={"80vh"} marginTop={"2vh"}>
            <Header>{dictionary.addReservationYourselfPage.title}</Header>
            <OrangeBackground width="70%">
                <form onSubmit={handleSubmit}>
                    <Box sx={{ display: "flex", flexDirection: "column", gap: "1.5rem", marginBottom: "2vh" }}>
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
                                sx: { 
                                    textAlign: 'left',
                                    borderRadius: '8px'
                                }
                            }}
                        >
                            <MenuItem value=""></MenuItem>
                            {courts.map(court => (
                                <MenuItem key={court.id} value={court.id}>
                                    {court.name}
                                </MenuItem>
                            ))}
                        </CustomInput>

                        <div style={{
                            width: '100%',
                            marginBottom: '8px'
                        }}>
                            <label style={{
                                display: 'block',
                                marginBottom: '8px',
                                fontSize: '16px',
                                color: 'rgba(0, 0, 0, 0.87)'
                            }}>
                                {dictionary.addReservationYourselfPage.startTimeLabel}
                            </label>
                            <DatePicker
                                selected={formData.startTime}
                                onChange={handleStartTimeChange}
                                showTimeSelect
                                timeFormat="HH:mm"
                                timeIntervals={30}
                                dateFormat="Pp"
                                minDate={new Date()}
                                locale="pl"
                                filterTime={filterPassedTime}
                                excludeTimes={excludedTimes}
                                placeholderText={dictionary.addReservationYourselfPage.chooseStartTimeLabel}
                                disabled={!formData.courtId}
                                className="custom-datepicker-input"
                                calendarClassName="custom-datepicker-calendar"
                                timeClassName={() => "custom-time-slot"}
                                required
                            />
                            {startTimeError && (
                                <div style={{ color: 'red', fontSize: '0.8rem', marginTop: '4px' }}>
                                    {dictionary.addReservationYourselfPage.startTimeError}
                                </div>
                            )}
                        </div>

                        <div style={{
                            width: '100%',
                            marginBottom: '8px'
                        }}>
                            <label style={{
                                display: 'block',
                                marginBottom: '8px',
                                fontSize: '16px',
                                color: 'rgba(0, 0, 0, 0.87)'
                            }}>
                                {dictionary.addReservationYourselfPage.endTimeLabel}
                            </label>
                            <DatePicker
                                selected={formData.endTime}
                                onChange={handleEndTimeChange}
                                showTimeSelect
                                timeFormat="HH:mm"
                                timeIntervals={30}
                                dateFormat="Pp"
                                minDate={formData.startTime || new Date()}
                                locale="pl"
                                filterTime={(time) => {
                                    if (!formData.startTime) return false;
                                    const minEndTime = addMinutes(new Date(formData.startTime), 30);
                                    if (isBefore(time, minEndTime)) return false;
                                    const validTimes = getValidEndTimes();
                                    return validTimes.some(validTime => isSameMinute(validTime, time));
                                  }}
                                excludeTimes={excludedTimes}
                                placeholderText={dictionary.addReservationYourselfPage.chooseEndTimeLabel}
                                disabled={!formData.courtId}
                                className="custom-datepicker-input"
                                calendarClassName="custom-datepicker-calendar"
                                timeClassName={() => "custom-time-slot"}
                                required
                            />
                            {endTimeE && (
                                <div style={{ color: 'red', fontSize: '0.8rem', marginTop: '4px' }}>
                                    {endTimeErrorMessage}
                                </div>
                            )}
                        </div>
    
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
                            disabled={!formData.startTime || !formData.endTime}
                        >
                            <MenuItem value=""></MenuItem>
                            {availableTrainers.map(trainer => (
                                <MenuItem key={trainer.id} value={trainer.id}>
                                    {trainer.fullName}
                                </MenuItem>
                            ))}
                        </CustomInput>
    
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
                            inputProps={{ min: 1 }}
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
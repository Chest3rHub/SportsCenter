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
  addDays
} from 'date-fns';
import getClubWorkingHours from '../api/getClubWorkingHours';
import getCourtEvents from '../api/getCourtEvents';
import { pl } from 'date-fns/locale';
import getTrainerBusyTimes from "../api/getTrainerBusyTimes";

function MoveReservation() {
    const location = useLocation();
    const { id, courtId, startTime, endTime, trainerId: reservationTrainerId, offsetFromLocation } = location.state || {};
    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        newStartTime: new Date(startTime),
        newEndTime: new Date(endTime)
    });

    const [newStartTimeError, setNewStartTimeError] = useState(false);
    const [newStartTimeTooSoonError, setNewStartTimeTooSoonError] = useState(false);
    const [newEndTimeError, setNewEndTimeError] = useState(false); 
    const [newTimeDurationError, setNewTimeDurationError] = useState(false); 

    const [openSuccessBackdrop, setOpenSuccessBackdrop] = useState(false);
    const [openFailureBackdrop, setOpenFailureBackdrop] = useState(false);

    const [workingHours, setWorkingHours] = useState([]);
    const [existingReservations, setExistingReservations] = useState([]);
    const [excludedTimes, setExcludedTimes] = useState([]);
    const [trainerBusyTimes, setTrainerBusyTimes] = useState([]);

  
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

        const diffToStartHours = (start - now) / (1000 * 60 * 60); 

        if (diffToStartHours < 24) {
            setNewStartTimeTooSoonError(true);
            isValid = false;
        } else {
            setNewStartTimeTooSoonError(false);
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
    };


    function handleError(textToDisplay) {
        handleOpenFailure();
    }

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

    useEffect(() => {
        if (courtId && formData.newStartTime) {
            const startDate = new Date(formData.newStartTime);
            fetchReservations(courtId, startDate);
            fetchWorkingHoursForDate(startDate);
        }
    }, [courtId, formData.newStartTime]);

    useEffect(() => {
        if (workingHours.length > 0 && formData.newStartTime) {
            calculateExcludedTimes();
        }
    }, [workingHours, existingReservations, formData.newStartTime]);

    useEffect(() => {
        if (reservationTrainerId && formData.newStartTime) {
            getTrainerBusyTimes(reservationTrainerId, new Date(formData.newStartTime))
                .then(data => setTrainerBusyTimes(data))
            .   catch(err => console.error(err));
        }
    }, [reservationTrainerId, formData.newStartTime]);


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
        const todayCorrect = startOfDay(today);
        const selectedCorrect = startOfDay(selected);
        const todayDayOfWeek = todayCorrect.getDay();
        const selectedDayOfWeek = selectedCorrect.getDay();
        let weekOffset;
        if (todayDayOfWeek === 0) {
            const todayAdjusted = addDays(todayCorrect, 1);
            const startOfCurrentWeek = startOfWeek(todayAdjusted, { weekStartsOn: 1 });
            const startOfSelectedWeek = startOfWeek(selectedCorrect, { weekStartsOn: 1 });
            weekOffset = differenceInWeeks(startOfSelectedWeek, startOfCurrentWeek);
        } else {
            const startOfCurrentWeek = startOfWeek(todayCorrect, { weekStartsOn: 1 });
            const startOfSelectedWeek = startOfWeek(selectedCorrect, { weekStartsOn: 1 });
            weekOffset = differenceInWeeks(startOfSelectedWeek, startOfCurrentWeek);
        }
         
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
        if (!formData.newStartTime || !workingHours.length) return;

        const selectedDate = new Date(formData.newStartTime);
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

        const excludedReservationTimes = existingReservations
            .filter(reservation => reservation.eventId !== id && reservation.isReservation)
            .flatMap(reservation => {
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

        const excludedTrainerTimes = trainerBusyTimes
            .filter(period => {
                const periodStart = new Date(period.startTime).getTime();
                const periodEnd = new Date(period.endTime).getTime();
                const currentResStart = new Date(startTime).getTime();
                const currentResEnd = new Date(endTime).getTime();

            return !(periodStart === currentResStart && periodEnd === currentResEnd);
            })
            .flatMap(period => {
                const start = new Date(period.startTime);
                const end = new Date(period.endTime);
                const slots = [];
                let time = start;
                while (time < end) {
                    slots.push(new Date(time));
                    time = addMinutes(time, 30);
                }
                return slots;
            });


            const allExcluded = [...excludedWorkingHours, ...excludedReservationTimes, ...excludedTrainerTimes];

            setExcludedTimes([...new Set(allExcluded.map(d => d.getTime()))].map(t => new Date(t)));
        };

    const getValidEndTimes = () => {
        if (!formData.newStartTime) return [];
      
        const start = new Date(formData.newStartTime);
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
            return res.eventId !== id && 
                format(resStart, 'yyyy-MM-dd') === dateStr && 
                isAfter(resStart, start);
        })
        .sort((a, b) => new Date(a.startTime) - new Date(b.startTime))[0];

      
        const maxEndTime = nextReservation 
          ? new Date(nextReservation.startTime)
          : closeTime;

        const validTimes = [];
        let currentTime = addMinutes(start, 30);
        const endTime = isBefore(maxEndTime, closeTime) ? maxEndTime : closeTime;
      
        while (isBefore(currentTime, endTime) || isSameMinute(currentTime, endTime)) {
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
            newStartTime: date,
            newEndTime: date,
        }));
    };

    const handleEndTimeChange = (date) => {
        if (!date) return;
        const minutes = date.getMinutes();
        const roundedDate = new Date(date);
        roundedDate.setMinutes(minutes < 30 ? 0 : 30, 0, 0);
        setFormData(prev => ({
          ...prev,
          newEndTime: roundedDate
        }));
    };

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

        <GreenBackground height={"55vh"} marginTop={"2vh"}>
            <Header>{dictionary.moveReservation.title}</Header>
            <OrangeBackground width="70%">
                <form onSubmit={handleSubmit}>
                <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
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
                                {dictionary.moveReservation.newStartTimeLabel}
                            </label>
                            <DatePicker
                                selected={formData.newStartTime}
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
                                disabled={!courtId}
                                className="custom-datepicker-input"
                                calendarClassName="custom-datepicker-calendar"
                                timeClassName={() => "custom-time-slot"}
                                required
                            />
                            {newStartTimeError && (
                                <div style={{ color: 'red', fontSize: '0.8rem', marginTop: '4px' }}>
                                    {dictionary.addReservationYourselfPage.newStartTimeError}
                                </div>
                            )}

                            {newStartTimeTooSoonError && (
                                <div style={{ color: 'red', fontSize: '0.8rem', marginTop: '4px' }}>
                                    {dictionary.moveReservation.newStartTimeTooSoonError}
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
                                {dictionary.moveReservation.newEndTimeLabel}
                            </label>
                            <DatePicker
                                selected={formData.newEndTime}
                                onChange={handleEndTimeChange}
                                showTimeSelect
                                timeFormat="HH:mm"
                                timeIntervals={30}
                                dateFormat="Pp"
                                minDate={formData.newStartTime || new Date()}
                                locale="pl"
                                filterTime={(time) => {
                                    if (!formData.newStartTime) return false;
                                    const minEndTime = addMinutes(new Date(formData.newStartTime), 30);
                                    if (isBefore(time, minEndTime)) return false;
                                    const validTimes = getValidEndTimes();
                                    return validTimes.some(validTime => isSameMinute(validTime, time));
                                  }}
                                placeholderText={dictionary.addReservationYourselfPage.chooseEndTimeLabel}
                                disabled={!courtId}
                                className="custom-datepicker-input"
                                calendarClassName="custom-datepicker-calendar"
                                timeClassName={() => "custom-time-slot"}
                                required
                            />
                            {newEndTimeError && (
                                <div style={{ color: 'red', fontSize: '0.8rem', marginTop: '4px' }}>
                                    {newEndTimeError}
                                </div>
                            )}
                        </div>

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
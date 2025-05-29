import { Box, MenuItem, Checkbox, FormControlLabel, Typography } from "@mui/material";
import React, { useState, useContext, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import CustomInput from "../components/CustomInput";
import getCourts from "../api/getCourts";
import CustomTimeInput from "../components/CustomTimeInput";
import generateTime from "../utils/generateTime";
import getTrainers from "../api/getTrainers";
import getClubWorkingHours from '../api/getClubWorkingHours';
import getWorkingHoursForSingleDay from "../utils/getWorkingHoursForSingleDay";
import getEndTimeOptions from "../utils/getEndTimeOptions";
import getAvailableCourts from "../api/getAvailableCourts";
import getAvailableTrainers from "../api/getAvailableTrainers";
import moveReservation from "../api/moveReservation";
import getTrainerBusyTimes from "../api/getTrainerBusyTimes";
import Backdrop from '@mui/material/Backdrop';
import Avatar from '@mui/material/Avatar';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

function MoveReservation() {
    const location = useLocation();
    const { id, courtId, startTime, endTime, trainerId: reservationTrainerId, offsetFromLocation } = location.state || {};
    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();

    const [formData, setFormData] = useState({
        date: '',
        newStartTime: '',
        newEndTime: ''
    });

    const [dateRequiredError, setDateRequiredError] = useState(false);
    const [dateError, setDateError] = useState(false);
    const [courtNotAvailableError, setCourtNotAvailableError] = useState(false);
    const [trainerNotAvailableError, setTrainerNotAvailableError] = useState(false);
    const [startTimeRequiredError, setStartTimeRequiredError] = useState(false);
    const [startTimeError, setStartTimeError] = useState(false);
    const [endTimeRequiredError, setEndTimeRequiredError] = useState(false);
    const [endTimeBeforeStartError, setEndTimeBeforeStartError] = useState(false);
    const [endTimeDurationError, setEndTimeDurationError] = useState(false);
    const [newStartTimeTooSoonError, setNewStartTimeTooSoonError] = useState(false);

    const [openSuccessBackdrop, setOpenSuccessBackdrop] = useState(false);
    const [openFailureBackdrop, setOpenFailureBackdrop] = useState(false);
    const [failedSignUpLabel, setFailedSignUpLabel] = useState('');

    const [courts, setCourts] = useState([]);
    const [trainers, setTrainers] = useState([]);
    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [workingDaysAndHours, setWorkingDaysAndHours] = useState([]);
    const [offset, setOffset] = useState(-1);
    const [courtsError, setCourtsError] = useState('');

    useEffect(() => {
        getCourts()
            .then(response => {
                setCourts(response);
            })
            .then(data => {
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania get courts:', error);
            });
    }, []);
    useEffect(() => {
        getTrainers()
            .then(response => {
                setTrainers(response);
                setAvailableTrainers(response);
            })
            .then(data => {
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania get trainers:', error);
            });
    }, []);
    useEffect(() => {
        getClubWorkingHours(-1)
            .then(response => response.json())
            .then(data => {
                setWorkingDaysAndHours(data);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getClubWorkingHours:', error);
            });
    }, []);

    // jak sie ustawi date zerujemy wszystko poza data
    useEffect(() => {
        setFormData(prev => ({
            date: prev.date,
            newStartTime: '',
            newEndTime: '',
        }));
    }, [formData.date]);

    useEffect(() => {
        setFormData(prev => ({
            ...prev,
            newEndTime: '' 
        }));
    }, [formData.newStartTime]);


    // jesli zmieni sie wybrana data to wyliczany jest dla niej offset i pobierane dane o godzinach pracy 
    // dla tygodnia o podanym offsecie - 1 bo backend zle przesuwa
    useEffect(() => {
        if (formData.date) {

            const startDate = new Date(formData.date);
            getWorkingHoursForSingleDay(startDate).then(weekOffset => {
                console.log(weekOffset)
                getClubWorkingHours(weekOffset - 1)
                    .then(response => response.json())
                    .then(data => {
                        setWorkingDaysAndHours(data);
                    })
                    .catch(error => {
                        console.error('Błąd podczas wywoływania getClubWorkingHours po zmianie daty:', error);
                    });
            });
        }
    }, [formData.date]);

    // gdy sie zmieni godzina zakonczenia to pobieramy dostepne kort w danym przedziale czasowym
    useEffect(() => {
        const startDateTime = `${formData.date}T${formData.newStartTime}`;
        const endDateTime = `${formData.date}T${formData.newEndTime}`;

        getAvailableCourts(startDateTime, endDateTime)
            .then(response => {
                setCourts(response);
                if (!response || response.length === 0) {
                    setCourtsError('Brak dostępnych kortów w podanych godzinach');
                } else {
                    setCourtsError('');
                }
            })
            .then(data => {
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania get courts:', error);
            });
    }, [formData.newEndTime]);

    // pobranie dostepnych trenerow w podanych godzinach czasowych w dany dzien
    useEffect(() => {
        const startDateTime = `${formData.date}T${formData.newStartTime}`;
        const endDateTime = `${formData.date}T${formData.newEndTime}`;

        getAvailableTrainers(startDateTime, endDateTime)
            .then(response => {
                setAvailableTrainers(response);
            })
            .then(data => {
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania get courts:', error);
            });
    }, [formData.newEndTime]);

    // ustawienie dni i godzin pracy na takie jak w zaznaczonym dniu
    const selectedDayWorkingHours = workingDaysAndHours.find(day => day.date === formData.date);
    const openHour = selectedDayWorkingHours ? selectedDayWorkingHours.openHour.slice(0, 5) : "10:00";
    const closeHour = selectedDayWorkingHours ? selectedDayWorkingHours.closeHour.slice(0, 5) : "22:00";

    function isFormValid() {
        let valid = true;
        const { date, newStartTime, newEndTime } = formData;

        const now = new Date();
        const todayStr = now.toISOString().slice(0,10);
        const start = date && newStartTime ? new Date(`${date}T${newStartTime}:00`) : null;
        const end   = date && newEndTime ? new Date(`${date}T${newEndTime}:00`) : null;
        const diffToStartHours = (start - now) / (1000 * 60 * 60); 

        // data z przeszłości
        if (!date) {
            setDateRequiredError(true);
            valid=false;
            if (date < todayStr){
                setDateRequiredError(false);
                setDateError(true);
                valid = false;
            }
        } else {
            setDateRequiredError(false);
            if (date < todayStr){
                setDateError(true);
                valid = false;
            } else {
                setDateError(false);
            }
        }

        // godzina rozpoczęcia z przeszłości
        if (!startTime) {
            setStartTimeRequiredError(true);
            valid = false;
        } else {
            setStartTimeRequiredError(false);
            if (date === todayStr && start < now) {
                setStartTimeError(true);
                valid = false;
            } else {
                setStartTimeError(false);
            }
        }

        // godzina zakończenia wcześniejsza niż godzina rozpoczęcia
        if (!endTime) {
            setEndTimeRequiredError(true);
            valid = false;
        } else {
            setEndTimeRequiredError(false);
            if (start && end <= start) {
                setEndTimeBeforeStartError(true);
                valid = false;
            } else {
                setEndTimeBeforeStartError(false);
            }

            // długość 1–5h
            if (start && end) {
                const diffH = (end - start) / (1000*60*60);
                if (diffH < 1 || diffH > 5) {
                    setEndTimeDurationError(true);
                    valid = false;
                } else {
                    setEndTimeDurationError(false);
                }
            }
        }

        // przesunąć można tylko 24h przed rozpoczęciem
        if (diffToStartHours < 24) {
            setNewStartTimeTooSoonError(true);
            valid = false;
        } else {
            setNewStartTimeTooSoonError(false);
        }

        return valid;
    }

    async function handleSubmit() {
        if (!isFormValid()) {
            return;
        }

        let hasError = false;

        const courtIds = courts.map(c => c.id);
        if (!courtIds.includes(courtId)) {
            //console.log(courtId);
            setCourtNotAvailableError(true);
            hasError = true;
        } else {
            setCourtNotAvailableError(false);
        }

        const trainerIds = availableTrainers.map(t => t.id);
        if (!trainerIds.includes(reservationTrainerId)) {
            //console.log(reservationTrainerId);
            setTrainerNotAvailableError(true);
            hasError = true;
        } else {
            setTrainerNotAvailableError(false);
        }

        if (hasError) {
            return;
        }

        const makeIsoDateTime = (date, time) => `${date}T${time}:00`;

        const payload = {
            ReservationId: Number(id),
            NewStartTime: makeIsoDateTime(formData.date, formData.newStartTime),
            NewEndTime: makeIsoDateTime(formData.date, formData.newEndTime),
        };

        try {
            const response = await moveReservation(payload);

            if (!response.ok) {
                const errorData = await response.json();
                let failText = determineFailTextByResponseCode(response.status);
                setFailedSignUpLabel(failText);
                handleOpenFailure();
            } else {
                handleOpenSuccess();
                setFormData(prev => ({
                    date: '',
                    newStartTime: '',
                    newEndTime: '',
                }));
            }
        } catch (error) {
            console.error('Błąd podczas rezerwacji:', error);
        }
    }

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };
  
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

    function determineFailTextByResponseCode(responseCode) {
        switch (responseCode) {
          case 411:
            return dictionary.moveReservation.reservationNotFound;
          case 412:
            return dictionary.moveReservation.reservationOutsideWorkingHours;
          case 413:
            return dictionary.moveReservation.notThatClientReservation;
          case 414:
            return dictionary.moveReservation.postponeReservationNotAllowed;
          case 415:
            return dictionary.moveReservation.courtNotAvailableError;
          case 416:
            return dictionary.moveReservation.employeeNotFound;
          case 417:
            return dictionary.moveReservation.notTrainerEmployee;
          case 418:
            return dictionary.moveReservation.employeeAlreadyDismissedException;
          case 419:
            return dictionary.moveReservation.trainerNotAvailableError
          case 420:
            return dictionary.moveReservation.alreadyHasActivityLabel;
          default:
            return dictionary.moveReservation.savedFailureLabel;
        }
    }

    function handleError(textToDisplay) {
        handleOpenFailure();
    }

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
                <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>
                    <CustomInput
                        label={dictionary.addReservationYourselfPage.dateLabel}
                        type="date"
                        id="date"
                        name="date"
                        fullWidth
                        value={formData.date}
                        onChange={handleChange}
                        error={dateRequiredError || dateError}
                        helperText={dateRequiredError ? dictionary.moveReservation.dateRequiredError : dateError 
                                                      ? dictionary.moveReservation.dateError : ""}
                        required
                        size="small"
                        InputLabelProps={{
                            shrink: true
                        }}
                        />
                    
                        <Box sx={{
                            display: 'flex',
                            flexDirection: 'row',
                            columnGap: '3vw'
                        }}>
                    
                            <CustomTimeInput
                                select
                                label={dictionary.addReservationYourselfPage.startTimeLabel}
                                id="newStartTime"
                                name="newStartTime"
                                value={formData.newStartTime}
                                onChange={handleChange}
                                error={startTimeRequiredError || startTimeError || newStartTimeTooSoonError}
                                helperText={startTimeRequiredError ? dictionary.moveReservation.newStartTimeError : startTimeError 
                                                                   ? dictionary.moveReservation.newStartTimeError : newStartTimeTooSoonError 
                                                                   ? dictionary.moveReservation.newStartTimeTooSoonError : ""}
                                required
                                size="small"
                                fullWidth
                                SelectProps={{
                                    sx: {
                                        textAlign: 'left',
                                        borderRadius: '8px'
                                    }
                                }}
                                disabled={!formData.date}
                            >
                                <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                                    {generateTime(openHour, closeHour, 30).map((time) => (
                                        <MenuItem key={time} value={time}>
                                            {time}
                                        </MenuItem>
                                    ))}
                                </CustomTimeInput>
                                                    
                                <CustomTimeInput
                                    select
                                    label={dictionary.addReservationYourselfPage.endTimeLabel}
                                    id="newEndTime"
                                    name="newEndTime"
                                    value={formData.newEndTime}
                                    onChange={handleChange}
                                    error={endTimeRequiredError || endTimeBeforeStartError || endTimeDurationError}
                                    helperText={endTimeRequiredError ? dictionary.moveReservation.newEndTimeError : endTimeBeforeStartError
                                                                     ? dictionary.moveReservation.newEndTimeError : endTimeDurationError
                                                                     ? dictionary.moveReservation.newTimeDurationError : ""}
                                    required
                                    size="small"
                                    fullWidth
                                    SelectProps={{
                                        sx: {
                                            textAlign: 'left',
                                            borderRadius: '8px'
                                        }
                                    }}
                                    disabled={!formData.date || !formData.newStartTime}
                                >
                                    <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                                        {getEndTimeOptions({ startTime: formData.newStartTime },closeHour).map((time) => (
                                            <MenuItem key={time} value={time}>
                                                {time}
                                            </MenuItem>
                                        ))}
                                </CustomTimeInput>
                            </Box>

                            {courtNotAvailableError && (
                                <Typography color="error" sx={{ mb: 2, mt: 2 }}>
                                    {dictionary.moveReservation.courtNotAvailableError}
                                </Typography>
                            )}

                            {trainerNotAvailableError && (
                                <Typography color="error" sx={{ mb: 2 }}>
                                    {dictionary.moveReservation.trainerNotAvailableError}
                                </Typography>
                            )}

                    <Box sx={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: 'center',
                        columnGap: "4vw"
                    }}>
                    <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.moveReservation.returnLabel}</GreenButton>
                    <GreenButton type="submit" style={{ maxWidth: "10vw" }} onClick={handleSubmit}>{dictionary.moveReservation.saveLabel}</GreenButton>
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
            </OrangeBackground>

            <Typography sx={{ mt: 4 }}>
                {dictionary.moveReservation.timetableInfo}
            </Typography>

        </GreenBackground>
        </>
    )
}

export default MoveReservation;
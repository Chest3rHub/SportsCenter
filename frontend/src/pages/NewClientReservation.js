import { Box, MenuItem, Checkbox, FormControlLabel, Typography, Avatar } from "@mui/material";
import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
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
import addReservationYourself from "../api/addReservationYourself";
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

export default function NewClientReservation() {
    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();

    const [courts, setCourts] = useState([]);
    const [trainers, setTrainers] = useState([]);
    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [isEquipmentIncluded, setIsEquipmentIncluded] = useState(false);
    const [workingDaysAndHours, setWorkingDaysAndHours] = useState([]);
    const [offset, setOffset] = useState(-1);

    // errory
    const [courtsError, setCourtsError] = useState('');
    const [dateError, setDateError] = useState(false);
    const [startTimeError, setStartTimeError] = useState(false);
    const [participantsError, setParticipantsError] = useState(false);
    const [isSubmitDisabled, setIsSubmitDisabled] = useState(true);
    const MAX_PARTICIPANTS_AMOUNT=8;


    
    const [formData, setFormData] = useState({
        date: '',
        startTime: '',
        endTime: '',
        creationDate: '',
        courtId: '',
        trainerId: '',
        participantsCount: '',
        isEquipmentReserved: false,
    });

    // backdropy
    const [failedSignUpLabel, setFailedSignUpLabel] = useState('');

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

    function removeCourtErrorsAfterChange(){
        setCourtsError('');
    }

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
        getClubWorkingHours(0)
            .then(response => response.json())
            .then(data => {
                setWorkingDaysAndHours(data);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getClubWorkingHours:', error);
            });
    }, []);


    // jak sie ustawi date zerujemy wszystko poza data i walidacja czy nie jest z przeszlosci
    useEffect(() => {

        const today = new Date();
        const selectedDate = new Date(formData.date);
        today.setHours(0, 0, 0, 0);
        selectedDate.setHours(0, 0, 0, 0);

        if (selectedDate < today) {
            setDateError(true);
        } else {
            setDateError(false);
        }
        setFormData(prev => ({
            date: prev.date,
            startTime: '',
            endTime: '',
            creationDate: '',
            courtId: '',
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
        }));
        removeCourtErrorsAfterChange();
    }, [formData.date]);
    // jak sie zmieni czas rozpoczecia usuwamy wszystko poza data i czasem rozpoczecia i wybrany czas nie moze byc z przeszlosci dla dzisiejszego dnia

    useEffect(() => {
        const [startHour, startMinute] = formData.startTime.split(':').map(Number);
        const selectedStart = new Date();
        selectedStart.setHours(startHour, startMinute, 0, 0);

        const now = new Date();

        const selectedDate = new Date(formData.date);
        const today = new Date();
        selectedDate.setHours(0, 0, 0, 0);
        today.setHours(0, 0, 0, 0);

        if (selectedDate.getTime() === today.getTime() && selectedStart < now) {
            setStartTimeError(true); 
        } else {
            setStartTimeError(false);
        }

        setFormData(prev => ({
            date: prev.date,
            startTime: prev.startTime,
            endTime: '',
            creationDate: '',
            courtId: '',
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
        }));
        removeCourtErrorsAfterChange();

    }, [formData.startTime]);
    // jak sie zmieni czas zakonczenia usuwamy wszystko poza data i czasem rozpoczecia zakonczenia
    useEffect(() => {
        setFormData(prev => ({
            date: prev.date,
            startTime: prev.startTime,
            endTime: prev.endTime,
            creationDate: '',
            courtId: '',
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
        }));
        removeCourtErrorsAfterChange();

    }, [formData.endTime]);

    // jak sie zmieni kort usuwamy wszystko poza data, czasami i kortem
    useEffect(() => {
        setFormData(prev => ({
            date: prev.date,
            startTime: prev.startTime,
            endTime: prev.endTime,
            creationDate: '',
            courtId: prev.courtId,
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
        }));
    }, [formData.courtId]);
    // jak sie zmieni trener usuwamy wszystko poza data, czasami kortem i trenerem
    useEffect(() => {
        setFormData(prev => ({
            date: prev.date,
            startTime: prev.startTime,
            endTime: prev.endTime,
            creationDate: '',
            courtId: prev.courtId,
            trainerId: prev.trainerId,
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
        }));
    }, [formData.trainerId]);
    // jak sie zmieni liczba uczestnikow czy informacja o rezerwacji sprzetu to nic nie usuwamy

    // walidacja liczby uczestnikow, max 8
    // jak sie zmieni trener usuwamy wszystko poza data, czasami kortem i trenerem
    useEffect(() => {
        if(formData.participantsCount > MAX_PARTICIPANTS_AMOUNT || (formData.participantsCount && formData.participantsCount < 1 )){
            setParticipantsError(true);
            setIsSubmitDisabled(true);
        } else {
            setParticipantsError(false);
        }

        if(!formData.participantsCount){
            setIsSubmitDisabled(true);
        }
        if(formData.participantsCount <= MAX_PARTICIPANTS_AMOUNT && formData.participantsCount > 0){
            setIsSubmitDisabled(false);
        }
    }, [formData.participantsCount]);

    // jesli zmieni sie wybrana data to wyliczany jest dla niej offset i pobierane dane o godzinach pracy 
    useEffect(() => {
        if (formData.date) {

            const startDate = new Date(formData.date);
            getWorkingHoursForSingleDay(startDate).then(weekOffset => {
                console.log(weekOffset)
                getClubWorkingHours(weekOffset)
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
        const startDateTime = `${formData.date}T${formData.startTime}`;
        const endDateTime = `${formData.date}T${formData.endTime}`;

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
    }, [formData.endTime]);

    // pobranie dostepnych trenerow w podanych godzinach czasowych w dany dzien
    useEffect(() => {
        const startDateTime = `${formData.date}T${formData.startTime}`;
        const endDateTime = `${formData.date}T${formData.endTime}`;

        getAvailableTrainers(startDateTime, endDateTime)
            .then(response => {
                setAvailableTrainers(response);
            })
            .then(data => {
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania get courts:', error);
            });
    }, [formData.endTime]);


    function handleCancel() {
        navigate(-1);
    }

    function determineFailTextByResponseCode(responseCode) {
        switch (responseCode) {
          case 420:
            return dictionary.addReservationYourselfPage.alreadyHasActivityLabel;
          default:
            return dictionary.addReservationYourselfPage.savedFailureLabel;
        }
      }

    // ustawienie dni i godzin pracy na takie jak w zaznaczonym dniu
    const selectedDayWorkingHours = workingDaysAndHours.find(day => day.date === formData.date);
    const openHour = selectedDayWorkingHours ? selectedDayWorkingHours.openHour.slice(0, 5) : "10:00";
    const closeHour = selectedDayWorkingHours ? selectedDayWorkingHours.closeHour.slice(0, 5) : "22:00";



    function isFormValid() {
        const { date, startTime, courtId, participantsCount } = formData;

        let participantsNumber = Number(participantsCount);
        return (
            Boolean(date) &&
            Boolean(startTime) &&
            Boolean(courtId) &&
            Number(participantsCount) > 0 &&
            Number(participantsCount) <= MAX_PARTICIPANTS_AMOUNT &&
            !isNaN(participantsNumber)
        );
    }


    async function handleSubmit() {
        if (!isFormValid()) {
            return;
        }

        const makeIsoDateTime = (date, time) => `${date}T${time}:00`;

        const payload = {
            CourtId: Number(formData.courtId),
            StartTime: makeIsoDateTime(formData.date, formData.startTime),
            EndTime: makeIsoDateTime(formData.date, formData.endTime),
            CreationDate: new Date().toISOString().slice(0, 19),
            TrainerId: formData.trainerId ? Number(formData.trainerId) : 0,
            ParticipantsCount: formData.participantsCount.toString(),
            IsEquipmentReserved: formData.isEquipmentReserved ? "true" : "",
        };

        try {
            const response = await addReservationYourself(payload);

            if (!response.ok) {
                const errorData = await response.json();
                let failText = determineFailTextByResponseCode(response.status);
                setFailedSignUpLabel(failText);
                handleOpenFailure();
            } else {
                handleOpenSuccess();
                setFormData(prev => ({
                    date: '',
                    startTime: '',
                    endTime: '',
                    creationDate: '',
                    courtId: '',
                    trainerId: '',
                    participantsCount: '',
                    isEquipmentReserved: false,
                }));
            }
        } catch (error) {
            console.error('Błąd podczas rezerwacji:', error);
            handleOpenFailure();
        }
    }
    
    
    

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };
    return (<>
        <GreenBackground height={"80vh"} marginTop={"2vh"}>
            <Header>{dictionary.addReservationYourselfPage.title}</Header>
            <OrangeBackground width="70%">
                <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", }}>



                    <Box sx={{
                        display: 'flex',
                        flexDirection: 'row',
                    }}>

                        <CustomInput
                            label={dictionary.addReservationYourselfPage.dateLabel}
                            type="date"
                            id="date"
                            name="date"
                            fullWidth
                            value={formData.date}
                            onChange={handleChange}
                            error={dateError}
                            helperText={dateError ? dictionary.addReservationYourselfPage.dateErrorLabel : ""}
                            required
                            size="small"
                            InputLabelProps={{
                                shrink: true
                            }}
                        />

                    </Box>
                    <Box sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        columnGap: '3vw'
                    }}>

                        <CustomTimeInput
                            select
                            label={dictionary.addReservationYourselfPage.startTimeLabel}
                            id="startTime"
                            name="startTime"
                            value={formData.startTime}
                            onChange={handleChange}
                            error={startTimeError}
                            helperText={startTimeError ? dictionary.addReservationYourselfPage.startTimeErrorLabel : ""}
                            required
                            size="small"
                            fullWidth
                            SelectProps={{
                                sx: {
                                    textAlign: 'left',
                                    borderRadius: '8px'
                                }
                            }}
                            disabled={!formData.date || dateError}
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
                            id="endTime"
                            name="endTime"
                            value={formData.endTime}
                            onChange={handleChange}
                            required
                            size="small"
                            fullWidth
                            SelectProps={{
                                sx: {
                                    textAlign: 'left',
                                    borderRadius: '8px'
                                }
                            }}
                            disabled={!formData.date || !formData.startTime || startTimeError}
                        >
                            <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                            {getEndTimeOptions(formData,closeHour).map((time) => (
                                <MenuItem key={time} value={time}>
                                    {time}
                                </MenuItem>
                            ))}
                        </CustomTimeInput>
                    </Box>

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
                        disabled={!formData.date || !formData.startTime || !formData.endTime}
                        helperText={courtsError ? courtsError : ''}
                        error={courtsError}
                    >
                        <MenuItem value=""></MenuItem>
                        {courts.map(court => (
                            <MenuItem key={court.id} value={court.id}>
                                {court.name}
                            </MenuItem>
                        ))}
                    </CustomInput>
                    <CustomInput
                        select
                        label={dictionary.addReservationYourselfPage.trainerNameLabel}
                        id="trainerName"
                        name="trainerId"
                        value={formData.trainerId}
                        fullWidth
                        onChange={handleChange}
                        size="small"
                        SelectProps={{
                            sx: { textAlign: 'left' }
                        }}
                        disabled={!formData.date || !formData.startTime || !formData.endTime || !formData.courtId}
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
                        error={participantsError}
                        helperText={participantsError ? dictionary.addReservationYourselfPage.participantsCountError : ""}
                        size="small"
                        inputProps={{ min: 1 }}
                        required
                        disabled={!formData.date || !formData.startTime || !formData.endTime || !formData.courtId}
                    />
                    <FormControlLabel
                        control={
                            <Checkbox
                                id="isEquipmentReserved"
                                name="isEquipmentReserved"
                                checked={formData.isEquipmentReserved}
                                onChange={() => {
                                    setFormData(prev => ({
                                        ...prev,
                                        isEquipmentReserved: !prev.isEquipmentReserved
                                    }));
                                }}
                                sx={{
                                    color: "#8edfb4",
                                    '&.Mui-checked': {
                                        color: "#8edfb4",
                                    },
                                }}
                            />
                        }
                        label={dictionary.activityDetailsPage.isEquipmentIncludedLabel}
                    />

                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        columnGap:'3vw',
                        alignItems: 'center',
                    }}>
                       <GreenButton onClick={handleCancel} style={{ maxWidth: "13vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.activityDetailsPage.returnLabel}</GreenButton>
                       <GreenButton type="submit" disabled={isSubmitDisabled} style={{ maxWidth: "13vw" }} onClick={handleSubmit}>{dictionary.addReservationYourselfPage.confirmLabel}</GreenButton>
                        
                    </Box>
                </Box>
            </OrangeBackground>
        </GreenBackground>
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
                        {dictionary.activityDetailsPage.successLabel}
                    </Typography>
                </Box>
                <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                        {dictionary.addReservationYourselfPage.reservationCreatedLabel}
                    </Typography>
                </Box>
                <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                        {dictionary.activityDetailsPage.clickAnywhereLabel}
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
                minWidth: '40vw',
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
                        {dictionary.activityDetailsPage.failureLabel}
                    </Typography>
                </Box>
                <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                        {failedSignUpLabel}
                    </Typography>
                </Box>
                <Box>
                    <Typography sx={{
                        color: 'black',
                        fontSize: '1.5rem',
                    }}>
                        {dictionary.activityDetailsPage.clickAnywhereFailureLabel}
                    </Typography>
                </Box>
                <Box sx={{ textAlign: 'center', display: "flex", justifyContent: "center" }}>
                    <Avatar sx={{ width: "7rem", height: "7rem" }}>
                        <SentimentDissatisfiedIcon sx={{ fontSize: "7rem", color: 'red', stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }} />
                    </Avatar>
                </Box>
            </Box>
        </Backdrop>
    </>);
}
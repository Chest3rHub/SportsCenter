import { Box, MenuItem, Checkbox, FormControlLabel, Autocomplete, Typography } from "@mui/material";
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
import { useDebounce } from '../hooks/useDebounce';
import searchClientsTopTen from '../api/searchClientsTopTen';
import addRecurringReservationForClient from '../api/addRecurringReservationForClient';
import addReservationForClient from '../api/addReservationForClient';
import { Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import ReservationButton from '../components/buttons/ReservationButton';
import Backdrop from '@mui/material/Backdrop';
import Avatar from '@mui/material/Avatar';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

export default function AddReservationForClient() {

    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();

    const [courts, setCourts] = useState([]);
    const [availableCourts, setAvailableCourts] = useState([]);
    const [trainers, setTrainers] = useState([]);
    const [clients, setClients] = useState([]);
    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [isEquipmentIncluded, setIsEquipmentIncluded] = useState(false);
    const [workingDaysAndHours, setWorkingDaysAndHours] = useState([]);
    const [offset, setOffset] = useState(-1);
    const [courtsError, setCourtsError] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

    const [startTimeError, setStartTimeError] = useState(false);
    const [endTimeRequiredError, setEndTimeRequiredError] = useState(false);
    const [endTimeBeforeStartError, setEndTimeBeforeStartError] = useState(false);
    const [endTimeDurationError, setEndTimeDurationError] = useState(false);
    const [participantsCountError, setParticipantsCountError] = useState(false);
    const [isEquipmentReservedError, setIsEquipmentReservedError] = useState(false);
    const [recurrenceEndDateError, setRecurrenceEndDateError] = useState(false);
    const [dateError, setDateError] = useState(false);
    const [dateRequiredError, setDateRequiredError] = useState(false);
    const [startTimeRequiredError, setStartTimeRequiredError] = useState(false);
    const [clientRequiredError, setClientRequiredError] = useState(false);

    const [searchClientText, setSearchClientText] = useState('');
    const debouncedSearchText = useDebounce(searchClientText, 300);
    const [selectedClient, setSelectedClient] = useState(null);

    const [openProposalDialog, setOpenProposalDialog] = useState(false);
    const [reservationProposals, setReservationProposals] = useState([]);
    const [courtIdMissingError, setCourtIdMissingError] = useState({});
    const [trainerIdMissingError, setTrainerIdMissingError] = useState({});
    const [selectedCourtIds, setSelectedCourtIds] = useState({});
    const [selectedTrainerIds, setSelectedTrainerIds] = useState({});

    const setCourtFor = (uid, courtId) => {
        setSelectedCourtIds(prev =>   ({ ...prev,   [uid]: courtId }));
        setCourtIdMissingError(prev => ({ ...prev,   [uid]: false   }));
    };

    const setTrainerFor = (uid, trainerId) => {
        setSelectedTrainerIds(prev =>   ({ ...prev,   [uid]: trainerId }));
        setTrainerIdMissingError(prev => ({ ...prev,   [uid]: false     }));
    };

    const [failedSignUpLabel, setFailedSignUpLabel] = useState('');
    const [proposalErrors, setProposalErrors] = useState({});

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

    const [formData, setFormData] = useState({
        clientId: '',
        courtId: '',
        startTime: '',
        endTime: '',
        creationDate: '',
        trainerId: '',
        participantsCount: '',
        isEquipmentReserved: false,
        recurrence: '',
        recurrenceEndDate: ''
    });

    function removeCourtErrorsAfterChange(){
        setCourtsError('');
    }

    useEffect(() => {
        async function fetchData() {
            setIsLoading(true);
            try {
                const [clientsResponse] = await Promise.all([
                    searchClientsTopTen(debouncedSearchText)
                ]);

                const clientsData = await clientsResponse.json();
                setClients(clientsData);

            } catch (error) {
                console.error('Błąd podczas pobierania danych:', error);
            } finally {
                setIsLoading(false);
            }
        }

        fetchData();
    }, [debouncedSearchText]);
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
            clientId: prev.clientId,
            date: prev.date,
            startTime: '',
            endTime: '',
            creationDate: '',
            courtId: '',
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
            recurrence: '',
            recurrenceEndDate: ''
        }));
        removeCourtErrorsAfterChange();
    }, [formData.date]);
    // jak sie zmieni czas rozpoczecia usuwamy wszystko poza data i czasem rozpoczecia
    useEffect(() => {
        setFormData(prev => ({
            clientId: prev.clientId,
            date: prev.date,
            startTime: prev.startTime,
            endTime: '',
            creationDate: '',
            courtId: '',
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
            recurrence: '',
            recurrenceEndDate: ''
        }));
        removeCourtErrorsAfterChange();
    }, [formData.startTime]);
    // jak sie zmieni czas zakonczenia usuwamy wszystko poza data i czasem rozpoczecia zakonczenia
    useEffect(() => {
        setFormData(prev => ({
            clientId: prev.clientId,
            date: prev.date,
            startTime: prev.startTime,
            endTime: prev.endTime,
            creationDate: '',
            courtId: '',
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
            recurrence: '',
            recurrenceEndDate: ''
        }));
        removeCourtErrorsAfterChange();
    }, [formData.endTime]);
    // jak sie zmieni kort usuwamy wszystko poza data, czasami i kortem
    useEffect(() => {
        setFormData(prev => ({
            clientId: prev.clientId,
            date: prev.date,
            startTime: prev.startTime,
            endTime: prev.endTime,
            creationDate: '',
            courtId: prev.courtId,
            trainerId: '',
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
            recurrence: '',
            recurrenceEndDate: ''
        }));
    }, [formData.courtId]);
    // jak sie zmieni trener usuwamy wszystko poza data, czasami kortem i trenerem
    useEffect(() => {
        setFormData(prev => ({
            clientId: prev.clientId,
            date: prev.date,
            startTime: prev.startTime,
            endTime: prev.endTime,
            creationDate: '',
            courtId: prev.courtId,
            trainerId: prev.trainerId,
            participantsCount: '',
            isEquipmentReserved: prev.isEquipmentReserved,
            recurrence: '',
            recurrenceEndDate: ''
        }));
    }, [formData.trainerId]);
    // jak sie zmieni liczba uczestnikow czy informacja o rezerwacji sprzetu, rekurencji lub klient to nic nie usuwamy

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
        const startDateTime = `${formData.date}T${formData.startTime}`;
        const endDateTime = `${formData.date}T${formData.endTime}`;

        getAvailableCourts(startDateTime, endDateTime)
            .then(response => {
                setAvailableCourts(response);
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

    // ustawienie dni i godzin pracy na takie jak w zaznaczonym dniu
    const selectedDayWorkingHours = workingDaysAndHours.find(day => day.date === formData.date);
    const openHour = selectedDayWorkingHours ? selectedDayWorkingHours.openHour.slice(0, 5) : "10:00";
    const closeHour = selectedDayWorkingHours ? selectedDayWorkingHours.closeHour.slice(0, 5) : "22:00";

    function isFormValid() {
        let valid = true;
        const { clientId, courtId, date, startTime, endTime, participantsCount, recurrenceEndDate } = formData;

        const now = new Date();
        const todayStr = now.toISOString().slice(0,10);
        const start = date && startTime ? new Date(`${date}T${startTime}:00`) : null;
        const end   = date && endTime ? new Date(`${date}T${endTime}:00`) : null;

        if (!clientId){
            setClientRequiredError(true);
            valid=false;
        } else {
            setClientRequiredError(false);
        }

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

        // kort
        if (!courtId){
            setCourtsError(true);
            valid=false;
        } else {
            setCourtsError(false);
        }

        // ilość uczestników 1-8
        const participants = parseInt(participantsCount, 10);
        if (!participants || participants < 1 || participants > 8) {
            setParticipantsCountError(true);
            valid = false;
        } else {
            setParticipantsCountError(false);
        }

        // data zakończenia cykliczności - co najmniej 1 dzień po endTime
        if (recurrenceEndDate && end) {
            const recDate = new Date(recurrenceEndDate);
            recDate.setHours(0,0,0,0);
            const endDate = new Date(end);
            endDate.setHours(0,0,0,0);
            const daysDiff = (recDate - endDate) / (1000 * 60 * 60 * 24);

            if (daysDiff < 1) {
            setRecurrenceEndDateError(true);
            valid = false;
            } else {
            setRecurrenceEndDateError(false);
            }
        }
        return valid;
    }

    function determineFailTextByResponseCode(responseCode) {
        switch (responseCode) {
            case 411:
                return dictionary.addReservationForClientPage.courtNotAvailable;
            case 414:
                return dictionary.addReservationForClientPage.clientWithGivenIdNotFound;
            case 415:
                return dictionary.addReservationForClientPage.tooManyParticipants;
            case 416:
                return dictionary.addReservationForClientPage.employeeNotFound;
            case 417:
                return dictionary.addReservationForClientPage.notTrainerEmployee;
            case 418:
                return dictionary.addReservationForClientPage.employeeAlreadyDismissedException;
            case 419:
                return dictionary.addReservationForClientPage.trainerNotAvailableError;
            case 420:
                return dictionary.addReservationForClientPage.alreadyHasActivityLabel;
            default:
                return dictionary.addReservationForClientPage.savedFailureLabel;
        }
    }

    function handleError(textToDisplay) {
        navigate('/error', {
            state: { message: textToDisplay }
        });
    }

    function toLocalIso(dt) {
        const t = new Date(dt.getTime() - dt.getTimezoneOffset() * 60000);
        return t.toISOString().slice(0, 19);
    }

    async function handleSubmit() {
        if (!isFormValid()) {
            return;
        }
    
        const now = new Date();
        const offsetInMs = now.getTimezoneOffset() * 60 * 1000;
        const isRecurring = Boolean(formData.recurrence?.trim()) && Boolean(formData.recurrenceEndDate?.trim());

        let payload = {
            ClientId: Number(formData.clientId),
            CourtId: Number(formData.courtId),
            StartTime: toLocalIso(new Date(`${formData.date}T${formData.startTime}:00`)),
            EndTime: toLocalIso(new Date(`${formData.date}T${formData.endTime}:00`)),
            CreationDate: toLocalIso(new Date()),
            TrainerId: formData.trainerId ? Number(formData.trainerId) : 0,
            ParticipantsCount: formData.participantsCount.toString(),
            IsEquipmentReserved: formData.isEquipmentReserved ? "true" : "",
        };

        if (isRecurring) {
            payload.Recurrence = formData.recurrence;
            payload.RecurrenceEndDate = toLocalIso(new Date(`${formData.recurrenceEndDate}T23:59:00`));
        }

        try {
            let response;

            if (isRecurring) {
                response = await addRecurringReservationForClient(payload);
            } else {
                response = await addReservationForClient(payload);
            }
    
            if (!response.ok) {
                const errorData = await response.json();
                let failText = determineFailTextByResponseCode(response.status);
                setFailedSignUpLabel(failText);
                handleOpenFailure();
            }

            const resultData = await response.json(); 

            if (resultData.reservationProposals && resultData.reservationProposals.length > 0) {
                setReservationProposals(
                    resultData.reservationProposals.map((p, i) => ({
                        ...p,
                        _uid: `${p.date}-${i}`
                    }))
                );
                handleOpenProposal();
            } else {
                setOpenSuccessBackdrop(true);
                setFormData(prev => ({
                    clientId: '',
                    date: '',
                    startTime: '',
                    endTime: '',
                    creationDate: '',
                    courtId: '',
                    trainerId: '',
                    participantsCount: '',
                    isEquipmentReserved: false,
                    recurrence: '',
                    recurrenceEndDate: '',
                }));
            }

        } catch (error) {
            console.error('Błąd podczas rezerwacji:', error);
        }
    }

    const handleOpenProposal = () => setOpenProposalDialog(true);
    const handleCloseProposal = () => setOpenProposalDialog(false);

    const handleRejectProposal = uid => {
        setReservationProposals(prev => prev.filter(p => p._uid !== uid));
        setSelectedCourtIds(prev =>   { const o={...prev}; delete o[uid]; return o; });
        setSelectedTrainerIds(prev => { const o={...prev}; delete o[uid]; return o; });
        setCourtIdMissingError(prev =>   { const o={...prev}; delete o[uid]; return o; });
        setTrainerIdMissingError(prev => { const o={...prev}; delete o[uid]; return o; });
        if (reservationProposals.length === 1) handleCloseProposal();
    };

    const handleAcceptProposal = async (uid) => {
        const p = reservationProposals.find(x => x._uid === uid);
        const courtId   = selectedCourtIds[uid];
        const trainerId = selectedTrainerIds[uid];

        const [eh, em] = formData.endTime.split(':').map(Number);

        const dateOnly = p.date.split('T')[0];
        const start = new Date(`${dateOnly}T${formData.startTime}:00`);
        const end = new Date(`${dateOnly}T${formData.endTime}:00`);

        const hasCourtErr   = !courtId;
        setCourtIdMissingError(prev   => ({ ...prev,   [uid]: hasCourtErr }));
        if (hasCourtErr) return;

        const reservationData = {
            ClientId: Number(formData.clientId),
            CourtId: Number(courtId),
            StartTime: toLocalIso(start),
            EndTime: toLocalIso(end),
            CreationDate: toLocalIso(new Date()),
            TrainerId: trainerId ? Number(trainerId) : 0,
            ParticipantsCount: formData.participantsCount,
            IsEquipmentReserved: formData.isEquipmentReserved ? "true" : "",
        };

        try {
            const response = await addReservationForClient(reservationData);
            if (response.ok) {
                handleRejectProposal(uid);
            } else {
                const failText = determineFailTextByResponseCode(response.status);
                setProposalErrors(prev => ({ 
                    ...prev, 
                    [uid]: failText 
                }));
            }
        } catch (error) {
            console.error("Błąd:", error);
        }
    };

    function handleCancel() {
        navigate('/reservations', {
          });
    }

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData((prev) => ({
            ...prev,
            [name]: value
        }));
    };

    return (
        <>
            <GreenBackground height={"80vh"} marginTop={"2vh"}>
                <Header>{dictionary.addReservationForClientPage.title}</Header>
                <OrangeBackground width="70%">
                    <Box sx={{ display: "flex", flexDirection: "column", gap: "1.5rem", marginBottom: "2vh" }}>
                            <Autocomplete
                                id="client"
                                options={clients}
                                getOptionLabel={(option) => option.email}
                                value={selectedClient}
                                onChange={(event, newValue) => {
                                    handleChange({
                                        target: {
                                            name: "clientId",
                                            value: newValue?.clientId || ""
                                        }
                                    });
                                    setSelectedClient(newValue);
                                }}
                                onInputChange={(event, newInputValue) => {
                                    setSearchClientText(newInputValue);
                                }}
                                isOptionEqualToValue={(option, value) => option.id === value.id}
                                renderInput={(params) => (
                                    <CustomInput
                                        {...params}
                                        label={dictionary.addReservationForClientPage.clientLabel}
                                        name="clientId"
                                        required
                                        size="small"
                                        error={clientRequiredError}
                                        helperText={clientRequiredError ? dictionary.addReservationForClientPage.clientRequiredError : ""}
                                    />
                                )}
                            />

                            <CustomInput
                                label={dictionary.addReservationYourselfPage.dateLabel}
                                type="date"
                                id="date"
                                name="date"
                                fullWidth
                                value={formData.date}
                                onChange={handleChange}
                                error={dateRequiredError || dateError}
                                helperText={dateRequiredError ? dictionary.addReservationForClientPage.dateRequiredError : dateError 
                                                              ? dictionary.addReservationForClientPage.dateError : ""}
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
                                    id="startTime"
                                    name="startTime"
                                    value={formData.startTime}
                                    onChange={handleChange}
                                    required
                                    error={startTimeRequiredError || startTimeError}
                                    helperText={startTimeRequiredError ? dictionary.addReservationForClientPage.startTimeRequiredError : startTimeError 
                                                                       ? dictionary.addReservationForClientPage.startTimeError : ""}
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
                                    id="endTime"
                                    name="endTime"
                                    value={formData.endTime}
                                    onChange={handleChange}
                                    error={endTimeRequiredError || endTimeBeforeStartError || endTimeDurationError}
                                    helperText={endTimeRequiredError ? dictionary.addReservationForClientPage.endTimeRequiredError : endTimeBeforeStartError
                                                                     ? dictionary.addReservationForClientPage.endTimeBeforeStartError : endTimeDurationError
                                                                     ? dictionary.addReservationForClientPage.endTimeDurationError : ""
                                    }
                                    required
                                    size="small"
                                    fullWidth
                                    SelectProps={{
                                        sx: {
                                            textAlign: 'left',
                                            borderRadius: '8px'
                                        }
                                    }}
                                    disabled={!formData.date || !formData.startTime}
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
                                label={dictionary.addReservationForClientPage.courtNameLabel}
                                id="courtName"
                                name="courtId"
                                value={formData.courtId}
                                fullWidth
                                onChange={handleChange}
                                error={courtsError}
                                helperText={courtsError ? dictionary.addReservationForClientPage.courtsError : ""}
                                required
                                size="small"
                                SelectProps={{
                                    sx: {
                                        textAlign: 'left',
                                        borderRadius: '8px'
                                    }
                                }}
                                disabled={!formData.date || !formData.startTime || !formData.endTime}
                            >
                                <MenuItem value=""></MenuItem>
                                {availableCourts.map(court => (
                                    <MenuItem key={court.id} value={court.id}>
                                        {court.name}
                                    </MenuItem>
                                ))}
                            </CustomInput>

                            <CustomInput
                                select
                                label={dictionary.addReservationForClientPage.trainerNameLabel}
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
                                label={dictionary.addReservationForClientPage.participantsCountLabel}
                                type="number"
                                id="participantsCount"
                                name="participantsCount"
                                fullWidth
                                value={formData.participantsCount}
                                onChange={handleChange}
                                error={participantsCountError}
                                helperText={participantsCountError ? dictionary.addReservationForClientPage.participantsCountError : ""}
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
                                label={dictionary.addReservationForClientPage.isEquipmentReservedLabel}
                            />

                            <CustomInput
                                select
                                label={dictionary.addReservationForClientPage.recurrenceLabel}
                                id="recurrence"
                                name="recurrence"
                                value={formData.recurrence}
                                fullWidth
                                onChange={handleChange}
                                size="small"
                                SelectProps={{
                                    sx: {
                                        textAlign: 'left',
                                        borderRadius: '8px'
                                    }
                                }}
                            >
                                <MenuItem value="">{dictionary.addReservationForClientPage.recurrenceOptions?.None || 'Brak'}</MenuItem>
                                <MenuItem value="Daily">{dictionary.addReservationForClientPage.recurrenceOptions.Daily}</MenuItem>
                                <MenuItem value="Weekly">{dictionary.addReservationForClientPage.recurrenceOptions.Weekly}</MenuItem>
                                <MenuItem value="BiWeekly">{dictionary.addReservationForClientPage.recurrenceOptions.BiWeekly}</MenuItem>
                                <MenuItem value="Monthly">{dictionary.addReservationForClientPage.recurrenceOptions.Monthly}</MenuItem>
                            </CustomInput>

                            <CustomInput
                                label={dictionary.addReservationForClientPage.recurrenceEndDateLabel}
                                type="date"
                                id="recurrenceEndDate"
                                name="recurrenceEndDate"
                                fullWidth
                                value={formData.recurrenceEndDate}
                                onChange={handleChange}
                                error={recurrenceEndDateError}
                                helperText={recurrenceEndDateError ? dictionary.addReservationForClientPage.recurrenceEndDateError : ""}
                                size="small"
                                InputLabelProps={{
                                    shrink: true
                                }}
                            />

                            <Box sx={{
                                display: "flex",
                                flexDirection: "row",
                                justifyContent: 'center',
                                columnGap: "2vw"
                            }}>
                                <GreenButton onClick={handleCancel} style={{ backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.addReservationForClientPage.returnLabel}</GreenButton>
                                <GreenButton type="submit" onClick={handleSubmit}>{dictionary.addReservationForClientPage.confirmLabel}</GreenButton>
                            </Box>
                        </Box>
                        <Dialog 
                            open={openProposalDialog}
                            onClose={handleCloseProposal}
                            fullWidth
                            maxWidth="md"
                        >
                            <DialogTitle sx={{ textAlign: 'center', color: '#F46C63', fontSize: '2rem' }}>
                                {dictionary.addReservationForClientPage.proposalHeader}
                            </DialogTitle>

                            <DialogContent dividers>
                                <Typography sx={{ color: '#ccc', fontSize: '1rem', mb: 2, textAlign: 'center' }}>
                                    {dictionary.addReservationForClientPage.closeInfo}
                                </Typography>

                                {reservationProposals.length > 0 ? (
                                    reservationProposals.map(p => {
                                        const uid = p._uid;
                                        const date = new Date(p.date).toLocaleString('pl-PL', {
                                            year: 'numeric', month: '2-digit', day: '2-digit',
                                            hour: '2-digit', minute: '2-digit'
                                        
                                    });

                                    return (
                                        <Box key={uid}
                                        sx={{
                                            mb: 3,
                                            p: 3,
                                            backgroundColor: '#ebf9ea',
                                            borderRadius: 2,
                                            display: 'flex',
                                            flexDirection: 'column',
                                            rowGap: '3vh'
                                        }}
                                        >
                                            <Typography variant="h6" sx={{ fontWeight: 'bold', color: '#333' }}>
                                                {date}
                                            </Typography>

                                            {p.availableCourts.length === 0 ? (
                                                <Typography variant="body2" color="error" sx={{ mb: 2 }}>
                                                    {dictionary.addReservationForClientPage.noCourtsAvailable}
                                                </Typography>
                                            ) : (
                                                <CustomInput
                                                  select
                                                  label={dictionary.addReservationForClientPage.courtNameLabel}
                                                  fullWidth
                                                  value={selectedCourtIds[uid] || ''}
                                                  onChange={e => setCourtFor(uid, e.target.value)}
                                                  error={Boolean(courtIdMissingError[uid])}
                                                  size="small"
                                                  sx={{ mb: 2,  }}
                                                  >
                                                    <MenuItem value="" />
                                                    {p.availableCourts.map(courtId => {
                                                        const court = courts.find(c => c.id === courtId);
                                                        return (
                                                            <MenuItem key={courtId} value={courtId}>
                                                                {court?.name || `Id: ${courtId}`}
                                                            </MenuItem>
                                                        );
                                                    })}
                                                  </CustomInput>
                                            )}

                                            {p.availableTrainers.length === 0 ? (
                                                <Typography variant="body2" color="error" sx={{ mb: 2 }}>
                                                    {dictionary.addReservationForClientPage.noTrainersAvailable}
                                                </Typography>
                                            ) : (
                                                <CustomInput
                                                  select
                                                  label={dictionary.addReservationForClientPage.trainerNameLabel}
                                                  fullWidth
                                                  value={selectedTrainerIds[uid] || ''}
                                                  onChange={e => setTrainerFor(uid, e.target.value)}
                                                  error={Boolean(trainerIdMissingError[uid])}
                                                  size="small"
                                                  sx={{ mb: 2 }}
                                                  >
                                                    <MenuItem value="" />
                                                    {p.availableTrainers.map(trainerId => {
                                                        const trainer = trainers.find(t => t.id === trainerId);
                                                        return (
                                                            <MenuItem key={trainerId} value={trainerId}>
                                                                {trainer?.fullName || `Id: ${trainerId}`}
                                                            </MenuItem>
                                                        );
                                                    })}
                                                  </CustomInput>
                                            )}

                                            {proposalErrors[uid] && (
                                                <Typography color="error" sx={{ mb: 2 }}>
                                                {proposalErrors[uid]}
                                                </Typography>
                                            )}

                                            <Box sx={{
                                                display: "flex",
                                                flexDirection: "row",
                                                justifyContent: 'center',
                                                columnGap: "1vw"
                                                }}>

                                                <ReservationButton backgroundColor={"#F46C63"} onClick={() => handleRejectProposal(uid)}>{dictionary.addReservationForClientPage.ignoreReservationProposal}</ReservationButton>

                                                <ReservationButton
                                                    backgroundColor={"#8edfb4"}
                                                    disabled={p.availableCourts.length === 0}
                                                    onClick={() => handleAcceptProposal(uid)}
                                                >
                                                    {dictionary.addReservationForClientPage.acceptReservationProposal}
                                                </ReservationButton>
                                            </Box>
                                        </Box>
                                    );
                                })
                                ) : (
                                    <Typography variant="body1" color="textSecondary" align="center">
                                        {dictionary.addReservationForClientPage.noReservatonProposals}
                                    </Typography>
                                )}
                            </DialogContent>
                        </Dialog>
                        {/* BACKDROP SUKCESU */}
                        <Backdrop open={openSuccessBackdrop} onClick={handleCloseSuccess}
                                sx={{ color:'#fff', zIndex: theme=>theme.zIndex.drawer+1 }}>
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
                                        {dictionary.addReservationForClientPage.successLabel}
                                    </Typography>
                                </Box>
                                <Box>
                                    <Typography sx={{
                                        color: 'black',
                                        fontSize: '1.5rem',
                                    }}>
                                        {dictionary.addReservationForClientPage.reservationCreatedLabel}
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

                        {/* BACKDROP BŁĘDU */}
                        <Backdrop open={openFailureBackdrop} onClick={handleCloseFailure}
                                sx={{ color:'#fff', zIndex: theme=>theme.zIndex.drawer+1 }}>
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
                                    {dictionary.addReservationForClientPage.failureLabel}
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
                </OrangeBackground>
            </GreenBackground>
        </>
    );
}
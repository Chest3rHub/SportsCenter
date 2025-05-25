import { Box, MenuItem, Checkbox, FormControlLabel } from "@mui/material";
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

export default function NewClientReservation() {
    const { dictionary, toggleLanguage } = useContext(SportsContext);

    const [courts, setCourts] = useState([]);
    const [trainers, setTrainers] = useState([]);
    const [availableTrainers, setAvailableTrainers] = useState([]);
    const [isEquipmentIncluded, setIsEquipmentIncluded] = useState(false);
    const [workingDaysAndHours, setWorkingDaysAndHours] = useState([]);
    const [offset, setOffset] = useState(-1);
    const [courtsError, setCourtsError] = useState('');


    // moze zmienic zeby trener nie byl required?
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
    // jak sie zmieni czas rozpoczecia usuwamy wszystko poza data i czasem rozpoczecia
    useEffect(() => {
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




    // ustawienie dni i godzin pracy na takie jak w zaznaczonym dniu
    const selectedDayWorkingHours = workingDaysAndHours.find(day => day.date === formData.date);
    const openHour = selectedDayWorkingHours ? selectedDayWorkingHours.openHour.slice(0, 5) : "10:00";
    const closeHour = selectedDayWorkingHours ? selectedDayWorkingHours.closeHour.slice(0, 5) : "22:00";



    function isFormValid() {
        const { date, startTime, courtId, participantsCount } = formData;
    
        return (
            Boolean(date) &&
            Boolean(startTime) &&
            Boolean(courtId) &&
            Number(participantsCount) > 0
        );
    }
    
    
    async function handleSubmit() {
        if (!isFormValid()) {
            alert("Brakuje któregoś z pól: data, godzina rozpoczęcia, kort, liczba uczestników...");
            return;
        }
    
        const makeIsoDateTime = (date, time) => `${date}T${time}:00`;
    
        const payload = {
            CourtId: Number(formData.courtId),
            StartTime: makeIsoDateTime(formData.date, formData.startTime),
            EndTime: makeIsoDateTime(formData.date, formData.endTime),
            CreationDate: new Date().toISOString().slice(0,19),
            TrainerId: formData.trainerId ? Number(formData.trainerId) : 0,
            ParticipantsCount: formData.participantsCount.toString(),
            IsEquipmentReserved: formData.isEquipmentReserved ? "true" : "",
        };
    
        try {
            const response = await addReservationYourself(payload);
    
            if (response.ok) {
                alert("Rezerwacja została dodana pomyślnie.");
            } else {
                alert("Nie udało się dodać rezerwacji. Spróbuj ponownie później.");
            }
        } catch (error) {
            console.error('Błąd podczas rezerwacji:', error);
            alert("Wystąpił błąd podczas rezerwacji. Spróbuj ponownie później.");
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
                            error={false}
                            helperText={false ? dictionary.addReservationYourselfPage.startTimeError : ""}
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
                        error={false}
                        helperText={false ? dictionary.addReservationYourselfPage.participantsCountError : ""}
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

                        alignItems: 'center',
                    }}>
                        <GreenButton type="submit" style={{ maxWidth: "15vw" }} onClick={handleSubmit}>{dictionary.addReservationYourselfPage.confirmLabel}</GreenButton>
                    </Box>
                </Box>
            </OrangeBackground>
        </GreenBackground>
    </>);
}
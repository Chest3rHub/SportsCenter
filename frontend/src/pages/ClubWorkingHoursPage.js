import { Box, Typography, Modal, Button, MenuItem, Select, FormControl, InputLabel } from "@mui/material";
import { useContext, useEffect, useState } from "react";
import { SportsContext } from "../context/SportsContext";
import Header from "../components/Header";
import GreenButton from "../components/buttons/GreenButton";
import getClubWorkingHours from '../api/getClubWorkingHours';
import setSpecialWorkingHours  from '../api/setSpecialWorkingHours';
import setRegularWorkingHours from "../api/setRegularWorkingHours";

import CustomInput from "../components/CustomInput";

export default function ClubWorkingHoursPage() {
    const { dictionary } = useContext(SportsContext);
    const [workingHours, setWorkingHours] = useState([]);
    const [loading, setLoading] = useState(true);
    const [offset, setOffset] = useState(0);
    const [openSpecialHoursModal, setOpenSpecialHoursModal] = useState(false);
    const [openRegularHoursModal, setOpenRegularHoursModal] = useState(false);
    const [selectedDate, setSelectedDate] = useState('');
    const [selectedDayOfWeek, setSelectedDayOfWeek] = useState('');
    const [hours, setHours] = useState({
        openHour: '',
        closeHour: ''
    });

    useEffect(() => {
        fetchWorkingHours();
    }, [offset]);

    const fetchWorkingHours = async () => {
        try {
            setLoading(true);
            const response = await getClubWorkingHours(offset);
            if (!response.ok) {
                throw new Error('Failed to fetch opening hours!');
            }
            const data = await response.json();
            setWorkingHours(data);
        } catch (error) {
            console.error('Error:', error);
            setWorkingHours([]);
        } finally {
            setLoading(false);
        }
    };

    const handleNextWeek = () => {
        setOffset(prev => prev + 1);
    };

    const formatDayWithDate = (day) => {
    const dayMapping = {
        'poniedzialek': 'monday',
        'wtorek': 'tuesday',
        'sroda': 'wednesday',
        'czwartek': 'thursday',
        'piatek': 'friday',
        'sobota': 'saturday',
        'niedziela': 'sunday'
    };
    const dayKey = dayMapping[day.dayOfWeek.toLowerCase()] || day.dayOfWeek;
    const translatedDay = dictionary.days[dayKey] || day.dayOfWeek;
    return `${translatedDay} (${day.date})`;
};

    const handlePreviousWeek = () => {
        if (offset === 0) return;
        setOffset(prev => prev - 1);
    };

    const handleOpenSpecialHoursModal = () => {
        setSelectedDate('');
        setHours({ openHour: '', closeHour: '' });
        setOpenSpecialHoursModal(true);
    };

    const handleOpenRegularHoursModal = () => {
        setSelectedDayOfWeek('');
        setHours({ openHour: '', closeHour: '' });
        setOpenRegularHoursModal(true);
    };

    const handleCloseModals = () => {
        setOpenSpecialHoursModal(false);
        setOpenRegularHoursModal(false);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setHours(prev => ({ ...prev, [name]: value }));
    };

    const handleDateChange = (e) => {
        setSelectedDate(e.target.value);
    };

    const handleDayOfWeekChange = (e) => {
        setSelectedDayOfWeek(e.target.value);
    };

    const formatDateRange = () => {
        if (workingHours.length === 0) return '';
        const firstDay = workingHours[0].date;
        const lastDay = workingHours[workingHours.length - 1].date;
        return `${firstDay} - ${lastDay}`;
    };

    const formatTime = (timeString) => {
        if (!timeString) return '';
        return timeString.substring(0, 5);
    };

    const validateDate = (dateString) => {
        const selectedDate = new Date(dateString);
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        return selectedDate >= today;
    };
    const validateHours = (openHour, closeHour) => {
        if (!openHour || !closeHour) return true;
        return closeHour > openHour;
    };

   const handleSubmitSpecialHours = async () => {

    if (!validateDate(selectedDate)) {
            alert(dictionary.clubHoursPage.pastDateError);
            return;
        }

        if (!validateHours(hours.openHour, hours.closeHour)) {
            alert(dictionary.clubHoursPage.invalidHoursError);
            return;
        }
    if (!selectedDate || !hours.openHour || !hours.closeHour) {
        alert(dictionary.clubHoursPage.fillAllFieldsError);
        return;
    }

    try {
        const response = await setSpecialWorkingHours(
            selectedDate,
            hours.openHour,
            hours.closeHour
        );
        
        if (!response.ok) throw new Error('Failed to set special hours');
        
        fetchWorkingHours();
        handleCloseModals();
    } catch (error) {
        console.error('Error while trying to set special hours:', error);
        alert(dictionary.clubHoursPage.errorSettingHours);
    }
};

    const handleSubmitRegularHours = async () => {
        if (!selectedDayOfWeek || !hours.openHour || !hours.closeHour) {
            alert(dictionary.clubHoursPage.fillAllFieldsError);
            return;
        }

        try {
            const response = await setRegularWorkingHours(
                selectedDayOfWeek,
                hours.openHour,
                hours.closeHour
            );
            
            if (!response.ok) throw new Error('Failed to set regular hours');
            
            fetchWorkingHours();
            handleCloseModals();
        } catch (error) {
            console.error('Error while trying to set regular hours:', error);
            alert(dictionary.clubHoursPage.errorSettingHours);
        }
    };

return (
        <>
            <Box sx={{
                width: '64%',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                flexGrow: 1,
                marginLeft: 'auto',
                marginRight: 'auto',
                marginTop: '10vh',
            }}>
                <Header>{dictionary.clubHoursPage.workingHoursLabel}</Header>

                <Box sx={{
                    minHeight: '60vh',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    backgroundColor: 'white',
                    padding: '1rem',
                    display: 'flex',
                    flexDirection: 'column',
                }}>
                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center',
                        marginBottom: '0.5rem',
                        paddingBottom: '0.5rem',
                        borderBottom: '1px solid #eee',
                        gap: '0.5rem'
                    }}>
                        <Button onClick={handlePreviousWeek}
                            variant="contained"
                            sx={{
                                fontSize: '1rem',
                                color: 'black',
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                borderRadius: '30px',
                                minWidth: '40px',
                                height: '40px',
                            }}>
                            {'<'}
                        </Button>

                        <Typography variant="h6" sx={{
                            fontWeight: 'bold',
                            fontSize: '1.1rem',
                            color: 'black',
                            minWidth: '300px',
                            textAlign: 'center',
                            padding: '0 0.5rem'
                        }}>
                            {formatDateRange()}
                        </Typography>

                        <Button onClick={handleNextWeek}
                            variant="contained"
                            sx={{
                                fontSize: '1rem',
                                color: 'black',
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                borderRadius: '30px',
                                minWidth: '40px',
                                height: '40px',
                            }}>
                            {'>'}
                        </Button>
                    </Box>
                    <Box sx={{ 
                        flexGrow: 1,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center'
                    }}>
                        {
                         workingHours.length === 0 ? (
                            <Typography sx={{ 
                                color: 'black',
                                textAlign: 'center',
                                width: '100%'
                            }}>
                                {dictionary.clubHoursPage.noDataLabel}
                            </Typography>
                             ) : (
                            <Box sx={{
                                width: '100%',
                                maxWidth: '1200px'
                            }}>
                                {workingHours.map((day, index) => (
                                    <Box key={index} sx={{
                                        display: 'grid',
                                        gridTemplateColumns: '2fr 1fr 2fr',
                                        alignItems: 'center',
                                        marginBottom: '1rem',
                                        padding: '1rem',
                                        backgroundColor: index % 2 === 0 ? '#f9f9f9' : 'white',
                                        borderRadius: '10px',
                                    }}>
                                        <Typography sx={{
                                            color: 'black',
                                            fontWeight: 'bold',
                                            textAlign: 'right',
                                        }}>
                                            {formatDayWithDate(day)}
                                        </Typography>
                                        <Box></Box>
                                        <Typography sx={{
                                            color: 'black',
                                            textAlign: 'left',
                                        }}>
                                            {formatTime(day.openHour)} - {formatTime(day.closeHour)}
                                        </Typography>                                     
                                    </Box>
                                ))}
                            </Box>
                        )}
                    </Box>

                    <Box sx={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: 'center',
                        columnGap: "4vw",
                        alignItems: 'center',
                        marginTop: '2rem',
                        marginBottom: '1rem'
                    }}>
                        <GreenButton
                            onClick={handleOpenSpecialHoursModal}
                            style={{
                                fontSize: '0.9rem',
                                padding: "8px 16px",
                                backgroundColor: '#8edfb4',
                                color: 'black',
                                fontWeight: 'bold',
                            }}
                        >
                            {dictionary.clubHoursPage.setSpecialHoursButton}
                        </GreenButton>

                        <GreenButton
                            onClick={handleOpenRegularHoursModal}
                            style={{
                                fontSize: '0.9rem',
                                padding: "8px 16px",
                                backgroundColor: '#8edfb4',
                                color: 'black',
                                fontWeight: 'bold',
                            }}
                        >
                            {dictionary.clubHoursPage.setRegularHoursButton}
                        </GreenButton>
                    </Box>
                </Box>
            </Box>

            <Modal   //specjalne godziny
                open={openSpecialHoursModal}
                onClose={handleCloseModals}
                aria-labelledby="special-hours-modal-title"
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <Box sx={{
                    backgroundColor: 'white',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    padding: '3rem',
                    width: '500px',
                    maxWidth: '90%',
                }}>
                    <Typography variant="h5" component="h2" sx={{
                        marginBottom: '2.5rem',
                        fontWeight: 'bold',
                        color: 'black'
                    }}>
                        {dictionary.clubHoursPage.setSpecialHoursTitle}
                    </Typography>

                    <CustomInput
                        label={dictionary.clubHoursPage.selectDateLabel}
                        name="date"
                        type="date"
                        value={selectedDate}
                        onChange={handleDateChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                        inputProps={{min: new Date().toISOString().split('T')[0]}}
                    />

                    <CustomInput
                        label={dictionary.clubHoursPage.openHourLabel}
                        name="openHour"
                        type="time"
                        value={hours.openHour}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                    />

                    <CustomInput
                        label={dictionary.clubHoursPage.closeHourLabel}
                        name="closeHour"
                        type="time"
                        value={hours.closeHour}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                        error={!validateHours(hours.openHour, hours.closeHour)}
                        helperText={!validateHours(hours.openHour, hours.closeHour) ? 
                        dictionary.clubHoursPage.invalidHoursError : ''}
                    />

                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        marginTop: '1.5rem'
                    }}>
                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleCloseModals}
                        >
                            {dictionary.clubHoursPage.cancelButton}
                        </Button>

                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#8edfb4',
                                '&:hover': { backgroundColor: '#7ecba3' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleSubmitSpecialHours}
                        >
                            {dictionary.clubHoursPage.saveButton}
                        </Button>
                    </Box>
                </Box>
            </Modal>

            <Modal //godziny regularne
                open={openRegularHoursModal}
                onClose={handleCloseModals}
                aria-labelledby="regular-hours-modal-title"
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <Box sx={{
                    backgroundColor: 'white',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    padding: '3rem',
                    width: '500px',
                    maxWidth: '90%',
                }}>
                    <Typography variant="h5" component="h2" sx={{
                        marginBottom: '2.5rem',
                        fontWeight: 'bold',
                        color: 'black'
                    }}>
                        {dictionary.clubHoursPage.setRegularHoursTitle}
                    </Typography>

                        <FormControl fullWidth sx={{ mb: 4 }}>
                            <InputLabel sx={{ color: 'black' }}>
                                {dictionary.clubHoursPage.selectDayLabel}
                            </InputLabel>
                            <Select
                                value={selectedDayOfWeek}
                                onChange={handleDayOfWeekChange}
                                label={dictionary.clubHoursPage.selectDayLabel}
                                sx={{
                                    color: 'black',
                                    borderRadius: '40px',
                                    '& .MuiOutlinedInput-root': {
                                    borderRadius: '40px',
                                    },
                                    '& fieldset': {
                                    borderRadius: '40px',
                                    },
                                }}
                            >
                                <MenuItem value="monday">{dictionary.days.monday}</MenuItem>
                                <MenuItem value="tuesday">{dictionary.days.tuesday}</MenuItem>
                                <MenuItem value="wednesday">{dictionary.days.wednesday}</MenuItem>
                                <MenuItem value="thursday">{dictionary.days.thursday}</MenuItem>
                                <MenuItem value="friday">{dictionary.days.friday}</MenuItem>
                                <MenuItem value="saturday">{dictionary.days.saturday}</MenuItem>
                                <MenuItem value="sunday">{dictionary.days.sunday}</MenuItem>
                            </Select>
                        </FormControl>

                    <CustomInput
                        label={dictionary.clubHoursPage.openHourLabel}
                        name="openHour"
                        type="time"
                        value={hours.openHour}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                    />
                    <CustomInput
                        label={dictionary.clubHoursPage.closeHourLabel}
                        name="closeHour"
                        type="time"
                        value={hours.closeHour}
                        onChange={handleInputChange}
                        fullWidth
                        additionalStyles={{ marginBottom:'2.5vh', 
                            '& .MuiInputLabel-root': { color: 'black' },
                            '& .MuiOutlinedInput-root': {
                              color: 'black',
                              borderRadius:'40px',
                            },}}
                        InputLabelProps={{ shrink: true }}
                        error={!validateHours(hours.openHour, hours.closeHour)}
                        helperText={!validateHours(hours.openHour, hours.closeHour) ? 
                        dictionary.clubHoursPage.invalidHoursError : ''}
                    />

                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        marginTop: '1.5rem'
                    }}>
                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#FFE3B3',
                                '&:hover': { backgroundColor: '#e8d2a1' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleCloseModals}
                        >
                            {dictionary.clubHoursPage.cancelButton}
                        </Button>

                        <Button
                            variant="contained"
                            sx={{
                                backgroundColor: '#8edfb4',
                                '&:hover': { backgroundColor: '#7ecba3' },
                                minWidth: '120px',
                                height: '42px',
                                fontSize: '1rem',
                                color: 'black',
                                fontWeight: 'bold',
                                borderRadius: '20px',
                            }}
                            onClick={handleSubmitRegularHours}
                        >
                            {dictionary.clubHoursPage.saveButton}
                        </Button>
                    </Box>
                </Box>
            </Modal>
        </>
    );
}
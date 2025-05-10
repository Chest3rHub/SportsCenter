import { useEffect, useState } from 'react';
import { Box, Typography, IconButton, Tooltip } from '@mui/material';
import { ArrowBackIos, ArrowForwardIos } from '@mui/icons-material';
import ArrowBackIosRoundedIcon from '@mui/icons-material/ArrowBackIosRounded';
import ArrowForwardIosRoundedIcon from '@mui/icons-material/ArrowForwardIosRounded';
import getSchedule from '../api/getSchedule';


// co 30 min od 10:00 do 22:00
const HOURS = Array.from({ length: 24 }, (_, i) => 10 * 60 + i * 30); 
const DAYS_IN_WEEK = 7;

function getStartOfWeek(date) {
    const day = date.getDay();
    const diff = (day === 0 ? -6 : 1 - day);
    const start = new Date(date);
    start.setDate(date.getDate() + diff);
    start.setHours(0, 0, 0, 0);
    return start;
}

function addDays(date, days) {
    const newDate = new Date(date);
    newDate.setDate(newDate.getDate() + days);
    return newDate;
}

function addWeeks(date, weeks) {
    return addDays(date, weeks * 7);
}

function getDayOfTheWeekString(dayOfWeek) {
    // to nazwa dnia tyg
    // {date.toLocaleDateString('pl-PL', { weekday: 'long' })}
    switch (dayOfWeek) {
        case 0: return 'Nd';
        case 1: return 'Pon';
        case 2: return 'Wt';
        case 3: return 'Śr';
        case 4: return 'Czw';
        case 5: return 'Pt';
        case 6: return 'Sb';
        default: return '';
    }
}
function getActivityColor(activityName) {
    switch (activityName) {
        case 'Badminton': return '#F46C63';
        case 'Squash': return '#AFEBE4';
        case 'Tenis': return '#AFEBBC';
        default: return '#FFE3B3';
    }
}

export default function Timetable() {
    const [currentWeekStart, setCurrentWeekStart] = useState(getStartOfWeek(new Date()));
    const [events, setEvents] = useState([]);
    //const [loading, setLoading] = useState(true);

    // to gdybym chcial offset ustawiac ten sam przy powrocie z zapisywania sie np na zajecia
    //const [offset, setOffset] = useState(location.state?.offsetFromLocation ? location.state.offsetFromLocation : 0);
    const [offset, setOffset] = useState(0);

    const handlePrevWeek = () => {setCurrentWeekStart(addWeeks(currentWeekStart, -1));
        setOffset(prevOffset => prevOffset - 1);
    };
    const handleNextWeek = () => {setCurrentWeekStart(addWeeks(currentWeekStart, 1));
        setOffset(prevOffset => prevOffset + 1);
    };

    const daysOfWeek = Array.from({ length: DAYS_IN_WEEK }, (_, i) => addDays(currentWeekStart, i));

    useEffect(() => {
        getSchedule(offset)
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                if (Array.isArray(data)) {
                    // kombinacje zeby uzyskac ten dayIndex potrzebny do mapowania w kalendarzu...
                    // day: 0 to pon, day: 1 to wt itd
                    const processedEvents = data.map(event => {
                        const dateStr = event.date.split('T')[0]; 
                        const eventStart = new Date(`${dateStr}T${event.startTime}`);
                        const eventEnd = new Date(`${dateStr}T${event.endTime}`);
    
                        const startOfWeek = new Date(currentWeekStart);
                        const day = Math.floor((eventStart - startOfWeek) / (1000 * 60 * 60 * 24));
    
                        const startTime = eventStart.toTimeString().slice(0, 5);
                        const endTime = eventEnd.toTimeString().slice(0, 5);
    
                        return {
                            ...event,
                            day,
                            startTime,
                            end: endTime,
                        };
                    }).filter(e => e.day >= 0 && e.day < 7);
    
                    setEvents(processedEvents);
                } else {
                    console.error('Otrzymane dane nie są tablicą:', data);
                }
               // setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getSchedule:', error);
            });
    }, [offset]);

    return (
        <Box sx={{
            p: 3,
            width: '64%',
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            flexGrow: 1,
            marginLeft: 'auto',
            marginRight: 'auto',
        }}>
            <Box sx={{
                display: 'flex',
                flexDirection: 'row',
                columnGap: '1vw',
                alignItems: 'center',
                justifyContent: 'center',
                marginBottom: '3vh',
            }} >
                <IconButton onClick={handlePrevWeek}><ArrowBackIosRoundedIcon sx={{ color: 'black', fontSize: '2.5rem' }} /></IconButton>
                <Box sx={{
                    minWidth: '30vw'
                }}>
                    <Box
                        sx={{
                            backgroundColor: '#AFEBBC',
                            boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',

                            borderRadius: '100px',
                            padding: '10px 0',
                            fontSize: '2rem',
                            color: '#000000',
                            textAlign: 'center',
                            margin: 'auto',
                            fontWeight: 'bold',


                        }}
                    >
                        Tydzień: {currentWeekStart.toLocaleDateString('pl-PL', { day: '2-digit', month: '2-digit' })} – {addDays(currentWeekStart, 6).toLocaleDateString('pl-PL', { day: '2-digit', month: '2-digit' })}
                    </Box>

                </Box>

                <IconButton onClick={handleNextWeek}><ArrowForwardIosRoundedIcon sx={{ color: 'black', fontSize: '2.5rem' }} /></IconButton>
            </Box>

            <Box display="grid" gridTemplateColumns="80px repeat(7, 1fr)" sx={{

                borderRadius: '20px',
                boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                backgroundColor: 'white',
                padding: '1.35rem',
            }}>
                <Box />
                {daysOfWeek.map((date, idx) => (
                    <Box key={idx} p={1} borderLeft="1px solid #ccc">
                        <Typography variant="subtitle2" align="center" sx={{ fontSize: '0.9rem', }}>
                            {date.toLocaleDateString('pl-PL')}
                        </Typography>
                        <Typography variant="subtitle2" align="center" sx={{ fontSize: '0.9rem', color: '#0AB68B', fontWeight: 'bold' }}>
                            {getDayOfTheWeekString(date.getDay())}
                        </Typography>
                    </Box>
                ))}
                {HOURS.map((minutesSinceMidnight, rowIdx) => {
                    const hour = Math.floor(minutesSinceMidnight / 60);
                    const minutes = minutesSinceMidnight % 60;
                    const timeLabel = `${hour.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;

                    return (
                        <>
                            <Box height={40} p={0.5} borderTop="1px solid #eee">
                                <Typography variant="caption" sx={{ fontSize: '0.8rem', }}>{timeLabel}</Typography>
                            </Box>

                            {Array.from({ length: DAYS_IN_WEEK }, (_, dayIdx) => {
                                // ta opcja jesli maja sie wyswietlac tylko w tej komorce co sie zaczynaja
                               // const cellEvents = events.filter(e => e.day === dayIdx && e.startTime === timeLabel );

                                // ta opcpja jesli maja sie wyswietlac te eventy w komorkach pod spodem az do godziny konca
                               const cellEvents = events.filter(e => e.day === dayIdx && (e.startTime === timeLabel || (e.startTime <= timeLabel && e.end > timeLabel)));     
                                return (
                                    <Box
                                        key={dayIdx}
                                        height={40}
                                        borderTop="1px solid #eee"
                                        borderLeft="1px solid #ccc"
                                        sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5, p: 0.5 }}
                                    >
                                        {cellEvents.map((event, i) => (
                                            <Tooltip key={i} title={`${event.groupName ? event.groupName + ': ' : ''} ${event.description}`}>
                                                <Box
                                                    sx={{

                                                        color: 'white',
                                                        px: 1,
                                                        borderRadius: 1,
                                                        fontSize: '0.5rem',
                                                        whiteSpace: 'nowrap',
                                                        overflow: 'hidden',
                                                        textOverflow: 'ellipsis',
                                                        maxWidth: '100%',
                                                        minWidth: '1.1rem',
                                                        minHeight: '0.5rem',
                                                        maxHeight: '0.5rem',
                                                        backgroundColor: getActivityColor(event.groupName),
                                                    }}
                                                >

                                                </Box>
                                            </Tooltip>
                                        ))}
                                    </Box>
                                );
                            })}
                        </>
                    );
                })}
            </Box>
        </Box>
    );
}

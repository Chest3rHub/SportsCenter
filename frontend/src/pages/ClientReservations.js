import React, { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { SportsContext } from '../context/SportsContext';
import Header from '../components/Header';
import GreenBackground from '../components/GreenBackground';
import ReservationButton from '../components/buttons/ReservationButton';
import { Box } from '@mui/material';

export default function ClientReservations() {

    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();

    function handleCreateReservation() {
        navigate(`/Create-single-reservation-yourself`, {
        });
    }

    return (
        <>
            <GreenBackground gap={"4vh"} height={"76.5vh"}>
            <Header>{dictionary.clientReservations.title}</Header>
            <Box
                sx={{
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: 'center',
                    alignItems: 'flex-start',
                    columnGap: "4vw",
                    marginTop: '0vh',
                }}
            >       
                <ReservationButton
                    onClick={handleCreateReservation}
                    style={{
                        minWidth: '11vw',
                        height: '2.8rem',
                        paddingLeft: '1rem',
                        paddingRight: '1rem',
                        fontSize: '1rem',
                        whiteSpace: 'nowrap',
                    }}
                >
                    {dictionary.clientReservations.createReservation}
                </ReservationButton>  
            </Box>   
            </GreenBackground>
        </>
    );
}

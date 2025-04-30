import { useContext, useEffect, useState } from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { SportsContext } from '../context/SportsContext';
import Header from '../components/Header';
import GreenBackground from '../components/GreenBackground';
import ReservationButton from '../components/buttons/ReservationButton';
import { Box, Typography, Modal } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import getClientReservations from '../api/getClientReservations';
import cancelReservation from "../api/cancelReservation";
import ChangePageButton from "../components/buttons/ChangePageButton";
import GreenButton from "../components/buttons/GreenButton";

export default function ClientReservations() {

    const { dictionary, toggleLanguage } = useContext(SportsContext);
    const navigate = useNavigate();
    const location = useLocation();

    const [reservations, setReservations] = useState([]);
    const [loading, setLoading] = useState(true);

    const [selectedReservation, setSelectedReservation] = useState(null);
    const [offset, setOffset] = useState(location.state?.offsetFromLocation ? location.state.offsetFromLocation : 0);
    const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);

    const handleOpen = (reservation) => setSelectedReservation(reservation);;
    const handleClose = () => setSelectedReservation(null);;

    const maxReservationsPerPage = 6;
    const reservationsRequiredToEnablePagination = 7;

    useEffect(() => {
        getClientReservations(offset)
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                setReservations(data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getClientReservations:', error);
            })
    }, [offset, stateToTriggerUseEffectAfterDeleting]);

    function handleCreateReservation() {
        navigate(`/Create-single-reservation-yourself`, {
        });
    }

    function handleMoveReservation(reservation) {
        navigate('/move-reservation', {
            state: {
                id: reservation.reservationId,
                offsetFromLocation: offset
            }
        });
    }

    function getReservationStatus(reservation) {
        const now = new Date();
        const startTime = new Date(reservation.startTime);
        const endTime = new Date(reservation.endTime);

        if (reservation.isMoneyRefunded) {
            return dictionary.clientReservations.statusRefunded;
        }
    
        if (reservation.isReservationCanceled) {
            return dictionary.clientReservations.statusCanceled;
        }
    
        if (endTime < now) {
            return dictionary.clientReservations.statusCompleted;
        }

        if (reservation.isReservationPaid) {
            return dictionary.clientReservations.statusPaid;
        }
    
        if (startTime > now) {
            return dictionary.clientReservations.statusPlanned;
        }
    
        return dictionary.clientReservations.statusUnknown;
    } 

    function canCancelOrMoveReservation(reservation) {
        if (!reservation) return false;

        if (reservation.isReservationCanceled || reservation.isMoneyRefunded) return false;
    
        const now = new Date();
        const startTime = new Date(reservation.startTime);
        const hoursDifference = (startTime.getTime() - now.getTime()) / (1000 * 60 * 60);
        //console.log(hoursDifference);
        return hoursDifference >= 24;
    }    

    function handleCancelReservation(id) {
        handleClose();
        cancelReservation(id)
            .then(response => { })
            .then(data => {
                console.log("Rezerawcja odwołana:", data);
                setStateToTriggerUseEffectAfterDeleting((prev) => !prev);
            })
            .catch(error => {
                console.error("Błąd podczas odwoływania rezerwacji:", error);
            });
    }
    
    function handleNextPage() {
        if (reservations.length < 6) {
            return;
        }
        setOffset(prevOffset => prevOffset + 1);
    };

    function handlePreviousPage() {
        if (offset === 0) {
            return;
        }
        setOffset(prevOffset => prevOffset - 1);
    };

    const limitedReservations = reservations.slice(0, maxReservationsPerPage);

    return (
        <>
            <GreenBackground gap={"4vh"} height={"76.5vh"}>
            <Box
                sx={{
                    width: '90%',
                    display: 'flex',
                    flexDirection: 'column',
                    justifyContent: 'center',
                    flexGrow: 1,
                    marginLeft: 'auto',
                    marginRight: 'auto',
                }}
            >        
                <Header>{dictionary.clientReservations.title}</Header>
                <Box
                    sx={{
                        height: '55vh',
                        borderRadius: '20px',
                        boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                        backgroundColor: 'white',
                        padding: '1.35rem',
                    }}
                >
                    <Box
                        sx={{
                            display: 'flex',
                            alignContent: 'start',
                            alignItems: 'center',
                            width: '73%',
                            gap: '2%',
                            marginBottom: '3vh',
                        }}
                    >
                        <SmallGreenHeader width={'33%'}>{dictionary.clientReservations.startOfReservationLabel}</SmallGreenHeader>
                        <SmallGreenHeader width={'33%'}>{dictionary.clientReservations.endOfReservationLabel}</SmallGreenHeader>
                        <SmallGreenHeader width={'33%'}>{dictionary.clientReservations.statusLabel}</SmallGreenHeader>
                    </Box>
                    {limitedReservations.map((reservation) => (<Box
                        sx={{
                            marginTop: '1vh',
                            display: 'flex',
                            alignContent: 'start',
                            alignItems: 'center',
                            width: '100%',
                            padding: '0.6rem 0px',
                        }}
                    >
                        <Box
                            sx={{
                                width: '79.8%',
                                borderRadius: '70px',
                                backgroundColor: 'white',
                                boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center',
                                paddingTop: '0.6rem',
                                paddingBottom: '0.4rem',
                            }}
                        >
                            <Box
                                sx={{
                                    width: '33%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {new Date(reservation.startTime).toLocaleString('pl-PL', {
                                    day: '2-digit',
                                    month: '2-digit',
                                    year: 'numeric',
                                    hour: '2-digit',
                                    minute: '2-digit'
                                    })}
                                </Typography>

                            </Box>
                            <Box
                                sx={{
                                    width: '33%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {new Date(reservation.endTime).toLocaleString('pl-PL', {
                                    day: '2-digit',
                                    month: '2-digit',
                                    year: 'numeric',
                                    hour: '2-digit',
                                    minute: '2-digit'
                                    })}
                                </Typography>

                            </Box>
                            <Box
                                sx={{
                                    width: '33%',
                                    textAlign: 'center',


                                }}
                            >
                                <Typography>
                                    {getReservationStatus(reservation)}
                                </Typography>
                            </Box>
                        </Box>

                        <ReservationButton backgroundColor={"#f0aa4f"} onClick={() => handleMoveReservation(reservation)} disabled={!canCancelOrMoveReservation(reservation)}>{dictionary.clientReservations.moveLabel}</ReservationButton>
                        <ReservationButton backgroundColor={"#F46C63"} onClick={() => handleOpen(reservation)} disabled={!canCancelOrMoveReservation(reservation)}>{dictionary.clientReservations.cancelLabel}</ReservationButton>
                    </Box>))}
                </Box>  
                <Modal
                    open={selectedReservation}
                    onClose={handleClose}
                >
                    <Box
                        sx={{
                            width: '30vw',
                            height: '30vh',
                            position: 'absolute',
                            top: '50vh',
                            left: '50vw',
                            transform: 'translate(-50%, -50%)',
                            backgroundColor: 'white',
                            borderRadius: '10px',
                            boxShadow: 24,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                            justifyContent: 'center',
                        }}
                    >
                        <Typography sx={{
                            fontWeight: 'Bold',
                            fontSize: '2.2rem',
                            marginTop: '1vh',
                        }} >
                            {dictionary.clientReservations.confirmLabel}
                        </Typography>
                        <Typography sx={{
                            color: 'black',
                            fontSize: '1.5rem',
                            }}>
                            {selectedReservation ? new Date(selectedReservation.startTime).toLocaleString('pl-PL', {
                                day: '2-digit',
                                month: '2-digit',
                                year: 'numeric',
                                hour: '2-digit',
                                minute: '2-digit'
                            }) : ''}
                        </Typography>

                        <Box sx={{ display: 'flex', gap: '3rem', marginTop: '1rem' }}>
                            <GreenButton onClick={() => { handleClose() }} style={{ maxWidth: "10vw", backgroundColor: "#F46C63", minWidth: '7vw' }} hoverBackgroundColor={'#c3564f'}>{dictionary.clientReservations.noLabel}</GreenButton>
                            <GreenButton onClick={() => { handleCancelReservation(selectedReservation.reservationId) }} style={{ maxWidth: "10vw", minWidth: '7vw' }}>{dictionary.clientReservations.yesLabel}</GreenButton>
                        </Box>
                    </Box>
                </Modal>
                {<Box sx={{
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: 'center',
                    columnGap: "4vw",
                    marginTop: '5vh',
                }}>
                    <ChangePageButton disabled={offset === 0} onClick={handlePreviousPage} backgroundColor={"#F46C63"} minWidth={"10vw"}>{dictionary.newsPage.previousLabel}</ChangePageButton>
                    <ChangePageButton disabled={reservations.length < reservationsRequiredToEnablePagination} onClick={handleNextPage} backgroundColor={"#8edfb4"} minWidth={"10vw"}>{dictionary.newsPage.nextLabel}</ChangePageButton>
                </Box>}

                <Box sx={{
                    position: "fixed",
                    top: "12vh",
                    right: "2vw",
                    minWidth: "17vw"
                }}>

                <GreenButton onClick={handleCreateReservation}
                    style={{
                        fontSize: '0.8rem',
                        padding: "3px 8px",
                        backgroundColor: '#8edfb4',
                        color: 'black',
                        fontWeight: 'bold',
                    }}
                >
                {dictionary.clientReservations.createReservation}
                </GreenButton>
                </Box>
            </Box>
            </GreenBackground>
        </>
    );
}

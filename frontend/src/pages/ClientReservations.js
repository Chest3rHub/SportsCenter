import { useContext, useEffect, useState } from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import { SportsContext } from '../context/SportsContext';
import Header from '../components/Header';
import GreenBackground from '../components/GreenBackground';
import ReservationButton from '../components/buttons/ReservationButton';
import { Box, Typography, Modal, Backdrop, Avatar } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import getClientReservations from '../api/getClientReservations';
import cancelReservation from "../api/cancelReservation";
import ChangePageButton from "../components/buttons/ChangePageButton";
import GreenButton from "../components/buttons/GreenButton";
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

export default function ClientReservations() {
  const { dictionary } = useContext(SportsContext);
  const navigate = useNavigate();
  const location = useLocation();

  const [reservations, setReservations] = useState([]);
  const [loading, setLoading] = useState(true);

  const [selectedReservation, setSelectedReservation] = useState(null);
  const [offset, setOffset] = useState(location.state?.offsetFromLocation || 0);
  const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);

  // Stany do Backdrop błędu
  const [openCancelErrorBackdrop, setOpenCancelErrorBackdrop] = useState(false);
  const [cancelErrorMessage, setCancelErrorMessage] = useState("");

  const handleOpen = (reservation) => setSelectedReservation(reservation);
  const handleClose = () => setSelectedReservation(null);

  const maxReservationsPerPage = 6;
  const reservationsRequiredToEnablePagination = 7;

  useEffect(() => {
    getClientReservations(offset)
      .then(response => response.json())
      .then(data => {
        setReservations(data);
        setLoading(false);
      })
      .catch(error => {
      //  console.error('Błąd podczas wywoływania getClientReservations:', error);
      });
  }, [offset, stateToTriggerUseEffectAfterDeleting]);

  function handleCreateReservation() {
    navigate(`/Create-single-reservation-yourself`);
  }

  function handleMoveReservation(reservation) {
    navigate('/move-reservation', {
      state: {
        id: reservation.reservationId,
        courtId: reservation.courtId,
        startTime: reservation.startTime,
        endTime: reservation.endTime,
        trainerId: reservation.trainerId,
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

  function canCancelReservation(reservation) {
    if (!reservation) return false;
    if (reservation.isReservationCanceled || reservation.isMoneyRefunded) return false;
    const now = new Date();
    const startTime = new Date(reservation.startTime);
    const hoursDifference = (startTime.getTime() - now.getTime()) / (1000 * 60 * 60);
    return hoursDifference >= 2;
  }

  function canMoveReservation(reservation) {
    if (!reservation) return false;
    if (reservation.isReservationCanceled || reservation.isMoneyRefunded) return false;
    const now = new Date();
    const startTime = new Date(reservation.startTime);
    const hoursDifference = (startTime.getTime() - now.getTime()) / (1000 * 60 * 60);
    return hoursDifference >= 24;
  }

  function determineCancelErrorByStatus(status, serverMessage) {
    switch (status) {
      case 411:
        return dictionary.clientReservations.reservationNotFound || serverMessage;
      case 412:
        return dictionary.clientReservations.tooLateToCancel || serverMessage;
      case 413:
        return dictionary.clientReservations.alreadyCanceled || serverMessage;
      default:
        return serverMessage;
    }
  }

  // Zmieniony handler odwoływania rezerwacji
  function handleCancelReservation(id) {
    handleClose();

    cancelReservation(id)
      .then(response => {
        if (!response.ok) {
          return response.json().then(err => {
            const msg = err.message || dictionary.clientReservations.defaultCancelError;
            const errorText = determineCancelErrorByStatus(response.status, msg);
            throw new Error(errorText);
          });
        }
        return response.json();
      })
      .then(data => {
     //   console.log("Rezerwacja odwołana:", data);
        setStateToTriggerUseEffectAfterDeleting(prev => !prev);
      })
      .catch(error => {
     //   console.error("Błąd podczas odwoływania rezerwacji:", error);
        setCancelErrorMessage(error.message);
        setOpenCancelErrorBackdrop(true);
      });
  }

  function handleNextPage() {
    if (reservations.length < maxReservationsPerPage) {
      return;
    }
    setOffset(prevOffset => prevOffset + 1);
  }

  function handlePreviousPage() {
    if (offset === 0) {
      return;
    }
    setOffset(prevOffset => prevOffset - 1);
  }

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
              <SmallGreenHeader width={'25%'}>
                {dictionary.clientReservations.dateLabel}
              </SmallGreenHeader>
              <SmallGreenHeader width={'25%'}>
                {dictionary.clientReservations.timeLabel}
              </SmallGreenHeader>
              <SmallGreenHeader width={'25%'}>
                {dictionary.clientReservations.courtLabel}
              </SmallGreenHeader>
              <SmallGreenHeader width={'25%'}>
                {dictionary.clientReservations.statusLabel}
              </SmallGreenHeader>
            </Box>

            {limitedReservations.map((reservation) => (
              <Box
                key={reservation.reservationId}
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
                  <Box sx={{ width: '25%', textAlign: 'center' }}>
                    <Typography>
                      {new Date(reservation.startTime).toLocaleDateString('pl-PL')}
                    </Typography>
                  </Box>
                  <Box sx={{ width: '25%', textAlign: 'center' }}>
                    <Typography>
                      {`${new Date(reservation.startTime).toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' })} - ${new Date(reservation.endTime).toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' })}`}
                    </Typography>
                  </Box>
                  <Box sx={{ width: '25%', textAlign: 'center' }}>
                    <Typography>
                      {reservation.courtName}
                    </Typography>
                  </Box>
                  <Box sx={{ width: '25%', textAlign: 'center' }}>
                    <Typography>
                      {getReservationStatus(reservation)}
                    </Typography>
                  </Box>
                </Box>

                <ReservationButton
                  backgroundColor={"#f0aa4f"}
                  onClick={() => handleMoveReservation(reservation)}
                  disabled={!canMoveReservation(reservation)}
                >
                  {dictionary.clientReservations.moveLabel}
                </ReservationButton>
                <ReservationButton
                  backgroundColor={"#F46C63"}
                  onClick={() => handleOpen(reservation)}
                  disabled={!canCancelReservation(reservation)}
                >
                  {dictionary.clientReservations.cancelLabel}
                </ReservationButton>
              </Box>
            ))}
          </Box>

          <Modal
            open={!!selectedReservation}
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
              <Typography
                sx={{
                  fontWeight: 'Bold',
                  fontSize: '2.2rem',
                  marginTop: '1vh',
                }}
              >
                {dictionary.clientReservations.confirmLabel}
              </Typography>
              <Typography
                sx={{
                  color: 'black',
                  fontSize: '1.5rem',
                }}
              >
                {selectedReservation
                  ? new Date(selectedReservation.startTime).toLocaleString('pl-PL', {
                      day: '2-digit',
                      month: '2-digit',
                      year: 'numeric',
                      hour: '2-digit',
                      minute: '2-digit'
                    })
                  : ''}
              </Typography>

              <Box sx={{ display: 'flex', gap: '3rem', marginTop: '1rem' }}>
                <GreenButton
                  onClick={handleClose}
                  style={{ maxWidth: "10vw", backgroundColor: "#F46C63", minWidth: '7vw' }}
                  hoverBackgroundColor={'#c3564f'}
                >
                  {dictionary.clientReservations.noLabel}
                </GreenButton>
                <GreenButton
                  onClick={() => handleCancelReservation(selectedReservation.reservationId)}
                  style={{ maxWidth: "10vw", minWidth: '7vw' }}
                >
                  {dictionary.clientReservations.yesLabel}
                </GreenButton>
              </Box>
            </Box>
          </Modal>

          <Box
            sx={{
              display: "flex",
              flexDirection: "row",
              justifyContent: 'center',
              columnGap: "4vw",
              marginTop: '5vh',
            }}
          >
            <ChangePageButton
              disabled={offset === 0}
              onClick={handlePreviousPage}
              backgroundColor={"#F46C63"}
              minWidth={"10vw"}
            >
              {dictionary.clientReservations.previousLabel}
            </ChangePageButton>
            <ChangePageButton
              disabled={reservations.length < reservationsRequiredToEnablePagination}
              onClick={handleNextPage}
              backgroundColor={"#8edfb4"}
              minWidth={"10vw"}
            >
              {dictionary.clientReservations.nextLabel}
            </ChangePageButton>
          </Box>

          <Box
            sx={{
              position: "fixed",
              top: "12vh",
              right: "2vw",
              minWidth: "17vw"
            }}
          >
            <GreenButton
              onClick={handleCreateReservation}
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

      {/* BACKDROP Z BŁĘDEM ODWOŁANIA */}
      <Backdrop
        sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
        open={openCancelErrorBackdrop}
        onClick={() => setOpenCancelErrorBackdrop(false)}
      >
        <Box
          sx={{
            backgroundColor: "white",
            margin: 'auto',
            minWidth: '30vw',
            minHeight: '20vh',
            borderRadius: '20px',
            boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
            p: 4,
            textAlign: 'center',
          }}
        >
          <Typography
            sx={{ color: 'red', fontWeight: 'Bold', fontSize: '2rem', mb: 2 }}
          >
            {dictionary.clientReservations.cancelErrorTitle || "Błąd podczas odwoływania"}
          </Typography>
          <Typography
            sx={{ color: 'black', fontSize: '1rem', mb: 3 }}
          >
            {cancelErrorMessage}
          </Typography>
          <Avatar sx={{ width: "5rem", height: "5rem", mx: 'auto' }}>
            <SentimentDissatisfiedIcon sx={{ fontSize: "5rem", color: 'red', stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }} />
          </Avatar>
          <Box sx={{ mt: 2 }}>
            <GreenButton
              onClick={() => setOpenCancelErrorBackdrop(false)}
              style={{ backgroundColor: "#F46C63", minWidth: '8rem' }}
              hoverBackgroundColor={'#c3564f'}
            >
              {dictionary.clientReservations.okButtonLabel || "OK"}
            </GreenButton>
          </Box>
        </Box>
      </Backdrop>
    </>
  );
}

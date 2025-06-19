import Header from "../components/Header";
import { Box, Typography, Modal, Backdrop, Avatar } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import { useNavigate, useLocation } from 'react-router-dom';
import GreenButton from "../components/buttons/GreenButton";
import ChangePageButton from "../components/buttons/ChangePageButton";
import GreyButton from "../components/buttons/GreyButton";
import ReservationButton from "../components/buttons/ReservationButton";
import getAllReservations from "../api/getAllReservations";
import cancelClientReservation from "../api/cancelClientReservation";
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

export default function Reservations() {
  const { dictionary } = useContext(SportsContext);
  const navigate = useNavigate();
  const location = useLocation();

  const [reservations, setReservations] = useState([]);
  const [loading, setLoading] = useState(true);

  const maxReservationsPerPage = 6;
  const reservationsRequiredToEnablePagination = 7;

  const [selectedReservation, setSelectedReservation] = useState(null);
  const [offset, setOffset] = useState(location.state?.offsetFromLocation || 0);
  const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);

  const [openCancelErrorBackdrop, setOpenCancelErrorBackdrop] = useState(false);
  const [cancelErrorMessage, setCancelErrorMessage] = useState("");

  const handleOpenModal = (reservation) => setSelectedReservation(reservation);
  const handleCloseModal = () => setSelectedReservation(null);

  useEffect(() => {
    getAllReservations(offset)
      .then(response => response.json())
      .then(data => {
        setReservations(data);
        setLoading(false);
      })
      .catch(error => {
      //  console.error('Błąd podczas wywoływania getAllReservations:', error);
      });
  }, [offset, stateToTriggerUseEffectAfterDeleting]);

  const limitedReservations = reservations.slice(0, maxReservationsPerPage);

  function getReservationStatus(reservation) {
    const now = new Date();
    const startTime = new Date(reservation.startTime);
    const endTime = new Date(reservation.endTime);

    if (reservation.isMoneyRefunded) {
      return dictionary.reservationsPage.statusRefunded;
    }
    if (reservation.isReservationCanceled) {
      return dictionary.reservationsPage.statusCanceled;
    }
    if (endTime < now) {
      return dictionary.reservationsPage.statusCompleted;
    }
    if (startTime <= now && now <= endTime) {
      return dictionary.reservationsPage.statusOngoing;
    }
    if (reservation.isReservationPaid) {
      return dictionary.reservationsPage.statusPaid;
    }
    if (startTime > now) {
      return dictionary.reservationsPage.statusPlanned;
    }
    return dictionary.reservationsPage.statusUnknown;
  }

  function canCancelOrMoveReservation(reservation) {
    if (!reservation) return false;
    if (reservation.isReservationCanceled || reservation.isMoneyRefunded) return false;
    const now = new Date();
    const startTime = new Date(reservation.startTime);
    return startTime > now;
  }

  function handleShowReservationSummary() {
    navigate(`/reservation-summary`);
  }

  function handleAddReservation() {
    navigate(`/create-single-reservation-for-client`);
  }

  function handleShowMoreInfo(id) {
    navigate(`/get-reservation-with-id`, {
      state: { id: id, offsetFromLocation: offset }
    });
  }

  function determineCancelErrorByStatus(status, serverMessage) {
    switch (status) {
      case 411:
        return dictionary.reservationsPage.reservationNotFound || serverMessage;
      case 412:
        return dictionary.reservationsPage.tooLateToCancel || serverMessage;
      case 413:
        return dictionary.reservationsPage.alreadyCanceled || serverMessage;
      default:
        return serverMessage;
    }
  }

  function handleCancelReservation(clientEmail, id) {
    handleCloseModal();

    cancelClientReservation(clientEmail, id)
      .then(response => {
        if (!response.ok) {
          return response.json().then(err => {
            const msg = err.message || dictionary.reservationsPage.defaultCancelError;
            const errorText = determineCancelErrorByStatus(response.status, msg);
            throw new Error(errorText);
          });
        }
        return response.json();
      })
      .then(data => {
        setStateToTriggerUseEffectAfterDeleting(prev => !prev);
      })
      .catch(error => {
      //  console.error("Błąd podczas odwoływania rezerwacji:", error);
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

  return (
    <>
      <Box
        sx={{
          width: '64%',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          flexGrow: 1,
          marginLeft: 'auto',
          marginRight: 'auto',
        }}
      >
        <Header>{dictionary.reservationsPage.title}</Header>

        <Box
          sx={{
            backgroundColor: '#eafaf1',
            padding: '1.2rem',
            borderRadius: '20px',
            margin: '1.5rem 0',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            gap: '1.2rem',
            boxShadow: '0 6px 12px rgba(0, 0, 0, 0.1)',
          }}
        >
          <GreenButton
            onClick={() => handleAddReservation()}
            style={{
              minWidth: '7vw',
              height: '2.8rem',
              paddingLeft: '1rem',
              paddingRight: '1rem',
              fontSize: '0.9rem',
              whiteSpace: 'nowrap',
            }}
          >
            {dictionary.reservationsPage.addReservationLabel}
          </GreenButton>

          <GreyButton
            onClick={() => handleShowReservationSummary()}
            style={{
              minWidth: '7vw',
              height: '2.8rem',
              paddingLeft: '1rem',
              paddingRight: '1rem',
              fontSize: '0.9rem',
              whiteSpace: 'nowrap',
              backgroundColor: '#ccc',
              color: 'black'
            }}
          >
            {dictionary.reservationsPage.showSummaryLabel}
          </GreyButton>
        </Box>

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
              width: '77%',
              gap: '1%',
              marginBottom: '3vh',
            }}
          >
            <SmallGreenHeader width={'22%'}>
              {dictionary.reservationsPage.clientEmailLabel}
            </SmallGreenHeader>
            <SmallGreenHeader width={'14%'}>
              {dictionary.reservationsPage.courtLabel}
            </SmallGreenHeader>
            <SmallGreenHeader width={'16%'}>
              {dictionary.reservationsPage.dateLabel}
            </SmallGreenHeader>
            <SmallGreenHeader width={'16%'}>
              {dictionary.reservationsPage.timeLabel}
            </SmallGreenHeader>
            <SmallGreenHeader width={'10%'}>
              {dictionary.reservationsPage.isEquipmentReservedLabel}
            </SmallGreenHeader>
            <SmallGreenHeader width={'18%'}>
              {dictionary.reservationsPage.statusLabel}
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
                padding: '0.3rem 0px',
              }}
            >
              <Box
                sx={{
                  width: '100%',
                  borderRadius: '70px',
                  backgroundColor: 'white',
                  boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  paddingTop: '0.6rem',
                  paddingBottom: '0.4rem',
                  gap: '2%',
                }}
              >
                <Box sx={{ width: '22%', textAlign: 'center' }}>
                  <Typography>
                    {reservation.clientEmail}
                  </Typography>
                </Box>

                <Box sx={{ width: '14%', textAlign: 'center' }}>
                  <Typography>
                    {reservation.courtName}
                  </Typography>
                </Box>

                <Box sx={{ width: '16%', textAlign: 'center' }}>
                  <Typography>
                    {new Date(reservation.startTime).toLocaleDateString('pl-PL')}
                  </Typography>
                </Box>

                <Box sx={{ width: '16%', textAlign: 'center' }}>
                  <Typography>
                    {`${new Date(reservation.startTime).toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' })} - ${new Date(reservation.endTime).toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' })}`}
                  </Typography>
                </Box>

                <Box sx={{ width: '10%', textAlign: 'center' }}>
                  <Typography>
                    {reservation.isEquipmentReserved
                      ? dictionary.reservationsPage.yesLabel
                      : dictionary.reservationsPage.noLabel}
                  </Typography>
                </Box>

                <Box sx={{ width: '18%', textAlign: 'center' }}>
                  <Typography>
                    {getReservationStatus(reservation)}
                  </Typography>
                </Box>
              </Box>

              <ReservationButton
                backgroundColor={"#f0aa4f"}
                onClick={() => handleShowMoreInfo(reservation.reservationId)}
                minWidth={'6vw'}
              >
                {dictionary.reservationsPage.moreInfoLabel}
              </ReservationButton>
              <ReservationButton
                backgroundColor={"#F46C63"}
                onClick={() => handleOpenModal(reservation)}
                disabled={!canCancelOrMoveReservation(reservation)}
                minWidth={'6vw'}
              >
                {dictionary.reservationsPage.cancelReservationLabel}
              </ReservationButton>
            </Box>
          ))}
        </Box>

        <Modal
          open={!!selectedReservation}
          onClose={handleCloseModal}
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
              {dictionary.reservationsPage.confirmLabel}
            </Typography>

            <Typography
              sx={{
                color: 'black',
                fontSize: '1.5rem',
                whiteSpace: 'pre-line',
              }}
            >
              {selectedReservation
                ? `${selectedReservation.clientEmail}
${new Date(selectedReservation.startTime).toLocaleDateString('pl-PL')} ${new Date(selectedReservation.startTime).toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' })} - ${new Date(selectedReservation.endTime).toLocaleTimeString('pl-PL', { hour: '2-digit', minute: '2-digit' })}`
                : ''}
            </Typography>

            <Box sx={{ display: 'flex', gap: '3rem', marginTop: '1rem' }}>
              <GreenButton
                onClick={handleCloseModal}
                style={{ maxWidth: "10vw", backgroundColor: "#F46C63", minWidth: '7vw' }}
                hoverBackgroundColor={'#c3564f'}
              >
                {dictionary.reservationsPage.noLabel}
              </GreenButton>
              <GreenButton
                onClick={() => handleCancelReservation(selectedReservation.clientEmail, selectedReservation.reservationId)}
                style={{ maxWidth: "10vw", minWidth: '7vw' }}
              >
                {dictionary.reservationsPage.yesLabel}
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
            {dictionary.reservationsPage.previousLabel}
          </ChangePageButton>
          <ChangePageButton
            disabled={reservations.length < reservationsRequiredToEnablePagination}
            onClick={handleNextPage}
            backgroundColor={"#8edfb4"}
            minWidth={"10vw"}
          >
            {dictionary.reservationsPage.nextLabel}
          </ChangePageButton>
        </Box>
      </Box>

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
          <Typography sx={{ color: 'red', fontWeight: 'Bold', fontSize: '2rem', mb: 2 }}>
            {dictionary.reservationsPage.cancelErrorTitle || "Błąd podczas odwoływania"}
          </Typography>
          <Typography sx={{ color: 'black', fontSize: '1rem', mb: 3 }}>
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
              {dictionary.reservationsPage.okButtonLabel || "OK"}
            </GreenButton>
          </Box>
        </Box>
      </Backdrop>
    </>
  );
}

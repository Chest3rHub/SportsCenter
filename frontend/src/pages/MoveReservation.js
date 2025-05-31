import { Box, MenuItem, Typography, Avatar, Backdrop } from "@mui/material";
import React, { useState, useContext, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import Header from '../components/Header';
import GreenButton from '../components/buttons/GreenButton';
import GreenBackground from '../components/GreenBackground';
import OrangeBackground from '../components/OrangeBackground';
import { SportsContext } from '../context/SportsContext';
import CustomInput from "../components/CustomInput";
import CustomTimeInput from "../components/CustomTimeInput";
import getCourts from "../api/getCourts";
import getTrainers from "../api/getTrainers";
import getClubWorkingHours from '../api/getClubWorkingHours';
import getWorkingHoursForSingleDay from "../utils/getWorkingHoursForSingleDay";
import generateTime from "../utils/generateTime";
import getEndTimeOptions from "../utils/getEndTimeOptions";
import getAvailableCourts from "../api/getAvailableCourts";
import getAvailableTrainers from "../api/getAvailableTrainers";
import moveReservation from "../api/moveReservation";
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';

function MoveReservation() {
  const location = useLocation();
  const {
    id,
    courtId: originalCourtId,
    startTime: originalStartTime,
    endTime: originalEndTime,
    trainerId: reservationTrainerId,
    offsetFromLocation
  } = location.state || {};

  const { dictionary } = useContext(SportsContext);
  const navigate = useNavigate();

  const [formData, setFormData] = useState({
    date: '',
    newStartTime: '',
    newEndTime: ''
  });

  const [dateError, setDateError] = useState(false);
  const [newStartTimeError, setNewStartTimeError] = useState(false);
  const [newStartTimeTooSoonError, setNewStartTimeTooSoonError] = useState(false);
  const [newEndTimeBeforeStartError, setNewEndTimeBeforeStartError] = useState(false);
  const [newEndTimeDurationError, setNewEndTimeDurationError] = useState(false);

  const [courtNotAvailableError, setCourtNotAvailableError] = useState(false);
  const [trainerNotAvailableError, setTrainerNotAvailableError] = useState(false);

  // backdropy
  const [openSuccessBackdrop, setOpenSuccessBackdrop] = useState(false);
  const [openFailureBackdrop, setOpenFailureBackdrop] = useState(false);
  const [failedSignUpLabel, setFailedSignUpLabel] = useState('');

  // lista kortów, trenerów, godzin pracy
  const [courts, setCourts] = useState([]);
  const [trainers, setTrainers] = useState([]);
  const [availableTrainers, setAvailableTrainers] = useState([]);
  const [workingDaysAndHours, setWorkingDaysAndHours] = useState([]);
  const [courtsError, setCourtsError] = useState('');

  useEffect(() => {
    getCourts()
      .then(response => setCourts(response))
      .catch(error => console.error('Błąd podczas getCourts:', error));
  }, []);

  useEffect(() => {
    getTrainers()
      .then(response => {
        setTrainers(response);
        setAvailableTrainers(response);
      })
      .catch(error => console.error('Błąd podczas getTrainers:', error));
  }, []);

  useEffect(() => {
    getClubWorkingHours(0)
      .then(res => res.json())
      .then(data => setWorkingDaysAndHours(data))
      .catch(error => console.error('Błąd podczas getClubWorkingHours:', error));
  }, []);

  // walidacja daty 
  useEffect(() => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (formData.date) {
      const selectedDate = new Date(formData.date);
      selectedDate.setHours(0, 0, 0, 0);

      if (selectedDate < today) {
        setDateError(true);
      } else {
        setDateError(false);
      }

      if (selectedDate >= today) {
        getWorkingHoursForSingleDay(selectedDate)
          .then(weekOffset => {
            return getClubWorkingHours(weekOffset);
          })
          .then(res => res.json())
          .then(data => setWorkingDaysAndHours(data))
          .catch(error => console.error('Błąd podczas getClubWorkingHours po zmianie daty:', error));
      }
    } else {
      // jeśli pole pustej daty → ani dataError, ani żaden inny błąd nie ustawiamy
      setDateError(false);
    }

    // przy każdej zmianie daty wyzeruj pola czasowe i powiązane błędy oraz dostępność kortów/trenerów
    setFormData(prev => ({
      date: prev.date,
      newStartTime: '',
      newEndTime: ''
    }));
    setNewStartTimeError(false);
    setNewStartTimeTooSoonError(false);
    setNewEndTimeBeforeStartError(false);
    setNewEndTimeDurationError(false);
    setCourtsError('');
    setCourtNotAvailableError(false);
    setTrainerNotAvailableError(false);

  }, [formData.date]);

  // walidacja newStartTime (tylko jeśli użytkownik wybrał cokolwiek)
  useEffect(() => {
    setFormData(prev => ({
      date: prev.date,
      newStartTime: prev.newStartTime,
      newEndTime: ''
    }));
    setNewEndTimeBeforeStartError(false);
    setNewEndTimeDurationError(false);
    setCourtsError('');
    setCourtNotAvailableError(false);
    setTrainerNotAvailableError(false);

    if (formData.newStartTime) {
      const now = new Date();
      const todayStr = now.toISOString().slice(0, 10);
      const startDateTime = new Date(`${formData.date}T${formData.newStartTime}:00`);

      // jeżeli ten sam dzień co dzisiaj i godzina < teraz → błąd
      if (formData.date === todayStr && startDateTime < now) {
        setNewStartTimeError(true);
        setNewStartTimeTooSoonError(false);
      } else {
        setNewStartTimeError(false);

        // czy jest co najmniej 24h do wybranego startu
        const diffToStartHours = (startDateTime - now) / (1000 * 60 * 60);
        if (diffToStartHours < 24) {
          setNewStartTimeTooSoonError(true);
        } else {
          setNewStartTimeTooSoonError(false);
        }
      }
    } else {
      // jeśli nie wybrano godziny startu, nie pokazujemy żadnego błędu
      setNewStartTimeError(false);
      setNewStartTimeTooSoonError(false);
    }
  }, [formData.newStartTime, formData.date]);

  // walidacja newEndTime i pobranie dostępnych kortów/trenerów
  useEffect(() => {
    setCourtsError('');
    setCourtNotAvailableError(false);
    setTrainerNotAvailableError(false);

    if (formData.newEndTime) {
      const startDateTime = formData.date && formData.newStartTime
        ? new Date(`${formData.date}T${formData.newStartTime}:00`)
        : null;
      const endDateTime = new Date(`${formData.date}T${formData.newEndTime}:00`);

      if (startDateTime && endDateTime <= startDateTime) {
        setNewEndTimeBeforeStartError(true);
        setNewEndTimeDurationError(false);
      } else {
        setNewEndTimeBeforeStartError(false);

        if (startDateTime) {
          const diffH = (endDateTime - startDateTime) / (1000 * 60 * 60);
          if (diffH < 1 || diffH > 5) {
            setNewEndTimeDurationError(true);
          } else {
            setNewEndTimeDurationError(false);
          }
        }
      }

      // jeśli nie ma błędu z czasem zakończenia ani błędu z czasem startu, pobierz dostępność
      if (
        !newEndTimeBeforeStartError &&
        !newEndTimeDurationError &&
        formData.newStartTime &&
        !newStartTimeError &&
        !newStartTimeTooSoonError
      ) {
        const isoStart = `${formData.date}T${formData.newStartTime}`;
        const isoEnd = `${formData.date}T${formData.newEndTime}`;

        getAvailableCourts(isoStart, isoEnd)
          .then(response => {
            setCourts(response);
            if (!response || response.length === 0) {
              setCourtsError(dictionary.moveReservation.noCourtsError || 'Brak dostępnych kortów w podanych godzinach');
            } else {
              setCourtsError('');
            }
          })
          .catch(error => console.error('Błąd podczas getAvailableCourts:', error));

        getAvailableTrainers(isoStart, isoEnd)
          .then(response => setAvailableTrainers(response))
          .catch(error => console.error('Błąd podczas getAvailableTrainers:', error));
      } else {
        // jeśli wystąpił błąd w czasie, to nie filtrujemy trenerów
        setAvailableTrainers(trainers);
      }
    } else {
      // Jjeśli nie wybrano końca, to ani required-error, ani validation-error nie pokazujemy
      setNewEndTimeBeforeStartError(false);
      setNewEndTimeDurationError(false);
      setAvailableTrainers(trainers);
    }
  }, [
    formData.newEndTime,
    formData.newStartTime,
    formData.date,
    newEndTimeBeforeStartError,
    newEndTimeDurationError,
    newStartTimeError,
    newStartTimeTooSoonError,
    trainers
  ]);

  // godziny otwarcia 
  const selectedDayWorkingHours = workingDaysAndHours.find(day => day.date === formData.date);
  const openHour = selectedDayWorkingHours ? selectedDayWorkingHours.openHour.slice(0, 5) : "10:00";
  const closeHour = selectedDayWorkingHours ? selectedDayWorkingHours.closeHour.slice(0, 5) : "22:00";

  // czy formularz jest poprawny 
  function isFormValid() {
    const { date, newStartTime, newEndTime } = formData;
    if (!date || !newStartTime || !newEndTime) {
      return false;
    }

    if (
      dateError ||
      newStartTimeError || newStartTimeTooSoonError ||
      newEndTimeBeforeStartError || newEndTimeDurationError
    ) {
      return false;
    }

    // czy do początku jest co najmniej 24h
    const now = new Date();
    const startDT = new Date(`${date}T${newStartTime}:00`);
    const diffToStartHours = (startDT - now) / (1000 * 60 * 60);
    if (diffToStartHours < 24) {
      return false;
    }

    return true;
  }

  async function handleSubmit() {
    if (!isFormValid()) {
      return;
    }

    // czy oryginalny kort nadal dostępny
    const courtIds = courts.map(c => c.id);
    if (!courtIds.includes(originalCourtId)) {
      setCourtNotAvailableError(true);
      return;
    } else {
      setCourtNotAvailableError(false);
    }

    // czy oryginalny trener nadal dostępny
    const trainerIds = availableTrainers.map(t => t.id);
    if (!trainerIds.includes(reservationTrainerId)) {
      setTrainerNotAvailableError(true);
      return;
    } else {
      setTrainerNotAvailableError(false);
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
        const failText = determineFailTextByResponseCode(response.status);
        setFailedSignUpLabel(failText);
        setOpenFailureBackdrop(true);
      } else {
        setOpenSuccessBackdrop(true);
        setFormData({ date: '', newStartTime: '', newEndTime: '' });
      }
    } catch (error) {
      console.error('Błąd podczas przenoszenia rezerwacji:', error);
      setFailedSignUpLabel(dictionary.moveReservation.savedFailureLabel);
      setOpenFailureBackdrop(true);
    }
  }

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
        return dictionary.moveReservation.trainerNotAvailableError;
      case 420:
        return dictionary.moveReservation.alreadyHasActivityLabel;
      default:
        return dictionary.moveReservation.savedFailureLabel;
    }
  }

  const handleCloseSuccess = () => setOpenSuccessBackdrop(false);
  const handleCloseFailure = () => setOpenFailureBackdrop(false);

  const handleCancel = () => {
    navigate('/my-reservations', {
      state: { offsetFromLocation }
    });
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  return (
    <>
      <GreenBackground height={"55vh"} marginTop={"2vh"}>
        <Header>{dictionary.moveReservation.title}</Header>
        <OrangeBackground width="70%">
          <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh" }}>
            <CustomInput
              label={dictionary.addReservationYourselfPage.dateLabel}
              type="date"
              id="date"
              name="date"
              fullWidth
              value={formData.date}
              onChange={handleChange}
              error={dateError}
              helperText={dateError ? dictionary.moveReservation.dateError : ""}
              size="small"
              InputLabelProps={{ shrink: true }}
            />

            <Box sx={{ display: 'flex', flexDirection: 'row', columnGap: '3vw' }}>
              <CustomTimeInput
                select
                label={dictionary.addReservationYourselfPage.startTimeLabel}
                id="newStartTime"
                name="newStartTime"
                value={formData.newStartTime}
                onChange={handleChange}
                error={newStartTimeError || newStartTimeTooSoonError}
                helperText={
                  newStartTimeError
                    ? dictionary.moveReservation.newStartTimeError
                    : newStartTimeTooSoonError
                      ? dictionary.moveReservation.newStartTimeTooSoonError
                      : ""
                }
                size="small"
                fullWidth
                disabled={!formData.date || dateError}
                SelectProps={{ sx: { textAlign: 'left', borderRadius: '8px' } }}
              >
                <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                {generateTime(openHour, closeHour, 30).map(time => (
                  <MenuItem key={time} value={time}>{time}</MenuItem>
                ))}
              </CustomTimeInput>

              <CustomTimeInput
                select
                label={dictionary.addReservationYourselfPage.endTimeLabel}
                id="newEndTime"
                name="newEndTime"
                value={formData.newEndTime}
                onChange={handleChange}
                error={newEndTimeBeforeStartError || newEndTimeDurationError}
                helperText={
                  newEndTimeBeforeStartError
                    ? dictionary.moveReservation.newEndTimeError
                    : newEndTimeDurationError
                      ? dictionary.moveReservation.newTimeDurationError
                      : ""
                }
                size="small"
                fullWidth
                disabled={!formData.date || !formData.newStartTime || newStartTimeError || newStartTimeTooSoonError}
                SelectProps={{ sx: { textAlign: 'left', borderRadius: '8px' } }}
              >
                <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                {getEndTimeOptions({ startTime: formData.newStartTime }, closeHour).map(time => (
                  <MenuItem key={time} value={time}>{time}</MenuItem>
                ))}
              </CustomTimeInput>
            </Box>

            {courtsError && (
              <Typography color="error" sx={{ mb: 2, mt: 2 }}>
                {courtsError}
              </Typography>
            )}
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

            <Box sx={{ display: "flex", flexDirection: "row", justifyContent: 'center', columnGap: "4vw" }}>
              <GreenButton
                onClick={handleCancel}
                style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }}
                hoverBackgroundColor={'#c3564f'}
              >
                {dictionary.moveReservation.returnLabel}
              </GreenButton>
              <GreenButton
                onClick={handleSubmit}
                type="submit"
                style={{ maxWidth: "10vw" }}
                disabled={!isFormValid()}
              >
                {dictionary.moveReservation.saveLabel}
              </GreenButton>
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
                <Typography sx={{ color: 'green', fontWeight: 'Bold', fontSize: '3rem', marginTop: '2vh' }}>
                  {dictionary.moveReservation.successLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ color: 'black', fontSize: '1.5rem' }}>
                  {dictionary.moveReservation.savedSuccessLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ color: 'black', fontSize: '1.5rem' }}>
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
                <Typography sx={{ color: 'red', fontWeight: 'Bold', fontSize: '3rem', marginTop: '2vh' }}>
                  {dictionary.moveReservation.failureLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ color: 'black', fontSize: '1.5rem' }}>
                  {failedSignUpLabel}
                </Typography>
              </Box>
              <Box>
                <Typography sx={{ color: 'black', fontSize: '1.5rem' }}>
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
  );
}

export default MoveReservation;

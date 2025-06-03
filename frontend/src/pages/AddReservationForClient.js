import { Box, MenuItem, Checkbox, FormControlLabel, Autocomplete, Typography, Avatar, Backdrop } from "@mui/material";
import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
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
import { useDebounce } from '../hooks/useDebounce';
import searchClientsTopTen from '../api/searchClientsTopTen';
import addRecurringReservationForClient from '../api/addRecurringReservationForClient';
import addReservationForClient from '../api/addReservationForClient';
import ReservationButton from '../components/buttons/ReservationButton';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import { Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';

export default function AddReservationForClient() {
  const { dictionary } = useContext(SportsContext);
  const navigate = useNavigate();

  const [courts, setCourts] = useState([]);
  const [availableCourts, setAvailableCourts] = useState([]);
  const [trainers, setTrainers] = useState([]);
  const [availableTrainers, setAvailableTrainers] = useState([]); 
  const [clients, setClients] = useState([]);
  const [workingDaysAndHours, setWorkingDaysAndHours] = useState([]);

  const [isLoading, setIsLoading] = useState(false);

  const [clientRequiredError, setClientRequiredError] = useState(false);
  const [dateRequiredError, setDateRequiredError] = useState(false);
  const [dateError, setDateError] = useState(false);
  const [startTimeRequiredError, setStartTimeRequiredError] = useState(false);
  const [startTimeError, setStartTimeError] = useState(false);
  const [endTimeRequiredError, setEndTimeRequiredError] = useState(false);
  const [endTimeBeforeStartError, setEndTimeBeforeStartError] = useState(false);
  const [endTimeDurationError, setEndTimeDurationError] = useState(false);
  const [courtsError, setCourtsError] = useState(false);
  const [participantsCountError, setParticipantsCountError] = useState(false);
  const [recurrenceEndDateError, setRecurrenceEndDateError] = useState(false);

  const [reservationProposals, setReservationProposals] = useState([]);
  const [failedReservations, setFailedReservations] = useState([])
  const [selectedCourtIds, setSelectedCourtIds] = useState({});
  const [selectedTrainerIds, setSelectedTrainerIds] = useState({});
  const [courtIdMissingError, setCourtIdMissingError] = useState({});
  const [trainerIdMissingError, setTrainerIdMissingError] = useState({});
  const [proposalErrors, setProposalErrors] = useState({});
  const [openProposalDialog, setOpenProposalDialog] = useState(false);

  const [openSuccessBackdrop, setOpenSuccessBackdrop] = useState(false);
  const [openFailureBackdrop, setOpenFailureBackdrop] = useState(false);
  const [failedSignUpLabel, setFailedSignUpLabel] = useState("");

  const [searchClientText, setSearchClientText] = useState("");
  const debouncedSearchText = useDebounce(searchClientText, 300);
  const [selectedClient, setSelectedClient] = useState(null);

  const [formData, setFormData] = useState({
    clientId: "",
    date: "",
    startTime: "",
    endTime: "",
    courtId: "",
    trainerId: "",
    participantsCount: "",
    isEquipmentReserved: false,
    recurrence: "",
    recurrenceEndDate: ""
  });

  function removeCourtErrorsAfterChange() {
    setCourtsError(false);
  }

  // top 10 klientow
  useEffect(() => {
    async function fetchClients() {
      setIsLoading(true);
      try {
        const clientsResponse = await searchClientsTopTen(debouncedSearchText);
        const clientsData = await clientsResponse.json();
        setClients(clientsData);
      } catch (error) {
        console.error("Błąd podczas pobierania klientów:", error);
      } finally {
        setIsLoading(false);
      }
    }
    fetchClients();
  }, [debouncedSearchText]);

  // wszystkie korty
  useEffect(() => {
    getCourts()
      .then(response => setCourts(response))
      .catch(error => console.error("Błąd getCourts:", error));
  }, []);

  // wszyscy trenerzy
  useEffect(() => {
    getTrainers()
      .then(response => {
        setTrainers(response);
        setAvailableTrainers(response); 
      })
      .catch(error => console.error("Błąd getTrainers:", error));
  }, []);

  // godziny otwarcia klubu
  useEffect(() => {
    getClubWorkingHours(-1)
      .then(res => res.json())
      .then(data => setWorkingDaysAndHours(data))
      .catch(error => console.error("Błąd getClubWorkingHours:", error));
  }, []);

 // obsluga bledow
  useEffect(() => {
    if (formData.clientId) {
      setClientRequiredError(false);
    }
  }, [formData.clientId]);

  useEffect(() => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (formData.date) {
      const selectedDate = new Date(formData.date);
      selectedDate.setHours(0, 0, 0, 0);
      setDateError(selectedDate < today);
    } else {
      setDateError(false);
    }

    setFormData(prev => ({
      clientId: prev.clientId,
      date: prev.date,
      startTime: "",
      endTime: "",
      courtId: "",
      trainerId: "",
      participantsCount: "",
      isEquipmentReserved: prev.isEquipmentReserved,
      recurrence: "",
      recurrenceEndDate: ""
    }));

    setDateRequiredError(false);
    removeCourtErrorsAfterChange();
    setParticipantsCountError(false);
    setRecurrenceEndDateError(false);
    setAvailableCourts([]);

  }, [formData.date]);

  useEffect(() => {
    if (formData.startTime && formData.date) {
      const now = new Date();
      const todayStr = now.toISOString().slice(0, 10);
      const selectedStart = new Date(`${formData.date}T${formData.startTime}:00`);
      setStartTimeError(formData.date === todayStr && selectedStart < now);
    } else {
      setStartTimeError(false);
    }

    setFormData(prev => ({
      clientId: prev.clientId,
      date: prev.date,
      startTime: prev.startTime,
      endTime: "",
      courtId: "",
      trainerId: "",
      participantsCount: "",
      isEquipmentReserved: prev.isEquipmentReserved,
      recurrence: "",
      recurrenceEndDate: ""
    }));

    setStartTimeRequiredError(false);
    setEndTimeRequiredError(false);
    setEndTimeBeforeStartError(false);
    setEndTimeDurationError(false);
    removeCourtErrorsAfterChange();
    setParticipantsCountError(false);
    setRecurrenceEndDateError(false);

  }, [formData.startTime, formData.date]);

  useEffect(() => {
    setEndTimeRequiredError(false);
    setEndTimeBeforeStartError(false);
    setEndTimeDurationError(false);
    removeCourtErrorsAfterChange();

    if (formData.endTime && formData.startTime && formData.date) {
      const startDT = new Date(`${formData.date}T${formData.startTime}:00`);
      const endDT = new Date(`${formData.date}T${formData.endTime}:00`);

      if (endDT <= startDT) {
        setEndTimeBeforeStartError(true);
      } else {
        setEndTimeBeforeStartError(false);
      }

      const diffH = (endDT - startDT) / (1000 * 60 * 60);
      if (diffH < 1 || diffH > 5) {
        setEndTimeDurationError(true);
      } else {
        setEndTimeDurationError(false);
      }

      if (
        !endTimeBeforeStartError &&
        !endTimeDurationError &&
        formData.startTime &&
        !startTimeError &&
        !startTimeRequiredError
      ) {
        const isoStart = `${formData.date}T${formData.startTime}`;
        const isoEnd = `${formData.date}T${formData.endTime}`;

        getAvailableCourts(isoStart, isoEnd)
          .then(response => {
            setAvailableCourts(response);
            if (!response || response.length === 0) {
              setCourtsError(true);
            } else {
              setCourtsError(false);
            }
          })
          .catch(error => console.error("Błąd getAvailableCourts:", error));

        getAvailableTrainers(isoStart, isoEnd)
          .then(response => {
            setAvailableTrainers(response);
          })
          .catch(error => console.error("Błąd getAvailableTrainers:", error));
      }
    } else {
      setAvailableCourts([]);
    }
  },
  [
    formData.endTime,
    formData.startTime,
    formData.date,
    startTimeError,
    startTimeRequiredError,
    endTimeBeforeStartError,
    endTimeDurationError
  ]);

  useEffect(() => {
    if (formData.courtId) {
      setCourtsError(false);
    }

    setFormData(prev => ({
      clientId: prev.clientId,
      date: prev.date,
      startTime: prev.startTime,
      endTime: prev.endTime,
      courtId: prev.courtId,
      trainerId: "",
      participantsCount: "",
      isEquipmentReserved: prev.isEquipmentReserved,
      recurrence: "",
      recurrenceEndDate: ""
    }));

    setParticipantsCountError(false);
    setRecurrenceEndDateError(false);
  }, [formData.courtId ]);

  useEffect(() => {
    setFormData(prev => ({
      clientId: prev.clientId,
      date: prev.date,
      startTime: prev.startTime,
      endTime: prev.endTime,
      courtId: prev.courtId,
      trainerId: prev.trainerId,
      participantsCount: "",
      isEquipmentReserved: prev.isEquipmentReserved,
      recurrence: "",
      recurrenceEndDate: ""
    }));
  }, [formData.trainerId]);

  useEffect(() => {
    if (formData.participantsCount === "") {
      setParticipantsCountError(false);
      return;
    }

    const cnt = parseInt(formData.participantsCount, 10);
    if (!cnt || cnt < 1 || cnt > 8) {
      setParticipantsCountError(true);
    } else {
      setParticipantsCountError(false);
    }
  }, [formData.participantsCount]);

  useEffect(() => {
    if (formData.recurrence && formData.recurrenceEndDate && formData.endTime && formData.date) {
      const recDate = new Date(`${formData.recurrenceEndDate}T00:00:00`);
      const endDate = new Date(`${formData.date}T${formData.endTime}:00`);
      recDate.setHours(0, 0, 0, 0);
      endDate.setHours(0, 0, 0, 0);

      const daysDiff = (recDate - endDate) / (1000 * 60 * 60 * 24);
      if (daysDiff < 1) {
        setRecurrenceEndDateError(true);
      } else {
        setRecurrenceEndDateError(false);
      }
    }
  }, [formData.recurrenceEndDate, formData.endTime, formData.date, formData.recurrence]);

  useEffect(() => {
    if (formData.date) {
      const startDate = new Date(formData.date);
      getWorkingHoursForSingleDay(startDate)
        .then(weekOffset => getClubWorkingHours(weekOffset - 1))
        .then(res => res.json())
        .then(data => setWorkingDaysAndHours(data))
        .catch(error => console.error("Błąd getClubWorkingHours po zmianie daty:", error));
    }
  }, [formData.date]);

  const selectedDayWorkingHours = workingDaysAndHours.find(day => day.date === formData.date);
  const openHour = selectedDayWorkingHours ? selectedDayWorkingHours.openHour.slice(0, 5) : "10:00";
  const closeHour = selectedDayWorkingHours ? selectedDayWorkingHours.closeHour.slice(0, 5) : "22:00";

  function isFormValid() {
    let valid = true;
    const { clientId, date, startTime, endTime, courtId, participantsCount, recurrenceEndDate } = formData;

    const now = new Date();
    const todayStr = now.toISOString().slice(0, 10);
    const startDT = date && startTime ? new Date(`${date}T${startTime}:00`) : null;
    const endDT = date && endTime ? new Date(`${date}T${endTime}:00`) : null;

    if (!clientId) {
      setClientRequiredError(true);
      valid = false;
    } else {
      setClientRequiredError(false);
    }

    if (!date) {
      setDateRequiredError(true);
      valid = false;
    } else {
      setDateRequiredError(false);
      if (date < todayStr) {
        setDateError(true);
        valid = false;
      } else {
        setDateError(false);
      }
    }

    if (!startTime) {
      setStartTimeRequiredError(true);
      valid = false;
    } else {
      setStartTimeRequiredError(false);
      if (date === todayStr && startDT < now) {
        setStartTimeError(true);
        valid = false;
      } else {
        setStartTimeError(false);
      }
    }

    if (!endTime) {
      setEndTimeRequiredError(true);
      valid = false;
    } else {
      setEndTimeRequiredError(false);
      if (startDT && endDT <= startDT) {
        setEndTimeBeforeStartError(true);
        valid = false;
      } else {
        setEndTimeBeforeStartError(false);
      }
      if (startDT && endDT) {
        const diffH = (endDT - startDT) / (1000 * 60 * 60);
        if (diffH < 1 || diffH > 5) {
          setEndTimeDurationError(true);
          valid = false;
        } else {
          setEndTimeDurationError(false);
        }
      }
    }

    if (!courtId) {
      setCourtsError(true);
      valid = false;
    } else {
      setCourtsError(false);
    }

    const cnt = parseInt(participantsCount, 10);
    if (!cnt || cnt < 1 || cnt > 8) {
      setParticipantsCountError(true);
      valid = false;
    } else {
      setParticipantsCountError(false);
    }

    if (formData.recurrence && recurrenceEndDate && endDT) {
      const recDate = new Date(`${recurrenceEndDate}T00:00:00`);
      recDate.setHours(0, 0, 0, 0);
      const endDateOnly = new Date(endDT);
      endDateOnly.setHours(0, 0, 0, 0);
      const daysDiff = (recDate - endDateOnly) / (1000 * 60 * 60 * 24);
      if (daysDiff < 1) {
        setRecurrenceEndDateError(true);
        valid = false;
      } else {
        setRecurrenceEndDateError(false);
      }
    }

    return valid;
  }

  function determineFailedReservationReasonByResponseCode(responseCode, startTime, endTime) {
    switch(responseCode) {
      case 1:
        const from = startTime.slice(0, 5);
        const to   = endTime.slice(0, 5);
        return dictionary.addReservationForClientPage.workingHoursError1 + from + dictionary.addReservationForClientPage.workingHoursError1 + to;
      case 2:
        const from2 = startTime.slice(0, 5);
        const to2   = endTime.slice(0, 5);
        return dictionary.addReservationForClientPage.workingHoursError1 + from2 + dictionary.addReservationForClientPage.workingHoursError1 + to2;
      case 3:
        return dictionary.addReservationForClientPage.alreadyHasActivityLabel;
      default: 
        return dictionary.addReservationForClientPage.savedFailureLabel;
    }
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

  const handleCloseSuccess = () => setOpenSuccessBackdrop(false);
  const handleCloseFailure = () => setOpenFailureBackdrop(false);
  const handleOpenSuccess = () => setOpenSuccessBackdrop(true);
  const handleOpenFailure = () => setOpenFailureBackdrop(true);

  async function handleSubmit() {
    if (!isFormValid()) {
      return;
    }

    const toLocalIso = dt => {
      const t = new Date(dt.getTime() - dt.getTimezoneOffset() * 60000);
      return t.toISOString().slice(0, 19);
    };

    const isRecurring = Boolean(formData.recurrence.trim()) && Boolean(formData.recurrenceEndDate.trim());

    const payload = {
      ClientId: Number(formData.clientId),
      CourtId: Number(formData.courtId),
      StartTime: toLocalIso(new Date(`${formData.date}T${formData.startTime}:00`)),
      EndTime: toLocalIso(new Date(`${formData.date}T${formData.endTime}:00`)),
      CreationDate: toLocalIso(new Date()),
      TrainerId: formData.trainerId ? Number(formData.trainerId) : 0,
      ParticipantsCount: formData.participantsCount.toString(),
      IsEquipmentReserved: formData.isEquipmentReserved ? "true" : ""
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
        const failText = determineFailTextByResponseCode(response.status);
        setFailedSignUpLabel(failText);
        handleOpenFailure();
        return;
      }

      const resultData = await response.json();

      setFailedReservations(resultData.failedReservations || []);

      const proposals = resultData.reservationProposals || [];
      setReservationProposals(
        proposals.map((p, i) => ({
          ...p,
          _uid: `${p.date}-${i}`
        }))
      );

      if ((resultData.failedReservations && resultData.failedReservations.length > 0) || (proposals.length > 0)) {

        setOpenProposalDialog(true);

      } else {
        handleOpenSuccess();
        setFormData({
          clientId: "",
          date: "",
          startTime: "",
          endTime: "",
          courtId: "",
          trainerId: "",
          participantsCount: "",
          isEquipmentReserved: false,
          recurrence: "",
          recurrenceEndDate: ""
        });
      }
    } catch (error) {
      console.error("Błąd podczas rezerwacji:", error);
    }
  }

  const handleRejectProposal = uid => {
    setReservationProposals(prev => prev.filter(p => p._uid !== uid));
    setSelectedCourtIds(prev => {
      const o = { ...prev };
      delete o[uid];
      return o;
    });
    setSelectedTrainerIds(prev => {
      const o = { ...prev };
      delete o[uid];
      return o;
    });
    setCourtIdMissingError(prev => {
      const o = { ...prev };
      delete o[uid];
      return o;
    });
    setTrainerIdMissingError(prev => {
      const o = { ...prev };
      delete o[uid];
      return o;
    });
    if (reservationProposals.length === 1) {
      setOpenProposalDialog(false);
    }
  };

  const handleAcceptProposal = async uid => {
    const p = reservationProposals.find(x => x._uid === uid);
    const chosenCourt = selectedCourtIds[uid];
    const chosenTrainer = selectedTrainerIds[uid];

    if (!chosenCourt) {
      setCourtIdMissingError(prev => ({ ...prev, [uid]: true }));
      return;
    } else {
      setCourtIdMissingError(prev => ({ ...prev, [uid]: false }));
    }

    const dateOnly = p.date.split("T")[0];
    const start = new Date(`${dateOnly}T${formData.startTime}:00`);
    const end = new Date(`${dateOnly}T${formData.endTime}:00`);

    const toLocalIso = dt => {
      const t = new Date(dt.getTime() - dt.getTimezoneOffset() * 60000);
      return t.toISOString().slice(0, 19);
    };

    const reservationData = {
      ClientId: Number(formData.clientId),
      CourtId: Number(chosenCourt),
      StartTime: toLocalIso(start),
      EndTime: toLocalIso(end),
      CreationDate: toLocalIso(new Date()),
      TrainerId: chosenTrainer ? Number(chosenTrainer) : 0,
      ParticipantsCount: formData.participantsCount,
      IsEquipmentReserved: formData.isEquipmentReserved ? "true" : ""
    };

    try {
      const response = await addReservationForClient(reservationData);
      if (response.ok) {
        handleRejectProposal(uid);
      } else {
        const failText = determineFailTextByResponseCode(response.status);
        setProposalErrors(prev => ({ ...prev, [uid]: failText }));
      }
    } catch (error) {
      console.error("Błąd:", error);
    }
  };

  function handleCancel() {
    navigate("/reservations");
  }

  function handleChange(e) {
    const { name, value, type, checked } = e.target;
    const newValue = type === "checkbox" ? checked : value;
    setFormData(prev => ({
      ...prev,
      [name]: newValue
    }));
  }

  const setCourtFor = (uid, courtId) => {
    setSelectedCourtIds(prev => ({ ...prev, [uid]: courtId }));
    setCourtIdMissingError(prev => ({ ...prev, [uid]: false }));
  };
  const setTrainerFor = (uid, trainerId) => {
    setSelectedTrainerIds(prev => ({ ...prev, [uid]: trainerId }));
    setTrainerIdMissingError(prev => ({ ...prev, [uid]: false }));
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
              getOptionLabel={option => option.email}
              value={selectedClient}
              onChange={(event, newValue) => {
                handleChange({ target: { name: "clientId", value: newValue?.clientId || "" } });
                setSelectedClient(newValue);
              }}
              onInputChange={(event, newInputValue) => {
                setSearchClientText(newInputValue);
                if (!newInputValue) {
                  setClientRequiredError(false);
                }
              }}
              isOptionEqualToValue={(option, value) => option.clientId === value?.clientId}
              renderInput={params => (
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
              name="date"
              fullWidth
              value={formData.date}
              onChange={handleChange}
              error={dateRequiredError || dateError}
              helperText={
                dateRequiredError
                  ? dictionary.addReservationForClientPage.dateRequiredError
                  : dateError
                    ? dictionary.addReservationForClientPage.dateError
                    : ""
              }
              required
              size="small"
              InputLabelProps={{ shrink: true }}
            />

            <Box sx={{ display: "flex", flexDirection: "row", columnGap: "3vw" }}>
              <CustomTimeInput
                select
                label={dictionary.addReservationYourselfPage.startTimeLabel}
                name="startTime"
                value={formData.startTime}
                onChange={handleChange}
                required
                error={startTimeRequiredError || startTimeError}
                helperText={
                  startTimeRequiredError
                    ? dictionary.addReservationForClientPage.startTimeRequiredError
                    : startTimeError
                      ? dictionary.addReservationForClientPage.startTimeError
                      : ""
                }
                size="small"
                fullWidth
                disabled={!formData.date}
                SelectProps={{ sx: { textAlign: "left", borderRadius: "8px" } }}
              >
                <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                {generateTime(openHour, closeHour, 30).map(time => (
                  <MenuItem key={time} value={time}>
                    {time}
                  </MenuItem>
                ))}
              </CustomTimeInput>

              <CustomTimeInput
                select
                label={dictionary.addReservationYourselfPage.endTimeLabel}
                name="endTime"
                value={formData.endTime}
                onChange={handleChange}
                required
                error={endTimeRequiredError || endTimeBeforeStartError || endTimeDurationError}
                helperText={
                  endTimeRequiredError
                    ? dictionary.addReservationForClientPage.endTimeRequiredError
                    : endTimeBeforeStartError
                      ? dictionary.addReservationForClientPage.endTimeBeforeStartError
                      : endTimeDurationError
                        ? dictionary.addReservationForClientPage.endTimeDurationError
                        : ""
                }
                size="small"
                fullWidth
                disabled={!formData.date || !formData.startTime}
                SelectProps={{ sx: { textAlign: "left", borderRadius: "8px" } }}
              >
                <MenuItem value="">{dictionary.addReservationYourselfPage.chooseTimeLabel}</MenuItem>
                {getEndTimeOptions({ startTime: formData.startTime }, closeHour).map(time => (
                  <MenuItem key={time} value={time}>
                    {time}
                  </MenuItem>
                ))}
              </CustomTimeInput>
            </Box>

            <CustomInput
              select
              label={dictionary.addReservationForClientPage.courtNameLabel}
              name="courtId"
              value={formData.courtId}
              fullWidth
              onChange={handleChange}
              required
              size="small"
              SelectProps={{ sx: { textAlign: "left", borderRadius: "8px" } }}
              disabled={!formData.date || !formData.startTime || !formData.endTime}
              error={courtsError}
              helperText={courtsError ? dictionary.addReservationForClientPage.courtsError : ""}
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
              name="trainerId"
              value={formData.trainerId}
              fullWidth
              onChange={handleChange}
              size="small"
              SelectProps={{ sx: { textAlign: "left" } }}
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
                  name="isEquipmentReserved"
                  checked={formData.isEquipmentReserved}
                  onChange={handleChange}
                  sx={{
                    color: "#8edfb4",
                    "&.Mui-checked": { color: "#8edfb4" }
                  }}
                />
              }
              label={dictionary.addReservationForClientPage.isEquipmentReservedLabel}
            />

            <CustomInput
              select
              label={dictionary.addReservationForClientPage.recurrenceLabel}
              name="recurrence"
              value={formData.recurrence}
              fullWidth
              onChange={handleChange}
              size="small"
              SelectProps={{ sx: { textAlign: "left", borderRadius: "8px" } }}
            >
              <MenuItem value="">
                {dictionary.addReservationForClientPage.recurrenceOptions?.None || "Brak"}
              </MenuItem>
              <MenuItem value="Daily">{dictionary.addReservationForClientPage.recurrenceOptions.Daily}</MenuItem>
              <MenuItem value="Weekly">{dictionary.addReservationForClientPage.recurrenceOptions.Weekly}</MenuItem>
              <MenuItem value="BiWeekly">{dictionary.addReservationForClientPage.recurrenceOptions.BiWeekly}</MenuItem>
              <MenuItem value="Monthly">{dictionary.addReservationForClientPage.recurrenceOptions.Monthly}</MenuItem>
            </CustomInput>

            <CustomInput
              label={dictionary.addReservationForClientPage.recurrenceEndDateLabel}
              type="date"
              name="recurrenceEndDate"
              fullWidth
              value={formData.recurrenceEndDate}
              onChange={handleChange}
              error={recurrenceEndDateError}
              helperText={recurrenceEndDateError ? dictionary.addReservationForClientPage.recurrenceEndDateError : ""}
              size="small"
              InputLabelProps={{ shrink: true }}
            />

            <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "center", columnGap: "2vw" }}>
              <GreenButton
                onClick={handleCancel}
                style={{ backgroundColor: "#F46C63" }}
                hoverBackgroundColor={"#c3564f"}
              >
                {dictionary.addReservationForClientPage.returnLabel}
              </GreenButton>
              <GreenButton type="submit" onClick={handleSubmit}>
                {dictionary.addReservationForClientPage.confirmLabel}
              </GreenButton>
            </Box>
          </Box>

          <Dialog open={openProposalDialog} onClose={() => setOpenProposalDialog(false)} fullWidth maxWidth="md">
            <DialogTitle sx={{ textAlign: "center", color: "#F46C63", fontSize: "2rem" }}>
              {dictionary.addReservationForClientPage.proposalHeader}
            </DialogTitle>
            <DialogContent dividers>
              <Typography sx={{ color: "#ccc", fontSize: "1rem", mb: 2, textAlign: "center" }}>
                {dictionary.addReservationForClientPage.closeInfo}
              </Typography>

              {failedReservations.length > 0 && (
                <Box sx={{ mb: 3, p: 2, backgroundColor: "#ffe7e7", borderRadius: 1 }}>
                  <Typography sx={{ fontWeight: "bold", color: "red", mb: 1 }}>
                    {dictionary.addReservationForClientPage.failedReservationsHeader || ""}
                  </Typography>
                  {failedReservations.map((f, idx) => {
                    const displayDate = new Date(f.date).toLocaleString("pl-PL", {
                      year: "numeric",
                      month: "2-digit",
                      day: "2-digit",
                      hour: "2-digit",
                      minute: "2-digit"
                    });
                    
                  const reasonText = determineFailedReservationReasonByResponseCode(
                    f.errorCode,
                    f.startTime || "", 
                    f.endTime   || ""
                  );

                  return (
                  <Box key={idx} sx={{ mb: 1 }}>
                    <Typography>
                      {displayDate}
                      {": "}
                      <Typography component="span" color="error">
                        {reasonText}
                      </Typography>
                    </Typography>
                  </Box>
                  );
                  })}
                </Box>
              )}

              {reservationProposals.length > 0 ? (
                reservationProposals.map(p => {
                  const uid = p._uid;
                  const dateDisplay = new Date(p.date).toLocaleString("pl-PL", {
                    year: "numeric",
                    month: "2-digit",
                    day: "2-digit",
                    hour: "2-digit",
                    minute: "2-digit"
                  });
                  return (
                    <Box
                      key={uid}
                      sx={{
                        mb: 3,
                        p: 3,
                        backgroundColor: "#ebf9ea",
                        borderRadius: 2,
                        display: "flex",
                        flexDirection: "column",
                        rowGap: "3vh"
                      }}
                    >
                      <Typography variant="h6" sx={{ fontWeight: "bold", color: "#333" }}>
                        {dateDisplay}
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
                          value={selectedCourtIds[uid] || ""}
                          onChange={e => setCourtFor(uid, e.target.value)}
                          error={Boolean(courtIdMissingError[uid])}
                          size="small"
                          sx={{ mb: 2 }}
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
                          value={selectedTrainerIds[uid] || ""}
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

                      <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "center", columnGap: "1vw" }}>
                        <ReservationButton backgroundColor={"#F46C63"} onClick={() => handleRejectProposal(uid)}>
                          {dictionary.addReservationForClientPage.ignoreReservationProposal}
                        </ReservationButton>
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

          <Backdrop
            open={openSuccessBackdrop}
            onClick={handleCloseSuccess}
            sx={{ color: "#fff", zIndex: theme => theme.zIndex.drawer + 1 }}
          >
            <Box
              sx={{
                backgroundColor: "white",
                margin: "auto",
                minWidth: "30vw",
                minHeight: "30vh",
                borderRadius: "20px",
                boxShadow: "0 10px 20px rgba(0, 0, 0, 0.2)"
              }}
            >
              <Typography sx={{ color: "green", fontWeight: "Bold", fontSize: "3rem", marginTop: "2vh" }}>
                {dictionary.addReservationForClientPage.successLabel}
              </Typography>
              <Typography sx={{ color: "black", fontSize: "1.5rem" }}>
                {dictionary.addReservationForClientPage.reservationCreatedLabel}
              </Typography>
              <Typography sx={{ color: "black", fontSize: "1.5rem" }}>
                {dictionary.activityDetailsPage.clickAnywhereLabel}
              </Typography>
              <Box sx={{ textAlign: "center", display: "flex", justifyContent: "center" }}>
                <Avatar sx={{ width: "7rem", height: "7rem" }}>
                  <SentimentSatisfiedIcon
                    sx={{ fontSize: "7rem", color: "green", stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }}
                  />
                </Avatar>
              </Box>
            </Box>
          </Backdrop>

          <Backdrop
            open={openFailureBackdrop}
            onClick={handleCloseFailure}
            sx={{ color: "#fff", zIndex: theme => theme.zIndex.drawer + 1 }}
          >
            <Box
              sx={{
                backgroundColor: "white",
                margin: "auto",
                minWidth: "40vw",
                minHeight: "30vh",
                borderRadius: "20px",
                boxShadow: "0 10px 20px rgba(0, 0, 0, 0.2)"
              }}
            >
              <Typography sx={{ color: "red", fontWeight: "Bold", fontSize: "3rem", marginTop: "2vh" }}>
                {dictionary.addReservationForClientPage.failureLabel}
              </Typography>
              <Typography sx={{ color: "black", fontSize: "1.5rem" }}>{failedSignUpLabel}</Typography>
              <Typography sx={{ color: "black", fontSize: "1.5rem" }}>
                {dictionary.activityDetailsPage.clickAnywhereFailureLabel}
              </Typography>
              <Box sx={{ textAlign: "center", display: "flex", justifyContent: "center" }}>
                <Avatar sx={{ width: "7rem", height: "7rem" }}>
                  <SentimentDissatisfiedIcon
                    sx={{ fontSize: "7rem", color: "red", stroke: "white", strokeWidth: 1.1, backgroundColor: "white" }}
                  />
                </Avatar>
              </Box>
            </Box>
          </Backdrop>
        </OrangeBackground>
      </GreenBackground>
    </>
  );
}

import { useLocation, useNavigate } from "react-router-dom";
import { Typography, Box, List, ListItem, ListItemText, Avatar, FormControlLabel, Checkbox } from "@mui/material";
import Header from "../components/Header";
import GreenButton from "../components/buttons/GreenButton";
import CustomInput from "../components/CustomInput";
import { useContext, useState } from "react";
import { SportsContext } from "../context/SportsContext";
import signUpForActivity from "../api/signUpForActivity";
import Backdrop from '@mui/material/Backdrop';
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied';
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied';
import cancelReservation from "../api/cancelReservation";
import cancelSignUpForActivity from "../api/cancelSignUpForActivity";
export default function MyActivityDetails() {
    const { dictionary, toggleLanguage, role } = useContext(SportsContext);

    const navigate = useNavigate();
    const location = useLocation();
    const { activityDetails } = location.state || {};
    function handleCancel() {
        navigate(-1);
    }

    const [isEquipmentIncluded, setIsEquipmentIncluded] = useState(false);
    const [failedSignUpLabel, setFailedSignUpLabel] = useState('')

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

    const {
        description,
        dateOfActivity,
        startTime,
        endTime,
        courtName,
        sportActivityName,
        levelName,
        trainerName,
        participants,
        cost,
        isEquipmentReserved,
        isCanceled,
        instanceOfActivityId,
        reservationId,
        clientName,
        clientSurname,
        isActivityCanceled
    } = activityDetails;

    const formattedDate = new Date(dateOfActivity + 'Z').toISOString().split("T")[0];
    const formattedStartTime = startTime.slice(0, 5);
    const formattedEndTime = endTime.slice(0, 5);


    function determineFailTextByResponseCode(responseCode) {
        switch (responseCode) {
          case 409:
            return dictionary.activityDetailsPage.alreadySignedUpLabel;
          case 418:
            return dictionary.activityDetailsPage.tooLongLabel;
          case 420:
            return dictionary.activityDetailsPage.canceledLabel;
          default:
            return dictionary.activityDetailsPage.savedFailureLabel;
        }
      }
      
    async function cancelActivityClient() {

        try {
            let response;
            if(reservationId){
                 response = await cancelReservation(reservationId,);
            } else if(instanceOfActivityId){
                 response = await cancelSignUpForActivity(instanceOfActivityId, formattedDate,);
            }
            if (!response.ok) {
                const errorData = await response.json();
                let failText = determineFailTextByResponseCode(response.status);
                setFailedSignUpLabel(failText);
                handleOpenFailure();
            } else {
                handleOpenSuccess();
            }
        } catch (error) {
            handleOpenFailure();
        }
    }

    function canCancel(event) {
        if (!event) return false;

        if (event.isReservationCanceled || event.isMoneyRefunded) return false;
    
        const now = new Date();
        const activityStart = new Date(`${dateOfActivity}T${startTime}`);
        const diffInHours = (activityStart - now) / (1000 * 60 * 60);
        return diffInHours >= 2;
    }   

    if (!activityDetails) {
        return <Typography variant="h6">{dictionary.activityDetailsPage.noDataAvailable}</Typography>;
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
                    alignItems: 'center',
                }}
            >
                <Box sx={{ maxWidth: '40vw', minWidth: '30vw' }}>
                    <Header backgroundColor={isActivityCanceled ? '#F46C63' : undefined}>{isActivityCanceled ? dictionary.activityDetailsPage.shortCanceledLabel : (sportActivityName ? sportActivityName : dictionary.activityDetailsPage.reservationLabel)}</Header>
                </Box>
                <Box sx={{
                   
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    backgroundColor: 'white',
                    padding: '1.35rem',
                    minWidth: '30vw',
                    display: 'flex',
                    flexDirection: 'column',
                    rowGap: '2vh',
                    paddingTop:'5vh',
                }}>

                    <CustomInput
                        label={dictionary.activityDetailsPage.dateLabel}
                        type="date"
                        id="date"
                        name="date"
                        fullWidth
                        value={formattedDate}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },

                        }}
                    />
                    <CustomInput
                        label={dictionary.activityDetailsPage.startTimeLabel}
                        type="startTime"
                        id="startTime"
                        name="startTime"
                        fullWidth
                        value={formattedStartTime}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },

                        }}
                    />
                    <CustomInput
                        label={dictionary.activityDetailsPage.endTimeLabel}
                        type="endTime"
                        id="endTime"
                        name="endTime"
                        fullWidth
                        value={formattedEndTime}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },

                        }}
                    />

                    <CustomInput
                        label={dictionary.activityDetailsPage.groupLabel}
                        type="text"
                        id="group"
                        name="group"
                        fullWidth
                        value={sportActivityName}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },
                        }}
                    />
                    {levelName && <CustomInput
                        label={dictionary.activityDetailsPage.skillLevelLabel}
                        type="text"
                        id="skillLevel"
                        name="skillLevel"
                        fullWidth
                        value={levelName}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },
                        }}
                    />}
                    {role!=='Trener' && <CustomInput
                        label={dictionary.activityDetailsPage.coachLabel}
                        type="text"
                        id="trainerName"
                        name="trainerName"
                        fullWidth
                        value={trainerName}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },
                        }}
                    />}
                    {role==='Trener' && !trainerName && clientName && <CustomInput
                        label={dictionary.activityDetailsPage.reservationMadeByLabel}
                        type="text"
                        id="reservationMadeBy"
                        name="reservationMadeBy"
                        fullWidth
                        value={`${clientName} ${clientSurname}`}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },
                        }}
                    />}
                    <CustomInput
                        label={dictionary.activityDetailsPage.courtLabel}
                        type="text"
                        id="court"
                        name="court"
                        fullWidth
                        value={courtName}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },
                        }}
                    />
                    {cost && <CustomInput
                        label={dictionary.activityDetailsPage.costLabel}
                        type="text"
                        id="cost"
                        name="cost"
                        fullWidth
                        value={cost + ' PLN'}
                        size="small"
                        InputProps={{ readOnly: true }}
                        readonlyStyle
                        additionalStyles={{
                            "& .MuiOutlinedInput-root": {
                                borderRadius: "12px",
                                "& fieldset": {
                                    borderColor: "#d3d3d3",
                                },
                            },
                        }}
                    />}

                    {(role === 'Wlasciciel' || role === 'Pracownik administracyjny') && <FormControlLabel
                        control={
                            <Checkbox
                                id="isEquipmentReserved"
                                name="isEquipmentReserved"
                                checked={isEquipmentReserved}
                                onChange={() => { }}
                                sx={{
                                    color: "#8edfb4",
                                    '&.Mui-checked': {
                                        color: "#8edfb4",
                                    },
                                }}
                            />
                        }
                        label={dictionary.activityDetailsPage.isEquipmentReservedLabel}
                    />}
                    {role==='Klient' && <FormControlLabel
                        control={
                            <Checkbox
                                id="isEquipmentReserved"
                                name="isEquipmentReserved"
                                checked={isEquipmentReserved}
                                onChange={()=>{}}
                                sx={{
                                    color: "#8edfb4",
                                    '&.Mui-checked': {
                                        color: "#8edfb4",
                                    },
                                }}
                            />
                        }
                        label={dictionary.activityDetailsPage.isEquipmentIncludedLabel}
                    />}
                    {role==='Trener' && sportActivityName==='Rezerwacja' &&<FormControlLabel
                        control={
                            <Checkbox
                                id="isEquipmentReserved"
                                name="isEquipmentReserved"
                                checked={isEquipmentReserved}
                                onChange={()=>{}}
                                sx={{
                                    color: "#8edfb4",
                                    '&.Mui-checked': {
                                        color: "#8edfb4",
                                    },
                                }}
                            />
                        }
                        label={dictionary.activityDetailsPage.isEquipmentReservedLabel}
                    />}

                    <Box sx={{
                        display: 'flex',
                        justifyContent: 'center',
                        columnGap: '5vw',
                    }}>
                        <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.activityDetailsPage.returnLabel}</GreenButton>
                        {role === 'Klient' &&
                            <GreenButton
                                type="submit"
                                style={{
                                    maxWidth: "10vw",
                                    backgroundColor: "#F46C63"
                                }}
                                hoverBackgroundColor={'#c3564f'}
                                onClick={() => cancelActivityClient()}
                                disabled={!canCancel(activityDetails)}
                            >
                                {dictionary.activityDetailsPage.cancelLabel}
                            </GreenButton>}
                    </Box>
                </Box>

                {participants && <Box sx={{
                    display: 'flex',
                    position: 'absolute',
                    right: '1vw',
                    top: '22vh',
                    minWidth: '25vw',
                    borderRadius: '20px',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                    backgroundColor: 'white',
                    padding: '1.35rem',
                    minHeight:'10vh',
                    display: 'flex',
                    flexDirection: 'column',

                }}>
                    <Typography sx={{ fontSize: '1.5rem', fontWeight: 'bold' }}>
                        {dictionary.activityDetailsPage.participantsLabel}:
                    </Typography>
                    <List component="ol" sx={{ paddingLeft: '1.5rem', listStyleType: 'decimal' }}>
                        {participants.map((participant, index) => (
                            <ListItem
                                key={index}
                                component="li"
                                disablePadding
                                sx={{ display: 'list-item', textAlign: 'left' }}
                            >
                                <ListItemText primary={`${participant.firstName} ${participant.lastName}`} />
                            </ListItem>
                        ))}
                    </List>
                </Box>}
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
                        <Typography sx={{
                            color: 'green',
                            fontWeight: 'Bold',
                            fontSize: '3rem',
                            marginTop: '2vh',

                        }}>
                            {dictionary.activityDetailsPage.successLabel}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography sx={{
                            color: 'black',
                            fontSize: '1.5rem',
                        }}>
                            {dictionary.activityDetailsPage.canceledSuccessLabel}
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
            <Backdrop
                sx={(theme) => ({ color: '#fff', zIndex: theme.zIndex.drawer + 1 })}
                open={openFailureBackdrop}
                onClick={handleCloseFailure}
            >
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
                            {dictionary.activityDetailsPage.failureLabel}
                        </Typography>
                    </Box>
                    <Box>
                        <Typography sx={{
                            color: 'black',
                            fontSize: '1.5rem',
                        }}>
                            {dictionary.activityDetailsPage.canceledFailureLabel}
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
        </>
    );
}

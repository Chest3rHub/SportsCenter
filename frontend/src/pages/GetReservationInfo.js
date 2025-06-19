import Header from "../components/Header";
import { Box, Typography } from "@mui/material";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import GreenButton from "../components/buttons/GreenButton";
import CustomInput from "../components/CustomInput";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import getReservationInfo from "../api/getReservationInfo";
import { Checkbox, FormControlLabel } from '@mui/material';

export default function GetReservationInfo() {

    const { dictionary, token } = useContext(SportsContext);
    const navigate = useNavigate();
    const location = useLocation();
    const { id, offsetFromLocation } = location.state || {};

    const [reservationData, setReservationData] = useState({
        clientEmail: '',
        courtName: '',
        startTime: '',
        endTime: '',
        trainer: '',
        isEquipmentReserved: '',
        isReservationPaid: '',
        isReservationCanceled: '',
        IsMoneyRefunded: '',
        status: '',
    });

    useEffect(() => {
          if (!id) return;

            const fetchReservationData = async () => {
              try {
                const response = await getReservationInfo(token, id);
                const data = await response.json();
                const status = getReservationStatus(data);

                setReservationData({
                    ...data,
                    status, 
                });
              } catch (error) {
              //    console.error("Błąd podczas pobierania danych rezerwacji:", error);
              }
            };
            fetchReservationData();
      }, [id, token]); 

    function handleCancel() {
        navigate('/reservations', {
            state: { offsetFromLocation }  
        });
    }

    function getReservationStatus(reservation) {
        const now = new Date();
        const startTime = new Date(reservation.startTime);
        const endTime = new Date(reservation.endTime);

        if (reservation.isMoneyRefunded) {
            return dictionary.reservationInfoPage.statusRefunded;
        }
    
        if (reservation.isReservationCanceled) {
            return dictionary.reservationInfoPage.statusCanceled;
        }
    
        if (endTime < now) {
            return dictionary.reservationInfoPage.statusCompleted;
        }

        if (reservation.isReservationPaid) {
            return dictionary.reservationInfoPage.statusPaid;
        }
    
        if (startTime > now) {
            return dictionary.reservationInfoPage.statusPlanned;
        }
    
        return dictionary.reservationInfoPage.statusUnknown;
    } 

    return (
        <GreenBackground height={"55vh"} marginTop={"2vh"}>
        <Header>{dictionary.reservationInfoPage.title}</Header>
        <OrangeBackground width="40%">
            <Box sx={{
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
                width: "80%",
                }}
            >
            <Box sx={{ 
                display: "flex", 
                flexDirection: "column", 
                justifyContent: "center",
                alignItems: "center", 
                width: "100%", 
                gap: "1rem", 
                marginTop: "4vh",
                marginBottom: "2vh", 
                marginLeft: "9vh", 
                }}>
            
            <CustomInput
                label={dictionary.reservationInfoPage.clientEmailLabel}
                type="text"
                id="clientEmail"
                name="clientEmail"
                fullWidth
                value={reservationData.clientEmail}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
            />

            <CustomInput
                label={dictionary.reservationInfoPage.courtNameLabel}
                type="text"
                id="courtName"
                name="courtName"
                fullWidth
                value={reservationData.courtName}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
            />

            <CustomInput
                label={dictionary.reservationInfoPage.startTimeLabel}
                type="datetime-local"
                id="startTime"
                name="startTime"
                fullWidth
                value={reservationData.startTime}
                size="small"
                InputLabelProps={{
                    shrink: true
                }}
                InputProps={{ readOnly: true }}
                readonlyStyle
            />

            <CustomInput
                label={dictionary.reservationInfoPage.endTimeLabel}
                type="datetime-local"
                id="endTime"
                name="endTime"
                fullWidth
                value={reservationData.endTime}
                size="small"
                InputLabelProps={{
                    shrink: true
                }}
                InputProps={{ readOnly: true }}
                readonlyStyle
            />

            <CustomInput
                label={dictionary.reservationInfoPage.trainerNameLabel}
                type="text"
                id="trainer"
                name="trainer"
                fullWidth
                value={reservationData.trainer}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
            />

            <CustomInput
                label={dictionary.reservationInfoPage.costLabel}
                type="number"
                id="cost"
                name="cost"
                fullWidth
                value={reservationData.cost}
                size="small"
                InputProps={{ readOnly: true }}
                InputLabelProps={{
                    shrink: true
                }}
                readonlyStyle
            />

            <CustomInput
                label={dictionary.reservationInfoPage.statusLabel}
                type="text"
                id="status"
                name="status"
                fullWidth
                value={reservationData.status}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
            />

            <FormControlLabel
                control={
                <Checkbox
                    id="isEquipmentReserved"
                    name="isEquipmentReserved"
                    checked={reservationData.isEquipmentReserved}
                    disabled
                    sx={{
                        color: "#8edfb4",
                        '&.Mui-checked': {
                            color: "#8edfb4",
                        },
                    }}
                />
                }
                label={dictionary.reservationInfoPage.isEquipmentReservedLabel}
            />

            </Box>
            </Box>

            <Box sx={{  
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: 'center',
                    columnGap: "4vw" 
                    }}>
                  <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.getActivityInfoPage.returnLabel}</GreenButton>
            </Box>
        </OrangeBackground>
        </GreenBackground>
    );
}
import Header from "../components/Header";
import { Box, Typography, Modal } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import ActivitiesButton from "../components/buttons/ActivitiesButton";
import getActivityInfo from "../api/getActivityInfo";
import { useNavigate, useLocation } from "react-router-dom";
import GreenButton from "../components/buttons/GreenButton";
import GreyButton from "../components/buttons/GreyButton";
import ChangePageButton from "../components/buttons/ChangePageButton";
import CustomInput from "../components/CustomInput";
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";


export default function GetActivityInfo() {

      const { dictionary, token } = useContext(SportsContext);
      const navigate = useNavigate();
      const location = useLocation();
      const { id, offsetFromLocation } = location.state || {};
    
      const [activityData, setActivityData] = useState({
          sportActivityId: 0,
          activityName: '',
          levelName: '',
          activityDate: '',
          startDate: '',
          dayOfWeek: '',
          startHour: '',
          durationInMinutes: 0,
          courtName: '',
          maxParticipants: 0,
          costWithoutEquipment: 0,
          costWithEquipment: 0,
          isCanleced: false
      });
        
        useEffect(() => {
          if (!id) return;

            const fetchActivityData = async () => {
              try {
                const response = await getActivityInfo(token, id);
                const data = await response.json();
                setActivityData(data);
              } catch (error) {
                  console.error("Błąd podczas pobierania danych zajęć:", error);
              }
            };
            fetchActivityData();
      }, [id, token]); 

      function handleCancelActivityInstance(id) {
              // handleClose();
              // cancelActivity(id)
              //     .then(response => { })
              //     .then(data => {
              //         console.log("Zajecia odwołane:", data);
              //         setStateToTriggerUseEffectAfterDeleting((prev) => !prev);
      
              //     })
              //     .catch(error => {
              //         console.error("Błąd podczas zwalniania pracownika:", error);
              //     });
        }

        function handleCancel() {
          navigate('/trainings', {
              state: { offsetFromLocation }  
            });
      }
    
        return (
            <GreenBackground height={"55vh"} marginTop={"2vh"}>
            <Header>{dictionary.getActivityInfoPage.title}</Header>
            <OrangeBackground width="70%">
    
                <Box sx={{  
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: 'center',
                    columnGap: "4vw" 
                    }}>
                  <ActivitiesButton backgroundColor={"#f0aa4f"} onClick={() => handleCancelActivityInstance(id)} minWidth={'11vw'}>
                      {dictionary.getActivityInfoPage.cancelActivityInstanceLabel}
                  </ActivitiesButton>
                  <GreenButton onClick={handleCancel} style={{ maxWidth: "10vw", backgroundColor: "#F46C63" }} hoverBackgroundColor={'#c3564f'}>{dictionary.getActivityInfoPage.returnLabel}</GreenButton>
                  
                </Box>
    
                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "row",
                        justifyContent: "space-between",
                        width: "100%",
                    }}
                >
                <Box sx={{ 
                    display: "flex", 
                    flexDirection: "column", 
                    gap: "1rem", 
                    marginTop: "4vh",
                    marginBottom: "2vh",  
                    width: "60%"
                  }}>
                  <CustomInput
                    label={dictionary.getActivityInfoPage.sportActivityId}
                    type="number"
                    id="sportActivityId"
                    name="sportActivityId"
                    fullWidth
                    value={activityData.sportActivityId}
                    size="small"
                    InputProps={{ readOnly: true }}
                    readonlyStyle
                  />
                  <CustomInput
                    label={dictionary.getActivityInfoPage.sportActivityName}
                    type="text"
                    id="sportActivityName"
                    name="sportActivityName"
                    fullWidth
                    value={activityData.activityName}
                    size="small"
                    InputProps={{ readOnly: true }}
                    readonlyStyle
                  />
                  <CustomInput
                    label={dictionary.getActivityInfoPage.sportActivityLevelName}
                    type="text"
                    id="sportActivityLevelName"
                    name="sportActivityLevelName"
                    fullWidth
                    value={activityData.levelName}
                    size="small"
                    InputProps={{ readOnly: true }}
                    readonlyStyle
                  />
                  <CustomInput
                    label={dictionary.getActivityInfoPage.startDate}
                    type="date"
                    id="startDate"
                    name="startDate"
                    fullWidth
                    value={activityData.startDate}
                    size="small"
                    InputLabelProps={{
                      shrink: true
                    }}
                    InputProps={{ readOnly: true }}
                    readonlyStyle
                  />
                  <CustomInput
                            label={dictionary.getActivityInfoPage.dayOfWeek}
                            type="text"
                            fullWidth
                            value={activityData.dayOfWeek}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />
                        <CustomInput
                            label={dictionary.getActivityInfoPage.startHour}
                            type="time"
                            fullWidth
                            value={activityData.startHour}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />
                        <CustomInput
                            label={dictionary.getActivityInfoPage.durationInMinutes}
                            type="number"
                            fullWidth
                            value={activityData.durationInMinutes}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />
                        <CustomInput
                            label={dictionary.getActivityInfoPage.courtName}
                            type="text"
                            fullWidth
                            value={activityData.courtName}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />
                        <CustomInput
                            label={dictionary.getActivityInfoPage.maxParticipants}
                            type="number"
                            fullWidth
                            value={activityData.maxParticipants}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />
                        <CustomInput
                            label={dictionary.getActivityInfoPage.costWithoutequipment}
                            type="number"
                            fullWidth
                            value={activityData.costWithoutEquipment}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />
                        <CustomInput
                            label={dictionary.getActivityInfoPage.costWithEquipment}
                            type="number"
                            fullWidth
                            value={activityData.costWithEquipment}
                            size="small"
                            InputProps={{ readOnly: true }}
                            readonlyStyle
                        />                
                </Box>
    
                {/* <Box sx={{ width: "48%" }}> */}
                  {/* <Typography variant="h6" align="left">{dictionary.accountPage.role}: {activityData.role}</Typography> */}
                      {/* {activityData.role === "Wlasciciel" && ( */}
                        <>
                          {/* <Typography variant="h6" align="left" sx={{ marginBottom: "20px" }}>
                            {dictionary.accountPage.balance}: {activityData.balance} zł
                          </Typography> */}
                          {/* <Box sx={{ display: "flex", flexDirection: "column", marginBottom: "20px" }}> */}
                           {/* // <GreenButton onClick={handleAddBalance}>{dictionary.accountPage.addBalance}</GreenButton> */}
                          {/* </Box> */}
                        </>
                      {/* )} */}
                  <Box sx={{ display: "flex", flexDirection: "column"}}>
                    {/* //<GreenButton onClick={handleChangePassword}>{dictionary.accountPage.changePassword}</GreenButton> */}
                  </Box>
                </Box>
                {/* </Box> */}
            </OrangeBackground>
            </GreenBackground>
        );
}
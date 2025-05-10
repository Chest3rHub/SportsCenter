import Header from "../components/Header";
import { Box, Typography, Modal } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import ActivitiesButton from "../components/buttons/ActivitiesButton";
import getScheduleActivities from "../api/getScheduleActivities";
import { useNavigate} from "react-router-dom";
import GreenButton from "../components/buttons/GreenButton";
import GreyButton from "../components/buttons/GreyButton";
import ChangePageButton from "../components/buttons/ChangePageButton";
import deleteSportActivity from "../api/deleteSportActivity";

export default function SportActivities() {

    const { dictionary, role  } = useContext(SportsContext);

    const navigate = useNavigate();

    const [activities, setActivities] = useState([]);
    const [loading, setLoading] = useState(true);
    const [offset, setOffset] = useState(0);

    const [selectedActivity, setSelectedActivity] = useState(null);
    const handleOpenModal = (activity) => setSelectedActivity(activity);
    const handleCloseModal = () => setSelectedActivity(null);

    const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);

    const maxActivitiesPerPage = 6;
    const activitiesRequiredToEnablePagination = 7;

    useEffect(() => {
        console.log('Aktualny offset:', offset);
    
        getScheduleActivities(offset)
            .then(response => {
                console.log('Response:', response);
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                setActivities(data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getScheduleActivities:', error);
            });
    }, [offset, stateToTriggerUseEffectAfterDeleting]);
    
    
    function handleShowMoreInfo(id) {
        navigate(`/get-sport-activity-with-id`, {
            state: { id }
        });
    }

    function handleShowActivitiesSummary() {
        navigate(`/get-activity-summary`, {
            //state: { id }
        });
    }

    const handleDeleteActivity = async () => {
        if (!selectedActivity) return;
      
        try {
          const response = await deleteSportActivity(selectedActivity.sportActivityId);
      
          if (!response.ok) {
            const errorData = await response.json();
            console.error("Błąd usuwania:", errorData);
            alert("Wystąpił błąd podczas usuwania zajęć.");
            return;
          }
      
          alert("Zajęcia zostały usunięte.");
          handleCloseModal();
          setStateToTriggerUseEffectAfterDeleting(prev => !prev);
        } catch (error) {
          console.error("Błąd połączenia z serwerem:", error);
          alert("Nie udało się połączyć z serwerem.");
        }
      };
      

    function handleAddActivity() {
        navigate(`/Add-activity`, {
        });
    }

    function handleNextPage() {
        if (activities.length < 6) {
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
    
    const limitedActivities = activities.slice(0, maxActivitiesPerPage);

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
                <Header>{dictionary.sportActivitiesPage.sportActivitiesLabel}</Header>
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
                        onClick={() => handleAddActivity()}
                        style={{
                        minWidth: '7vw',
                        height: '2.8rem',
                        paddingLeft: '1rem',
                        paddingRight: '1rem',
                        fontSize: '0.9rem',
                        whiteSpace: 'nowrap',
                        }}
                    >
                        {dictionary.sportActivitiesPage.addActivityLabel}
                    </GreenButton>
                    <GreyButton
                        onClick={() => handleShowActivitiesSummary()} 
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
                        {dictionary.sportActivitiesPage.showSummaryLabel}
                    </GreyButton>
                </Box>
                <Box
                    sx={{
                        height: '67vh',
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
                        <SmallGreenHeader width={'22%'}>{dictionary.sportActivitiesPage.sportActivityId}</SmallGreenHeader>
                        <SmallGreenHeader width={'22%'}>{dictionary.sportActivitiesPage.sportActivityName}</SmallGreenHeader>
                        <SmallGreenHeader width={'24%'}>{dictionary.sportActivitiesPage.sportActivityLevelName}</SmallGreenHeader>                        <SmallGreenHeader width={'16.6%'}>{dictionary.sportActivitiesPage.dayOfWeek}</SmallGreenHeader>
                        <SmallGreenHeader width={'22%'}>{dictionary.sportActivitiesPage.startHour}</SmallGreenHeader>
                        <SmallGreenHeader width={'22%'}>{dictionary.sportActivitiesPage.courtName}</SmallGreenHeader>
                        </Box>
                        {limitedActivities.map((activity) => (<Box
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
                            <Box
                                sx={{
                                    width: '20%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {activity.sportActivityId}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '20%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {activity.activityName}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '23%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {activity.levelName}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '20%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {activity.dayOfWeek}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '20%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {activity.startHour.slice(0, 5)}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '20%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {activity.courtName}
                                </Typography>
                            </Box>
                            </Box>
                            <ActivitiesButton backgroundColor={"#f0aa4f"} onClick={() => handleShowMoreInfo(activity.sportActivityId)} minWidth={'6vw'}>
                                {dictionary.sportActivitiesPage.moreInfoLabel}
                            </ActivitiesButton>
                            <ActivitiesButton backgroundColor={"#F46C63"}onClick={() => handleOpenModal(activity)} minWidth={'6vw'}>
                                {dictionary.sportActivitiesPage.deleteActivityLabel}
                            </ActivitiesButton>                    
                        </Box>))}
                    </Box>
                    <Modal 
                        open={!!selectedActivity} 
                        onClose={handleCloseModal}
                    >
                        <Box sx={{
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
                            {dictionary.sportActivitiesPage.confirmLabel}
                        </Typography>
                        <Typography sx={{
                            color: 'black',
                            fontSize: '1.5rem',
                        }}>{selectedActivity ? selectedActivity.activityName : ''}</Typography>
                        <Box sx={{ display: 'flex', gap: '3rem', marginTop: '1rem' }}>
                            <GreenButton onClick={handleCloseModal} style={{ maxWidth: "10vw", backgroundColor: "#F46C63", minWidth: '7vw' }} hoverBackgroundColor={'#c3564f'}>{dictionary.sportActivitiesPage.noLabel}</GreenButton>
                            <GreenButton onClick={handleDeleteActivity} style={{ maxWidth: "10vw", minWidth: '7vw' }}>{dictionary.sportActivitiesPage.yesLabel}</GreenButton>
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
                        <ChangePageButton disabled={offset === 0} onClick={handlePreviousPage} backgroundColor={"#F46C63"} minWidth={"10vw"}>{dictionary.sportActivitiesPage.previousLabel}</ChangePageButton>
                        <ChangePageButton disabled={activities.length < activitiesRequiredToEnablePagination} onClick={handleNextPage} backgroundColor={"#8edfb4"} minWidth={"10vw"}>{dictionary.sportActivitiesPage.nextLabel}</ChangePageButton>
                    </Box>}
                </Box>
            </>
        );
}
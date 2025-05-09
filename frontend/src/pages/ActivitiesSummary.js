import Header from "../components/Header";
import { Box, Typography } from "@mui/material";
import { SportsContext } from "../context/SportsContext";
import { useContext, useState, useEffect, } from "react";
import { useNavigate } from "react-router-dom";
import GreenButton from "../components/buttons/GreenButton";
import GreyButton from "../components/buttons/GreyButton";
import getActivitySummary from "../api/getActivitySummary";
import CustomInput from "../components/CustomInput";
import SmallGreenHeader from "../components/SmallGreenHeader";
import ChangePageButton from "../components/buttons/ChangePageButton";

export default function SportActivities() {

    const { dictionary, token } = useContext(SportsContext);
     const navigate = useNavigate();

    const [activitiesSummary, setActivitiesSummary] = useState([]);
    const [loading, setLoading] = useState(true);
    const [offset, setOffset] = useState(0);

    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [dateError, setDateError] = useState({});

    //by pamietac filtr podczas przechodzenia na kolejne strony (paginacja)
    const [isFilteringByDate, setIsFilteringByDate] = useState(false);
    const [startDateValue, setstartDateValue] = useState(null);
    const [endDateValue, setendDateValue] = useState(null);
   
    const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);

    const maxActivitiesPerPage = 5;
    const activitiesRequiredToEnablePagination = 6;

    const limitedActivities = activitiesSummary.slice(0, maxActivitiesPerPage);

    function handleClearFilters() {
        setStartDate('');
        setEndDate('');
        setDateError({});
        setOffset(0);
        setLoading(true);
        setIsFilteringByDate(false);
        setActivitiesSummary([]);
    }

    function handleSearchByDate() {
        if (!startDate || !endDate) {
            setDateError({
                startDateError: !startDate ? dictionary.activitiesSummaryPage.dateRequiredError : '',
                endDateError: !endDate ? dictionary.activitiesSummaryPage.dateRequiredError : ''
            });
            return;
        }
    
        if (new Date(startDate) > new Date(endDate)) {
            setDateError({
                startDateError: dictionary.activitiesSummaryPage.dateRangeError,
                endDateError: ''
            });
            return;
        }
    
        setDateError({});
        setIsFilteringByDate(true);
        setOffset(0);
    }
    

    const fetchActivitiesSummary = async () => {
        try {
            setLoading(true);
            const response = await getActivitySummary(token, startDate, endDate, offset);
    
            if (!response.ok) {
                const errorText = await response.text();
                console.error("API responded with error:", errorText);
                throw new Error('Failed to fetch activities summary');
            }
    
            const data = await response.json();
            setActivitiesSummary(data.summariesByZajecia || []);
        } catch (error) {
            console.error("Error fetching activities summary:", error);
        } finally {
            setLoading(false);
        }
    };
    
    
    function handleNextPage() {
        if (activitiesSummary.length < 4) {
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

    useEffect(() => {
        setLoading(true);
        fetchActivitiesSummary();
    }, [offset, isFilteringByDate, startDate, endDate]);
    

    return (
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
            <Header>{dictionary.activitiesSummaryPage.title}</Header>

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
                <CustomInput
                    InputLabelProps={{ shrink: true }}
                    label={dictionary.activitiesSummaryPage.startDateLabel}
                    type="date"
                    value={startDate}
                    onChange={(e) => setStartDate(e.target.value)}
                    error={Boolean(dateError.startDateError)}
                    helperText={dateError.startDateError}
                    placeholder={dictionary.activitiesSummaryPage.startDatePlaceholder}
                    additionalStyles={{ marginLeft: '1vw', minWidth: '8vw', marginRight: '1.3rem' }}
                />
                <CustomInput
                    InputLabelProps={{ shrink: true }}
                    label={dictionary.activitiesSummaryPage.endDateLabel}
                    type="date"
                    value={endDate}
                    onChange={(e) => setEndDate(e.target.value)}
                    error={Boolean(dateError.endDateError)}
                    helperText={dateError.endDateError}
                    placeholder={dictionary.activitiesSummaryPage.endDatePlaceholder}
                    additionalStyles={{ minWidth: '8vw', marginRight: '1.5rem' }}
                />
                <GreenButton
                    onClick={handleSearchByDate}
                    style={{
                        minWidth: '7vw',
                        height: '2.8rem',
                        paddingLeft: '1rem',
                        paddingRight: '1rem',
                        fontSize: '0.9rem',
                        whiteSpace: 'nowrap',
                    }}
                >
                    {dictionary.activitiesSummaryPage.searchLabel}
                </GreenButton>
                <GreyButton
                    onClick={handleClearFilters}
                    style={{
                        minWidth: '7vw',
                        height: '2.8rem',
                        paddingLeft: '1rem',
                        paddingRight: '1rem',
                        fontSize: '0.9rem',
                        whiteSpace: 'nowrap',
                        color: 'black'
                    }}
                >
                    {dictionary.activitiesSummaryPage.clearLabel}
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
                        width: '80%',
                        gap: '2%',
                        marginBottom: '3vh',
                    }}
                >
                    <SmallGreenHeader width={'20%'}>{dictionary.activitiesSummaryPage.nameLabel}</SmallGreenHeader>
                    <SmallGreenHeader width={'20%'}>{dictionary.activitiesSummaryPage.completedLabel}</SmallGreenHeader>
                    <SmallGreenHeader width={'20%'}>{dictionary.activitiesSummaryPage.cancelledLabel}</SmallGreenHeader>
                    <SmallGreenHeader width={'20%'}>{dictionary.activitiesSummaryPage.totalRevenueLabel}</SmallGreenHeader>
                </Box>

                {limitedActivities.map((summary) => (
                    <Box
                        sx={{
                            marginTop: '1vh',
                            display: 'flex',
                            alignContent: 'start',
                            alignItems: 'center',
                            width: '80%',
                            gap: '2%',
                            padding: '0.6rem 0px',
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
                                maxWidth: '41.5vw'
                            }}
                        >
                            <Box sx={{ width: '20%', textAlign: 'center' }}>
                                <Typography>{summary.zajeciaNazwa}</Typography>
                            </Box>
                            <Box sx={{ width: '20%', textAlign: 'center' }}>
                                <Typography>{summary.completedActivities}</Typography>
                            </Box>
                            <Box sx={{ width: '20%', textAlign: 'center'}}>
                                <Typography>{summary.cancelledActivities}</Typography>
                            </Box>
                            <Box sx={{width: '20%', textAlign: 'center'}}>
                            <Typography>{summary.totalRevenue} zł</Typography>
                            </Box>
                        </Box>
                    </Box>))}
            </Box>
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
                        {dictionary.clientsPage.previousLabel}
                    </ChangePageButton>
                    <ChangePageButton
                        disabled={activitiesSummary.length < activitiesRequiredToEnablePagination}
                        onClick={handleNextPage}
                        backgroundColor={"#8edfb4"}
                        minWidth={"10vw"}
                    >
                        {dictionary.clientsPage.nextLabel}
                    </ChangePageButton>
                </Box>
        </Box>
    );
}

import Header from "../components/Header";
import { Box, Typography, Modal } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import ClientsButton from "../components/ClientsButton";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import getClients from "../api/getClients";
import getClientsByAge from "../api/getClientsByAge";
import { useNavigate } from "react-router-dom";
import GreenButton from "../components/GreenButton";
import GreyButton from "../components/GreyButton";
import ChangePageButton from "../components/ChangePageButton";
import CustomInput from "../components/CustomInput";

export default function Clients() {

    const { dictionary, token, role } = useContext(SportsContext);
    const navigate = useNavigate();

    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(true);

    const [selectedClient, setSelectedClient] = useState(null);
    const [offset, setOffset] = useState(0);

    const [minAge, setMinAge] = useState('');
    const [maxAge, setMaxAge] = useState('');
    const [ageError, setAgeError] = useState('');

    //by pamietac filtr wiekowy podczas przechodzenia na kolejne strony (paginacja)
    const [isFilteringByAge, setIsFilteringByAge] = useState(false);
    const [minAgeValue, setMinAgeValue] = useState(null);
    const [maxAgeValue, setMaxAgeValue] = useState(null);


    const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);


    const handleOpen = (client) => setSelectedClient(client);;
    const handleClose = () => setSelectedClient(null);;


    const maxClientsPerPage = 6;
    const clientsRequiredToEnablePagination = 7;

    const fetchClients = async () => {
        try {
            let response;
            
            if (isFilteringByAge && minAgeValue !== null && maxAgeValue !== null) {
                response = await getClientsByAge(token, minAgeValue, maxAgeValue, offset);
            } else {
                response = await getClients(token, offset);
            }

            if (!response.ok) {
                throw new Error('Failed to fetch clients');
            }

            const data = await response.json();
            setClients(data);
        } catch (error) {
            console.error('Error fetching clients:', error);
        } finally {
            setLoading(false);
        }
    };


    useEffect(() => {
        setLoading(true);
        fetchClients();
    }, [offset, stateToTriggerUseEffectAfterDeleting]);


    function handleClearFilters() {
        setMinAge('');
        setMaxAge('');
        setAgeError({ minAgeError: '', maxAgeError: '' });
        setIsFilteringByAge(false);
        setMinAgeValue(null);
        setMaxAgeValue(null);
        setOffset(0);
        setLoading(true);
        fetchClients();
    }


    function handleChangeClientPassword(id) {
        navigate(`/change-password`, {
            state: { id }
        });
    }

    function handleGiveDiscount(email) {
        navigate(`/add-client-discount`, {
            state: { email }
        });
    }

    function handleAddDeposit(email) {
        navigate(`/add-deposit-to-client`, {
            state: { email }
        });
    }

    function handleChangeDeposit(client) {
        navigate(`/update-client-deposit`, {
            state: {
                email: client.email,
                name: client.name,
                surname: client.surname
            }
        });
    }


    function handleNextPage() {
        if (clients.length < 6) {
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

    function handleSearchByAge() {
        const min = Number(minAge);
        const max = Number(maxAge);

        let minAgeError = '';
        let maxAgeError = '';

        console.log("Wysyłam minAge:", minAge, "maxAge:", maxAge);


        if (!minAge || !maxAge) {
            if (!minAge) minAgeError = dictionary.clientsPage.ageFieldsRequiredError;
            if (!maxAge) maxAgeError = dictionary.clientsPage.ageFieldsRequiredError;
        } else {

            if (min <= 0 || max <= 0) {
                if (min <= 0) minAgeError = dictionary.clientsPage.ageMustBePositiveError;
                if (max <= 0) maxAgeError = dictionary.clientsPage.ageMustBePositiveError;
            } else if (min > max) {
                minAgeError = dictionary.clientsPage.ageRangeError;
            }
        }

        if (minAgeError || maxAgeError) {
            setAgeError({ minAgeError, maxAgeError });
            return;
        }

        setAgeError({ minAgeError: '', maxAgeError: '' });
        setMinAgeValue(min);
        setMaxAgeValue(max);
        setIsFilteringByAge(true);
        setOffset(0);

        getClientsByAge(token, min, max, 0)
            .then(response => response.json())
            .then(data => {
                setClients(data);
                setOffset(0);
            })
            .catch(error => {
                console.error("Błąd pobierania danych:", error);
            });
    }

    const limitedClients = clients.slice(0, maxClientsPerPage);

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
                <Header>{dictionary.clientsPage.clientsLabel}</Header>
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
                        label={dictionary.clientsPage.ageFromLabel}
                        type="number"
                        value={minAge}
                        onChange={(e) => setMinAge(e.target.value)}
                        error={Boolean(ageError.minAgeError)}
                        helperText={ageError.minAgeError}
                        placeholder={dictionary.clientsPage.minAgePlaceholder}
                        additionalStyles={{ marginLeft:'1vw',minWidth: '8vw', marginRight:'1.3rem' }}
                    />
                    <CustomInput
                        label={dictionary.clientsPage.ageToLabel}
                        type="number"
                        value={maxAge}
                        onChange={(e) => setMaxAge(e.target.value)}
                        error={Boolean(ageError.maxAgeError)}
                        helperText={ageError.maxAgeError}
                        placeholder={dictionary.clientsPage.maxAgePlaceholder}
                        sx={{ width: '12vw' }}
                        additionalStyles={{ minWidth: '8vw',marginRight:'1.5rem' }}
                    />
                    <GreenButton
                        onClick={handleSearchByAge}
                        style={{
                            minWidth: '7vw',
                            height: '2.8rem',
                            paddingLeft: '1rem',
                            paddingRight: '1rem',
                            fontSize: '0.9rem',
                            whiteSpace: 'nowrap',
                        }}
                    >
                        {dictionary.clientsPage.searchLabel}
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
                        {dictionary.clientsPage.clearLabel}
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
                        <SmallGreenHeader width={'20%'}>{dictionary.clientsPage.clientLabel}</SmallGreenHeader>
                        <SmallGreenHeader width={'20%'}>{dictionary.clientsPage.emailLabel}</SmallGreenHeader>
                    </Box>
                    {limitedClients.map((client) => (<Box key={client.id}
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
                                width: '60.8%',
                                borderRadius: '70px',
                                backgroundColor: 'white',
                                boxShadow: '0 5px 5px rgb(0, 0, 0, 0.6)',
                                display: 'flex',
                                justifyContent: 'space-between',
                                alignItems: 'center',
                                paddingTop: '0.6rem',
                                paddingBottom: '0.4rem',
                                maxWidth: '20.5vw'
                            }}
                        >
                            <Box
                                sx={{
                                    width: '50%',
                                    textAlign: 'center',
                                }}
                            >
                                <Typography>
                                    {client.name} {client.surname}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '50%',
                                    textAlign: 'center',


                                }}
                            >
                                <Typography>
                                    {client.email}
                                </Typography>
                            </Box>
                        </Box>
                        {role === 'Wlasciciel' && <ClientsButton backgroundColor={"#f0aa4f"} onClick={() => handleChangeClientPassword(client.id)} minWidth={'9vw'}>
                            {dictionary.clientsPage.changePasswordLabel}
                        </ClientsButton>}
                        <ClientsButton backgroundColor={"#8edfb4"} onClick={() => handleGiveDiscount(client.email)} minWidth={'9vw'}>
                            {dictionary.clientsPage.giveDiscountLabel}
                        </ClientsButton>
                        <ClientsButton backgroundColor={"#f0aa4f"} onClick={() => handleAddDeposit(client.email)} minWidth={'9vw'}>
                            {dictionary.clientsPage.addDepositLabel}
                        </ClientsButton>
                        <ClientsButton backgroundColor={"#F46C63"} onClick={() => handleChangeDeposit(client)} minWidth={'9vw'}>
                            {dictionary.clientsPage.changeDeposit}
                        </ClientsButton>
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
                        disabled={clients.length < clientsRequiredToEnablePagination}
                        onClick={handleNextPage}
                        backgroundColor={"#8edfb4"}
                        minWidth={"10vw"}
                    >
                        {dictionary.clientsPage.nextLabel}
                    </ChangePageButton>
                </Box>
            </Box>
        </>
    );

}
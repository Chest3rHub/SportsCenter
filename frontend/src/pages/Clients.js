import Header from "../components/Header";
import { Box, Typography } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import ClientsButton from "../components/ClientsButton";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import getClients from "../api/getClients";
import getClientsByAge from "../api/getClientsByAge";
import { useNavigate } from "react-router-dom";
import CustomInput from "../components/CustomInput";
import GreenButton from "../components/GreenButton";
import ChangePageButton from "../components/ChangePageButton";

export default function Clients() {
    const { dictionary, token } = useContext(SportsContext);
    const navigate = useNavigate();

    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(true);

    const [offset, setOffset] = useState(0);

    const [minAge, setMinAge] = useState('');
    const [maxAge, setMaxAge] = useState('');

    const [ageError, setAgeError] = useState('');

    const maxClientsPerPage = 6;
    const clientsRequiredToEnablePagination = 7;


    useEffect(() => {
        setLoading(true);
        const fetchClients = async () => {
            try {
                const response = minAge || maxAge
                    ? await getClientsByAge(token, minAge, maxAge, offset)
                    : await getClients(token, offset);

                if (!response.ok) throw new Error('Błąd pobierania danych');

                const data = await response.json();
                setClients(data);
            } catch (error) {
                console.error('Błąd podczas pobierania klientów:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchClients();
    }, [offset, minAge, maxAge, token]);

    function handleChangeClientPassword(id) {
        navigate(`/change-password`, { state: { id } });
    }

    function handleGiveDiscount(email) {
        navigate(`/add-client-discount`, { state: { email } });
    }

    function handleAddDeposit(email) {
        navigate(`/add-deposit-to-client`, { state: { email } });
    }

    function handleChangeDeposit(client) {
        navigate(`/update-client-deposit`, {
            state: {
                email: client.email,
                name: client.name,
                surname: client.surname,
            },
        });
    }

    function handleNextPage() {
        if (clients.length >= 6) setOffset((prev) => prev + 1);
    }

    function handlePreviousPage() {
        if (offset > 0) setOffset((prev) => prev - 1);
    }

    function handleSearchByAge() {
        const min = Number(minAge);
        const max = Number(maxAge);
    
        let minAgeError = '';
        let maxAgeError = '';

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
        setOffset(0);
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
                    margin: 'auto',
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
                sx={{ width: '12vw' }}
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
                            alignItems: 'center',
                            width: '80%',
                            marginBottom: '3vh',
                        }}
                    >
                        <SmallGreenHeader width={'20%'}>{dictionary.clientsPage.clientLabel}</SmallGreenHeader>
                        <SmallGreenHeader width={'20%'}>{dictionary.clientsPage.emailLabel}</SmallGreenHeader>
                    </Box>
                    {limitedClients.map((client) => (
                        <Box
                            key={client.id}
                            sx={{
                                marginTop: '1vh',
                                display: 'flex',
                                alignItems: 'center',
                                width: '100%',
                                padding: '0.6rem 0',
                            }}
                        >
                            <Box
                                sx={{
                                    width: '60.8%',
                                    borderRadius: '70px',
                                    backgroundColor: 'white',
                                    boxShadow: '0 5px 5px rgba(0, 0, 0, 0.6)',
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                    alignItems: 'center',
                                    paddingY: '0.5rem',
                                }}
                            >
                                <Box sx={{ width: '50%', textAlign: 'center' }}>
                                    <Typography>{client.name} {client.surname}</Typography>
                                </Box>
                                <Box sx={{ width: '50%', textAlign: 'center' }}>
                                    <Typography>{client.email}</Typography>
                                </Box>
                            </Box>
                            <ClientsButton backgroundColor="#f0aa4f" onClick={() => handleChangeClientPassword(client.id)} minWidth="9vw">
                                {dictionary.clientsPage.changePasswordLabel}
                            </ClientsButton>
                            <ClientsButton backgroundColor="#8edfb4" onClick={() => handleGiveDiscount(client.email)} minWidth="9vw">
                                {dictionary.clientsPage.giveDiscountLabel}
                            </ClientsButton>
                            <ClientsButton backgroundColor="#f0aa4f" onClick={() => handleAddDeposit(client.email)} minWidth="9vw">
                                {dictionary.clientsPage.addDepositLabel}
                            </ClientsButton>
                            <ClientsButton backgroundColor="#F46C63" onClick={() => handleChangeDeposit(client)} minWidth="9vw">
                                {dictionary.clientsPage.changeDeposit}
                            </ClientsButton>
                        </Box>
                    ))}
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
                        backgroundColor="#F46C63"
                        minWidth="10vw"
                    >
                        {dictionary.clientsPage.previousLabel}
                    </ChangePageButton>
                    <ChangePageButton
                        disabled={clients.length < clientsRequiredToEnablePagination}
                        onClick={handleNextPage}
                        backgroundColor="#8edfb4"
                        minWidth="10vw"
                    >
                        {dictionary.clientsPage.nextLabel}
                    </ChangePageButton>
                </Box>
            </Box>
        </>
    );
}

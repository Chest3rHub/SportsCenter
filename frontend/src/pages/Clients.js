import Header from "../components/Header";
import { Box, Typography, Modal } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import ClientsButton from "../components/ClientsButton";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import getClients from "../api/getClients";
import { useNavigate } from "react-router-dom";
import GreenButton from "../components/GreenButton";
import ChangePageButton from "../components/ChangePageButton";
export default function Clients() {

    const { dictionary, token } = useContext(SportsContext);
    const navigate = useNavigate();

    const [clients, setClients] = useState([]);
    const [loading, setLoading] = useState(true);

    const [selectedClient, setSelectedClient] = useState(null);
    const [offset, setOffset] = useState(0);
    const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);


    const handleOpen = (client) => setSelectedClient(client);;
    const handleClose = () => setSelectedClient(null);;


    const maxClientsPerPage = 6;
    const clientsRequiredToEnablePagination = 7;


    useEffect(() => {
        getClients(token, offset)
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                setClients(data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getClients:', error);
            });
    }, [offset, stateToTriggerUseEffectAfterDeleting]);

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
                        <SmallGreenHeader width={'24%'}>{dictionary.clientsPage.clientLabel}</SmallGreenHeader>
                        <SmallGreenHeader width={'24%'}>{dictionary.clientsPage.emailLabel}</SmallGreenHeader>
                    </Box>
                    {limitedClients.map((client) => (<Box
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
                        <ClientsButton backgroundColor={"#f0aa4f"} onClick={() => handleChangeClientPassword(client.id)} minWidth={'11vw'}>
                            {dictionary.clientsPage.changePasswordLabel}
                        </ClientsButton>
                        <ClientsButton backgroundColor={"#8edfb4"} onClick={() => handleGiveDiscount(client.email)} minWidth={'11vw'}>
                            {dictionary.clientsPage.giveDiscountLabel}
                        </ClientsButton>
                        <ClientsButton backgroundColor={"#F46C63"} onClick={() => handleAddDeposit(client.email)} minWidth={'11vw'}>
                            {dictionary.clientsPage.addDepositLabel}
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
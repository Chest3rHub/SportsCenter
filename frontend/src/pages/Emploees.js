import Header from "../components/Header";
import { Box, Typography, Modal } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import EmployeesButton from "../components/EmployeesButton";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import getEmployees from "../api/getEmployees";
import { useNavigate } from "react-router-dom";
import GreenButton from "../components/GreenButton";
import fireEmployee from "../api/fireEmployee";
import ChangePageButton from "../components/ChangePageButton";
export default function Employees() {

    const { dictionary, token } = useContext(SportsContext);
    const navigate = useNavigate();

    const [employees, setEmployees] = useState([]);
    const [loading, setLoading] = useState(true);

    const [selectedEmployee, setSelectedEmployee] = useState(null);
    const [offset, setOffset] = useState(0);
    const [stateToTriggerUseEffectAfterDeleting, setStateToTriggerUseEffectAfterDeleting] = useState(false);


    const handleOpen = (employee) => setSelectedEmployee(employee);;
    const handleClose = () => setSelectedEmployee(null);;

    // max 6 pracownikow na stronie poki co
    const maxEmployeesPerPage = 6;
    const employeesRequiredToEnablePagination = 7;


    useEffect(() => {
        getEmployees(token, offset)
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log('Odpowiedź z API:', data);
                setEmployees(data);
                setLoading(false);
            })
            .catch(error => {
                console.error('Błąd podczas wywoływania getEmployees:', error);
            });
    }, [offset, stateToTriggerUseEffectAfterDeleting]);

    function handleChangeEmployeePassword(id) {
        navigate(`/change-password`, {
            state: { id }
        });
    }

    function handleFireEmployee(id) {
        handleClose();
        fireEmployee(id, token)
            .then(response => { })
            .then(data => {
                console.log("Pracownik zwolniony:", data);
                setStateToTriggerUseEffectAfterDeleting((prev) => !prev);

            })
            .catch(error => {
                console.error("Błąd podczas zwalniania pracownika:", error);
            });
    }

    function handleNextPage() {
        if (employees.length < 6) {
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

    const limitedEmployees = employees.slice(0, maxEmployeesPerPage);

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
                <Header>{dictionary.employeesPage.employeesLabel}</Header>
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
                        <SmallGreenHeader width={'37%'}>{dictionary.employeesPage.employeeLabel}</SmallGreenHeader>
                        <SmallGreenHeader width={'37%'}>{dictionary.employeesPage.positionLabel}</SmallGreenHeader>
                    </Box>
                    {limitedEmployees.map((employee) => (<Box
                        sx={{
                            marginTop: '1vh',
                            display: 'flex',
                            alignContent: 'start',
                            alignItems: 'center',
                            width: '100%',
                            padding: '0.6rem 0px',
                            // tutaj pracownik juz tzn imie nazwisko stanowisko
                        }}
                    >
                        <Box
                            sx={{
                                width: '60.8%',
                                borderRadius: '70px',
                                backgroundColor: employee.fireDate ? '#ffe6e6' : 'white',
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
                                    {employee.fullName}
                                </Typography>
                            </Box>
                            <Box
                                sx={{
                                    width: '50%',
                                    textAlign: 'center',


                                }}
                            >
                                <Typography>
                                    {employee.role}
                                </Typography>
                            </Box>
                        </Box>
                        <EmployeesButton backgroundColor={"#f0aa4f"} onClick={() => handleChangeEmployeePassword(employee.id)} minWidth={'11vw'}>
                            {dictionary.employeesPage.changePasswordLabel}
                        </EmployeesButton>
                        {!employee.fireDate ? <EmployeesButton backgroundColor={"#F46C63"} onClick={() => handleOpen(employee)} minWidth={'11vw'}>
                            {dictionary.employeesPage.fireLabel}
                        </EmployeesButton> : <EmployeesButton backgroundColor={"#F46C63"} onClick={() => { }} minWidth={'11vw'} disabled={employee.fireDate}>
                            {employee.fireDate}
                        </EmployeesButton>}

                    </Box>))}
                </Box>
                <Modal
                    open={selectedEmployee}
                    onClose={handleClose}
                >
                    <Box
                        sx={{
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
                            {dictionary.employeesPage.confirmLabel}
                        </Typography>
                        <Typography sx={{
                            color: 'black',
                            fontSize: '1.5rem',
                        }}>{selectedEmployee ? selectedEmployee.fullName : ''}</Typography>
                        <Box sx={{ display: 'flex', gap: '3rem', marginTop: '1rem' }}>
                            <GreenButton onClick={() => { handleClose() }} style={{ maxWidth: "10vw", backgroundColor: "#F46C63", minWidth: '7vw' }} hoverBackgroundColor={'#c3564f'}>{dictionary.employeesPage.noLabel}</GreenButton>
                            <GreenButton onClick={() => { handleFireEmployee(selectedEmployee.id) }} style={{ maxWidth: "10vw", minWidth: '7vw' }}>{dictionary.employeesPage.yesLabel}</GreenButton>
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
                    <ChangePageButton disabled={offset === 0} onClick={handlePreviousPage} backgroundColor={"#F46C63"} minWidth={"10vw"}>{dictionary.newsPage.previousLabel}</ChangePageButton>
                    <ChangePageButton disabled={employees.length < employeesRequiredToEnablePagination} onClick={handleNextPage} backgroundColor={"#8edfb4"} minWidth={"10vw"}>{dictionary.newsPage.nextLabel}</ChangePageButton>
                </Box>}
            </Box>
        </>
    );

}
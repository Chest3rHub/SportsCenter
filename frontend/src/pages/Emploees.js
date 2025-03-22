import Header from "../components/Header";
import { Box, Typography } from "@mui/material";
import SmallGreenHeader from "../components/SmallGreenHeader";
import EmployeesButton from "../components/EmployeesButton";
import { SportsContext } from "../context/SportsContext";
import { useContext, useEffect, useState } from "react";
import getEmployees from "../api/getEmployees";

export default function Employees() {

    const { dictionary, token } = useContext(SportsContext);

    const [employees, setEmployees] = useState([]);
    const [loading, setLoading] = useState(true);


    useEffect(() => {
        getEmployees(token)
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
    }, []);

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
                    {employees.map((employee) => (<Box
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
                                    {employee.email}
                                </Typography>
                            </Box>
                        </Box>
                        <EmployeesButton backgroundColor={"#f0aa4f"} onClick={() => console.log('click')} minWidth={'11vw'}>
                            {dictionary.employeesPage.changePasswordLabel}
                        </EmployeesButton>
                        <EmployeesButton backgroundColor={"#F46C63"} onClick={() => console.log('click')} minWidth={'11vw'}>
                            {dictionary.employeesPage.fireLabel}
                        </EmployeesButton>
                    </Box>))}
                </Box>
            </Box>
        </>
    );

}
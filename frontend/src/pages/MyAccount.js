import { useContext, useEffect, useState } from "react";
import { Box, Typography, TextField } from "@mui/material";
import { useNavigate } from "react-router-dom";  
import GreenBackground from "../components/GreenBackground";
import OrangeBackground from "../components/OrangeBackground";
import GreenButton from '../components/buttons/GreenButton';
import { SportsContext } from "../context/SportsContext";
import Header from "../components/Header";
import CustomInput from "../components/CustomInput";
import getAccountInfo from "../api/getAccountInfo";

export default function MyAccount() {
    const { dictionary, token } = useContext(SportsContext);
    const navigate = useNavigate();

    const [userData, setUserData] = useState({
        lastName: '',
        name: '',
        address: '',
        dateOfBirth: '',
        phoneNumber: '',
        email: '',
        role: '',
        balance: 0
    });
    
    useEffect(() => {
        const fetchUserData = async () => {
          try {
            const response = await getAccountInfo(token);
            const data = await response.json();
            setUserData(data);
          } catch (error) {
              console.error("Błąd podczas pobierania danych użytkownika:", error);
          }
        };
        fetchUserData();
  }, []); 

  function handleChangePassword() {
    navigate('/change-password');
  }

  function handleAddBalance() {
    navigate('/wallet');
  }

    return (
        <GreenBackground height={"55vh"} marginTop={"2vh"}>
        <Header>{dictionary.accountPage.title}</Header>
        <OrangeBackground width="70%">

            <Box sx={{ marginBottom: "15px" }}>
              <Typography sx={{ fontSize: "1.5rem", fontWeight: "bold" }}>
              {dictionary.accountPage.personalDataLabel}
              </Typography>
            </Box>

            <Box
                sx={{
                    display: "flex",
                    flexDirection: "row",
                    justifyContent: "space-between",
                    width: "100%",
                }}
            >
            <Box sx={{ display: "flex", flexDirection: "column", gap: "1rem", marginBottom: "2vh", width: "48%"}}>
              <CustomInput
                label={dictionary.accountPage.firstNameLabel}
                type="text"
                id="firstName"
                name="firstName"
                fullWidth
                value={userData.name}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
              />
              <CustomInput
                label={dictionary.accountPage.lastNameLabel}
                type="text"
                id="lastName"
                name="lastName"
                fullWidth
                value={userData.lastName}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
              />
              <CustomInput
                label={dictionary.accountPage.addressLabel}
                type="text"
                id="address"
                name="address"
                fullWidth
                value={userData.address}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
              />
              <CustomInput
                label={dictionary.accountPage.dateOfBirthLabel}
                type="date"
                id="dateOfBirth"
                name="dateOfBirth"
                fullWidth
                value={userData.dateOfBirth}
                size="small"
                InputLabelProps={{
                  shrink: true
                }}
                InputProps={{ readOnly: true }}
                readonlyStyle
              />
              <CustomInput
                label={dictionary.accountPage.phoneNumberLabel}
                type="tel"
                id="phoneNumber"
                name="phoneNumber"
                fullWidth
                value={userData.phoneNumber}
                size="small"
                InputProps={{ readOnly: true }}
                readonlyStyle
              />
            </Box>

            <Box sx={{ width: "48%" }}>
              <Typography variant="h6" align="left">{dictionary.accountPage.role}: {userData.role}</Typography>
                  {userData.role === "Klient" && (
                    <>
                      <Typography variant="h6" align="left" sx={{ marginBottom: "20px" }}>
                        {dictionary.accountPage.balance}: {userData.balance} zł
                      </Typography>
                      {/* <Box sx={{ display: "flex", flexDirection: "column", marginBottom: "20px" }}>
                        <GreenButton onClick={handleAddBalance}>{dictionary.accountPage.addBalance}</GreenButton>
                      </Box> */}
                    </>
                  )}
              <Box sx={{ display: "flex", flexDirection: "column"}}>
                <GreenButton onClick={handleChangePassword}>{dictionary.accountPage.changePassword}</GreenButton>
              </Box>
            </Box>
            </Box>
        </OrangeBackground>
        </GreenBackground>
    );
}


import { Box, Typography } from "@mui/material";
import GreenButton from "../components/GreenButton";
import { useNavigate, useLocation } from 'react-router-dom';
import { SportsContext } from "../context/SportsContext";
import { useContext } from "react";

export default function NotFound() {

    const navigate = useNavigate();
    const location = useLocation();

    const { dictionary, toggleLanguage } = useContext(SportsContext);

    function onClick() {
        // powrot do strony glownej, moze label przycisku zmienic potem 
        navigate("/");
    }

    return (
        <Box sx={{
            marginTop: "10vh",
            textAlign: "center",
            display: "flex",
            flexDirection: "column",
            alignItems: "center",

        }}>
            <Typography sx={{
                fontSize: "50px"
            }}>
                Strona not found...
            </Typography>
            <GreenButton onClick={onClick} style={{ "max-width": "15vw" }}>{dictionary.errorPage.returnLabel}</GreenButton>
        </Box>
    );
}
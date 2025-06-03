import { Box, Typography } from "@mui/material";
import GreenButton from '../components/buttons/GreenButton';
import { useNavigate } from 'react-router-dom';
import { SportsContext } from "../context/SportsContext";
import { useContext } from "react";

export default function NotFound() {
    const navigate = useNavigate();
    const { dictionary } = useContext(SportsContext);

    const handleBackClick = () => {
        navigate("/");
    };

    return (
        <Box sx={{
            width: '64%',
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center',
            flexGrow: 1,
            marginLeft: 'auto',
            marginRight: 'auto',
            marginTop: '10vh',
        }}>
            <Box sx={{
                minHeight: '65vh',
                width: '100%',
                borderRadius: '20px',
                boxShadow: '0 10px 20px rgba(0, 0, 0, 0.2)',
                backgroundColor: 'white',
                padding: '3rem',
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                textAlign: 'center'
            }}>
                <Typography variant="h3" sx={{
                    fontWeight: 'bold',
                    color: 'black',
                    marginBottom: '2rem'
                }}>
                    {dictionary.notFoundPage.notFoundTitle}
                </Typography>

                <Typography variant="h5" sx={{
                    color: 'black',
                    marginBottom: '3rem',
                    maxWidth: '500px'
                }}>
                    {dictionary.notFoundPage.notFoundMessage}
                </Typography>

                <Box sx={{
                    width: '200px',
                    height: '200px',
                    borderRadius: '50%',
                    backgroundColor: '#FFE3B3',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.1)',
                    marginBottom: '2rem'
                }}>
                    <Typography variant="h1" sx={{ color: 'black' }}>
                        ❓
                    </Typography>
                </Box>

                <GreenButton onClick={handleBackClick} style={{ maxWidth: "15vw" }}>
                    {dictionary.notFoundPage.homeLabel }
                </GreenButton>
            </Box>
        </Box>
    );
}

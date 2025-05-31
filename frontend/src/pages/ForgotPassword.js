import { useContext } from "react";
import { SportsContext } from "../context/SportsContext";
import { Box, Typography } from "@mui/material";

export default function ForgotPassword(){
    const { dictionary } = useContext(SportsContext);

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
                    {dictionary.forgotPassword.title || "Zapomniałeś hasła?"}
                </Typography>

                <Typography variant="h5" sx={{
                    color: 'black',
                    marginBottom: '3rem',
                    maxWidth: '500px'
                }}>
                    {dictionary.forgotPassword.message || 
                    "W celu zresetowania hasła zapraszamy do recepcji klubu z dokumentem potwierdzającym tożsamość. Pracownik klubu pomoże Ci odzyskać dostęp do konta."}
                </Typography>

                <Box sx={{
                    width: '200px',
                    height: '200px',
                    borderRadius: '50%',
                    backgroundColor: '#FFE3B3',
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    boxShadow: '0 10px 20px rgba(0, 0, 0, 0.1)'
                }}>
                    <Typography variant="h1" sx={{ color: 'black' }}>
                        🏢
                    </Typography>
                </Box>
            </Box>
        </Box>
    );
}

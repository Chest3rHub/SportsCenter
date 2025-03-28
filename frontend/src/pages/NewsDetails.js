import { useLocation, useNavigate } from "react-router-dom";
import Header from "../components/Header";
import OrangeBackground from "../components/OrangeBackground";
import GreenBackground from "../components/GreenBackground";
import NewsButton from "../components/NewsButton";
import { Box, Typography } from "@mui/material";
import { SportsContext } from "../context/SportsContext";
import { useContext } from "react";

export default function NewsDetails() {
    const location = useLocation();
    const navigate = useNavigate();
    const { dictionary }= useContext(SportsContext);

    const { oneNewsDetails, offsetFromLocation} = location.state || {};
    console.log(oneNewsDetails);
    function handleCancel() {
        navigate('/news', {
            state: { offsetFromLocation }  
          });
    }
    return (<>
        <GreenBackground gap={"4vh"} height={"76.5vh"}>
            <Header>
                {oneNewsDetails.title}
            </Header>
            <OrangeBackground width={"80%"}
                maxHeight={"53vh"}
                minHeight={"53vh"}
                overflow={"hidden"}
                height={"53vh"}>
                <Box
                    sx={{
                        display: 'grid',
                        gridTemplateColumns: '33% 63%',
                        gap: '1vw',
                        minHeight: '47vh',
                        marginBottom: '1vh',
                    }}
                >
                    <Box
                        sx={{
                            backgroundColor: 'white',
                            height: '28vh',
                            width: '28vh',
                            display: 'flex',
                            justifyContent: 'center',
                            borderRadius: '20px',
                            // lewa kolumna ze zdjeciem
                        }}
                    >
                        <Typography sx={{
                            margin: 'auto',
                        }}>
                            image...
                        </Typography>
                    </Box>

                    <Box
                        sx={{
                            overflowY: 'auto',        
                            maxHeight: '45vh',
                            borderRadius:'5px',
                            // mozna przewijac tekst w dol jak jest zbyt dlugi 
                            // prawa kolumna z contentem
                        }}
                    >
                        <Typography sx={{
                            color: "black",
                            textAlign: "left",
                            overflow: "hidden",
                            fontSize:'1.2rem',
                        }}>
                            {oneNewsDetails.content}

                        </Typography>
                    </Box>
                </Box>
                <Box>
                    <NewsButton backgroundColor={"#F46C63"} onClick={handleCancel} minWidth={'10vw'}>
                        {dictionary.newsDetailsPage.returnLabel}
                    </NewsButton>
                </Box>

            </OrangeBackground>
        </GreenBackground>
    </>);
}
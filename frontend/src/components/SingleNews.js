import { Typography, Box } from "@mui/material";
import OrangeBackground from "./OrangeBackground";
import { SportsContext } from "../context/SportsContext";
import { useContext } from "react";
import NewsButton from "./NewsButton";

export default function SingleNews({ oneNewsDetails }) {
    const {role, dictionary} = useContext(SportsContext);
    console.log(oneNewsDetails)
    return (
        <OrangeBackground 
            width={"80%"} 
            maxHeight={"12vh"} 
            minHeight={"12vh"} 
            overflow={"hidden"} 
            height={"12vh"}
            position={ "relative" }
        >
            <Typography sx={{
                color: "black",
                fontWeight: "bold",
                textAlign: "left"
            }}>
                {oneNewsDetails.title}
            </Typography>
            <Typography sx={{
                color: "black",
                textAlign: "left",
                overflow: "hidden",          
                display: "-webkit-box",      
                WebkitLineClamp: 2,        
                WebkitBoxOrient: "vertical",
                textOverflow: "ellipsis",
                
            }}>
                {oneNewsDetails.content}
            </Typography>
            <Box sx={{
                position: "absolute", 
                bottom: 0,           
                right: 0,             
                padding: "10px",      
                background: "transparent", 
                display: "flex",       
                gap: "1vw", 
            }}>
                <NewsButton backgroundColor={"#8edfb4"} onClick={() => console.log("click")}>{dictionary.newsPage.showLabel}</NewsButton>
                { (role ==="Wlasciciel" || role ==="Pracownik administracyjny") &&
                 <NewsButton backgroundColor={"#f0aa4f"} onClick={() => console.log("click")}>{dictionary.newsPage.editLabel}</NewsButton>}
                { (role ==="Wlasciciel" || role ==="Pracownik administracyjny") &&
                 <NewsButton backgroundColor={"#F46C63"} onClick={() => console.log("click")}>{dictionary.newsPage.removeLabel}</NewsButton>}

            </Box>
        </OrangeBackground>
    );
}

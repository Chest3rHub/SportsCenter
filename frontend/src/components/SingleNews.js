import { Typography, Box } from "@mui/material";
import OrangeBackground from "./OrangeBackground";
import { SportsContext } from "../context/SportsContext";
import { useContext } from "react";
import NewsButton from "./NewsButton";
import removeNews from "../api/removeNews";
import { useNavigate } from "react-router-dom";

export default function SingleNews({ oneNewsDetails, onNewsDeleted }) {
    const {role, dictionary, token} = useContext(SportsContext);
    const navigate = useNavigate();

    // te stany sa do walidacji, moze pozniej dodac jesli nie uda sie request?

    //const [loading, setLoading] = useState(false);
    //const [error, setError] = useState(null);
    //const [success, setSuccess] = useState(false);

    const handleDelete = async (id) => {
        try {
          //  setLoading(true);  
          //  setError(null);  
            const response = await removeNews(id, token);

            if (response.ok) {
              //  setSuccess(true); 
                // tutaj aktualizuje liste newsow w nadrzednym komponencie
                onNewsDeleted(id);
            } else {
              //  throw new Error('Failed to remove news');
            }
        } catch (err) {
          //  setError(err.message);  
        } finally {
          //  setLoading(false); 
        }
    };

    const handleEdit = (oneNewsDetails) => {
        navigate('/edit-news', {
          state: { oneNewsDetails }  
        });
      };
      const handleShow = (id) => {
        navigate(`/news/${id}`, {
          state: { oneNewsDetails }  
        });
    };

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
                <NewsButton backgroundColor={"#8edfb4"} onClick={() => handleShow(oneNewsDetails.id)}>{dictionary.newsPage.showLabel}</NewsButton>
                { (role ==="Wlasciciel" || role ==="Pracownik administracyjny") &&
                 <NewsButton backgroundColor={"#f0aa4f"} onClick={() => handleEdit(oneNewsDetails)}>{dictionary.newsPage.editLabel}</NewsButton>}
                { (role ==="Wlasciciel" || role ==="Pracownik administracyjny") &&
                 <NewsButton backgroundColor={"#F46C63"} onClick={() => handleDelete(oneNewsDetails.id)}>{dictionary.newsPage.removeLabel}</NewsButton>}

            </Box>
        </OrangeBackground>
    );
}

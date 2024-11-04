import GreenButton from "../components/GreenButton";
import { useNavigate, useLocation } from 'react-router-dom';

export default function Error() {
    const navigate = useNavigate();
    const location = useLocation();

    
    const errorMessage = location.state.message;

    
    function onClick() {
        navigate(-1); 
    }
    return (
        <>
        <div style={{marginTop:"10vh", fontSize: "50px"}}></div>
         <p>{errorMessage}</p>
        <div style={{display: "flex", justifyContent:"center"}}>
            <GreenButton onClick={onClick} style={{"max-width": "15vw"}}>Wróć</GreenButton>
        </div>
        </>
    );
}
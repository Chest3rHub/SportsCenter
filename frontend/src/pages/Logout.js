import { useEffect, useContext } from "react";
import { SportsContext } from "../context/SportsContext";

export default function Logout(){
    const { role, token, setRole, setToken} = useContext(SportsContext);
    useEffect(()=>{
        setRole('Anonim');
        setToken(null);
    },[]);
    return (
        <>
            <div style={{marginTop:"10vh", fontSize: "50px"}}>Wylogowales sie z serwisu...</div>
        </>
    );
}
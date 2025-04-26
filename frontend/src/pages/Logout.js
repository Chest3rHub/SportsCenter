import { useEffect, useContext } from "react";
import { SportsContext } from "../context/SportsContext";

export default function Logout(){
    const { role, token, setRole, setToken} = useContext(SportsContext);
    useEffect(()=>{
        setRole('Anonim');
        setToken(null);
        document.cookie = "accessToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    },[]);
    return (
        <>
            <div style={{marginTop:"10vh", fontSize: "50px"}}>Wylogowales sie z serwisu...</div>
        </>
    );
}
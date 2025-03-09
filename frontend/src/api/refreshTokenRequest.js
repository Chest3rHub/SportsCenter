import API_URL from "../appConfig";
import { SportsContext } from '../context/SportsContext';
import { useContext } from "react";

export default async function refreshTokenRequest(){
    
    const {token} = useContext(SportsContext);

    return fetch(`${API_URL}/Users/login`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
      });
}
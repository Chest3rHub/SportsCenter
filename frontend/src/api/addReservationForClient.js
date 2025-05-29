import API_URL from "../appConfig";

export default async function addReservationForClient(payload, token){
    return fetch(`${API_URL}/Reservation/Create-single-reservation-for-client`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload),
        credentials: 'include',
      });
}
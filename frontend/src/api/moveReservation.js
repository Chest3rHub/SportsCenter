import API_URL from "../appConfig";

export default async function moveReservation(payload){
    return fetch(`${API_URL}/Reservation/Move-reservation`, { 
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload),
        credentials: 'include',
      });
}
import API_URL from "../appConfig";

export default async function addRecurringReservationForClient(payload, token){
    return fetch(`${API_URL}/Reservation/Create-recurring-reservation`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload),
        credentials: 'include',
      });
}
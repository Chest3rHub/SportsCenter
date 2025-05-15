import API_URL from "../appConfig";

export default async function cancelReservation(clientEmail, id){
    return fetch(`${API_URL}/Reservation/cancel-client-reservation`, { 
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          clientEmail: clientEmail,
          reservationId: id,
        }),
        credentials: 'include',
      });
}
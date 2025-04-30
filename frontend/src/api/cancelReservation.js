import API_URL from "../appConfig";

export default async function cancelReservation(id){
    return fetch(`${API_URL}/Reservation/cancel-reservation`, { 
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          reservationId: id,
        }),
        credentials: 'include',
      });
}
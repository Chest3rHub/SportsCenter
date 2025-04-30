import API_URL from "../appConfig";

export default async function moveReservation(formData, id){
    return fetch(`${API_URL}/Reservation/Move-reservation`, { 
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            reservationId: id,
            newStartTime: formData.newStartTime,
            newEndTime: formData.newEndTime,
        }),
        credentials: 'include',
      });
}
import API_URL from "../appConfig";

export default async function getReservationInfo(token, id){
    return fetch(`${API_URL}/Reservation/get-reservation-with-id-${id}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        },
        credentials: 'include',
      });
}
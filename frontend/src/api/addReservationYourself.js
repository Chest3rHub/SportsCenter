import API_URL from "../appConfig";

export default async function addReservationYourself(payload, token) {
    return fetch(`${API_URL}/Reservation/Create-single-reservation-yourself`, { 
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload),
        credentials: 'include',
    });
}

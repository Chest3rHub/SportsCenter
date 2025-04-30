import API_URL from "../appConfig";

export default async function getClientReservations(offset) {
    return fetch(`${API_URL}/Reservation/get-your-reservations?offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}
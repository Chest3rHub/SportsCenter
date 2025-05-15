import API_URL from "../appConfig";

export default async function getAllReservations(offset) {
    return fetch(`${API_URL}/Reservation/get-all-reservations?offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}
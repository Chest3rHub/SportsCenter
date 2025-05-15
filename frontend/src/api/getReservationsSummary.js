import API_URL from "../appConfig";

export default async function getActivitySummary(token, startDate, endDate, offset) {
    return fetch(`${API_URL}/Reservation/reservation-summary?startDate=${startDate}&endDate=${endDate}&offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
    credentials: 'include',
    });
}

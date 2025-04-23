import API_URL from "../appConfig";

export default async function getClientsByAge(token, minAge, maxAge, offset) {
    return fetch(`${API_URL}/Clients/byAge/?minAge=${Number(minAge)}&maxAge=${Number(maxAge)}&offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}

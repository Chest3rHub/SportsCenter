import API_URL from "../appConfig";

export default async function getClientsByAge(token, minAge, maxAge, offset = 0) {
    const url = `${API_URL}/Clients/byAge?minAge=${minAge}&maxAge=${maxAge}&offset=${offset}`;

    return fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include'
    });
}

import API_URL from "../appConfig";

export default async function getClients(token, offset) {
    return fetch(`${API_URL}/Clients/get-clients/?offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}

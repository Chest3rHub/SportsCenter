import API_URL from "../appConfig";

export default async function getClients(token, offset) {
    return fetch(`${API_URL}/Clients/get-clients/?offset=${offset}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        }
    });
}

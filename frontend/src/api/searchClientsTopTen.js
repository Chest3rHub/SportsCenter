import API_URL from "../appConfig";

export default async function searchClientsTopTen(text) {
    const url = text
        ? `${API_URL}/Clients/top10?text=${encodeURIComponent(text)}`
        : `${API_URL}/Clients/top10`;

    return fetch(url, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}

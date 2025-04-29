import API_URL from "../appConfig";

export default async function getCourts() {
    const response = await fetch(`${API_URL}/SportsCenterManagement/get-sports-club-courts`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });

    const data = await response.json();
    return data;
}
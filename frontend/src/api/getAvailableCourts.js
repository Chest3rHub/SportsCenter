import API_URL from "../appConfig";

export default async function getAvailableCourts(startTime, endTime) {

    const response =  await fetch(`${API_URL}/SportsCenterManagement/get-available-sports-club-courts?startTime=${startTime}&endTime=${endTime}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
    if (!response.ok) {
        throw new Error('Failed to fetch trainers');
    }
    return await response.json()
}
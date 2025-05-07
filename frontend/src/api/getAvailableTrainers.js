import API_URL from "../appConfig";

export default async function getAvailableTrainers(startTime, endTime) {
    
    const response = await fetch(`${API_URL}/Employees/get-available-trainers?startTime=${startTime}&endTime=${endTime}`, {
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
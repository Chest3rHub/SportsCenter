import API_URL from "../appConfig";

export default async function getTrainers(offset=0) {
    const response = await fetch(`${API_URL}/Employees/get-trainers?offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
    
    if (!response.ok) {
        throw new Error('Failed to fetch trainers');
    }

    const data = await response.json();
    console.log("Trainers data:", data);
    return data;
}
import API_URL from "../appConfig";

export default async function getTrainers() {
    const response = await fetch(`${API_URL}/Employees/get-trainers`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });

    const data = await response.json();
    return data;
}
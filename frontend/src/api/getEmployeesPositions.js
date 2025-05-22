import API_URL from "../appConfig";

export default async function getEmployeesPositions() {
     const response = await fetch(`${API_URL}/Employees/get-employees-positions`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
    if (!response.ok) {
        throw new Error('Failed to fetch employee positions');
    }

    return response.json();
}
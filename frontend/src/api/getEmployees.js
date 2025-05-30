import API_URL from "../appConfig";

export default async function getEmployees(offset) {
    return fetch(`${API_URL}/Employees?offset=${offset}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
    });
}
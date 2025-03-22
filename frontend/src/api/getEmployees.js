import API_URL from "../appConfig";

export default async function getEmployees(token){
    return fetch(`${API_URL}/Employees`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      });
}